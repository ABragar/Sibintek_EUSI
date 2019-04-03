using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Base.Audit.Entities;
using Base.Entities.Complex;
using Base.Helpers;
using Base.Service;
using Base.Settings;
using Base.Events;
using Base.UI;
using Base.Utils.Common.Maybe;
using AppContext = Base.Ambient.AppContext;
using Base.Events.Auth;
using Base.Events.Registration;

namespace Base.Audit.Services
{
    public class AuditItemService : BaseObjectService<AuditItem>, IAuditItemService, IAuditItemAuthService
    {

        private readonly IViewModelConfigService _view_model_config_service;
        private readonly ISettingService<AuditSetting> _setting_service;
        
        public AuditItemService(ISettingService<AuditSetting> setting_service, IBaseObjectServiceFacade facade, IViewModelConfigService view_model_config_service)
            : base(facade)
        {

            _setting_service = setting_service;
            _view_model_config_service = view_model_config_service;
        }

        private AuditSetting GetConfig()
        {
            return _setting_service.Get();
        }

        private AuditItem Create()
        {
            var item = new AuditItem
            {
                Date = AppContext.DateTime.Now
            };
            // SystemUser has zero in SecurityUser.ID
            if (AppContext.SecurityUser != null && AppContext.SecurityUser.ID > 0)
            {
                item.UserID = AppContext.SecurityUser.ID;
            }
            return item;
        }

        public void OnEvent(IOnCreate<BaseObject> evnt)
        {
            var config = GetConfig();
            if (!config.IsAudit(evnt.Modified.GetType())) return;

            var item = Create();
            item.Type = TypeAuditItem.CreateObject;
            item.Entity = new LinkBaseObject(evnt.Modified.GetType(), evnt.Modified.ID);
            item.SessionId = evnt.UnitOfWork.ID;

            var repository = evnt.UnitOfWork.GetRepository<AuditItem>();
            repository.Create(item);
            evnt.UnitOfWork.SaveChanges();
        }

        public void OnEvent(IOnUpdate<BaseObject> evnt)
        {
            var config = GetConfig();
            if (!config.IsAudit(evnt.Modified.GetType())) return;

            var repository = evnt.UnitOfWork.GetRepository<AuditItem>();

            var linkBaseObject = new LinkBaseObject(evnt.Modified.GetType(), evnt.Modified.ID);

            var item = repository.All()
                .FirstOrDefault(x =>
                    x.Entity.ID == linkBaseObject.ID &&
                    x.Entity.TypeName == linkBaseObject.TypeName &&
                    x.SessionId == evnt.UnitOfWork.ID &&
                    x.UserID == AppContext.SecurityUser.ID);

            if (item == null)
            {
                item = Create();
                item.Type = TypeAuditItem.UpdateObject;
                item.Entity = linkBaseObject;
                item.SessionId = evnt.UnitOfWork.ID;
                Diff(item, evnt.Original, evnt.Modified);
                repository.Create(item);
            }
            else
            {                
                item.Date = AppContext.DateTime.Now;
                Diff(item, evnt.Original, evnt.Modified);
                repository.Update(item);
            }

            evnt.UnitOfWork.SaveChanges();
        }

        public void OnEvent(IOnDelete<BaseObject> evnt)
        {
            var config = GetConfig();
            if (!config.IsAudit(evnt.Modified.GetType())) return;

            var item = Create();
            item.Type = TypeAuditItem.DeleteObject;
            item.Entity = new LinkBaseObject(evnt.Modified.GetType(), evnt.Modified.ID);
            item.SessionId = evnt.UnitOfWork.ID;

            var repository = evnt.UnitOfWork.GetRepository<AuditItem>();
            repository.Create(item);
            evnt.UnitOfWork.SaveChanges();
        }

        private void Diff(AuditItem audit_item, BaseObject original, BaseObject modified)
        {
            if (original == null || audit_item.Type != TypeAuditItem.UpdateObject) return;

            var type = audit_item.Entity.GetTypeBo();

            var editors = _view_model_config_service.GetEditors(type);

            var ignore = new[] { "ID", "RowVersion" };
            
            if (audit_item.Diff == null)
                audit_item.Diff = new List<DiffItem>();

            foreach (var editor in editors.Where(m => !ignore.Contains(m.PropertyName) && m.PropertyName != null))
            {
                var property_info = type.GetProperty(editor.PropertyName);
                
                var diff_item = new DiffItem()
                {                    
                    Property = editor.Title,
                    Member = editor.PropertyName
                };

                var original_pr_value = property_info.GetValue(original);
                var modified_pr_value = property_info.GetValue(modified);

                if (original_pr_value == null && modified_pr_value == null)
                    continue;

                if (editor.IsPrimitive)
                {
                    if (original_pr_value?.Equals(modified_pr_value) ?? modified_pr_value?.Equals(null) ?? false)
                        continue;

                    diff_item.OldValue = original_pr_value?.ToString();
                    diff_item.NewValue = modified_pr_value?.ToString();

                    if (diff_item.OldValue != diff_item.NewValue)
                        audit_item.Diff.Add(diff_item);
                }
                else
                {
                    if (editor.Relationship == Relationship.OneToMany)
                    {
                        if (typeof (IEasyCollectionEntry).IsAssignableFrom(editor.PropertyType.GetGenericArguments()[0]))
                        {
                            //var original_collection = (original_pr_value as IEnumerable<IEasyCollectionEntry>) ?? new List<IEasyCollectionEntry>();
                            //var modified_collection = (modified_pr_value as IEnumerable<IEasyCollectionEntry>) ?? new List<IEasyCollectionEntry>();

                            //var o_ids = original_collection.Select(x => x.ObjectID).ToArray();
                            //var m_ids = modified_collection.Select(x => x.ObjectID).ToArray();

                            //foreach (var id in o_ids.Except(m_ids))
                            //{
                            //    var obj = original_collection.Single(x => x.ObjectID == id);
                            //}

                            //foreach (var id in m_ids.Except(o_ids))
                            //{
                            //    var obj = modified_collection.Single(x => x.ObjectID == id);
                            //}
                        }
                    }
                    else if (editor.Relationship == Relationship.ManyToMany)
                    {

                    }
                    else if (editor.Relationship == Relationship.One)
                    {
                        string lookup = editor.ViewModelConfig.LookupProperty.Text ?? "ID";
                        DiffBaseObject(audit_item, diff_item, original_pr_value as IBaseObject, modified_pr_value as IBaseObject, lookup);
                    }
                }
            }
        }

        private void DiffBaseObject(AuditItem audit_item, DiffItem diff_item, IBaseObject original, IBaseObject modified, string lookup)
        {
            int original_id = 0;
            int modified_id = 0;

            if (original != null) original_id = original.ID;

            if (modified != null) modified_id = modified.ID;

            if (original_id == modified_id) return;

            if (original != null)
            {
                diff_item.OldValue =
                    //$"{original_id}; {original.GetType().GetProperty(lookup).GetValue(original)}";
                    $"{original.GetType().GetProperty(lookup).GetValue(original)}";
            }

            if (modified != null)
            {
                diff_item.NewValue =                    
                    //$"{modified_id}; {modified.GetType().GetProperty(lookup).GetValue(modified)}";
                    $"{modified.GetType().GetProperty(lookup).GetValue(modified)}";
            }

            audit_item.Diff.Add(diff_item);
        }

        #region Auth Events
        public void OnEvent(IOnLogOn<IAuthResult> evnt)
        {
            ProcessAuthEvent(evnt, TypeAuditItem.LogOn);
        }

        public void OnEvent(IOnLogOff<IAuthResult> evnt)
        {
            ProcessAuthEvent(evnt, TypeAuditItem.LogOf);
        }

        public void OnEvent(IOnLogOnError<IAuthResult> evnt)
        {
            ProcessAuthEvent(evnt, TypeAuditItem.LogOnError);
        }

        public void OnEvent(IOnAccountRegistered<IRegisterResult> evnt)
        {
            var item = Create();
            var user = evnt.UnitOfWork.GetRepository<Base.Security.User>().Filter(f => f.SysName.Equals(evnt.Result.Login)).FirstOrDefault();
            item.Type = TypeAuditItem.AccountRegistered;
            item.Entity = CreateAuditRegisterSubItem(evnt);
            item.SessionId = evnt.UnitOfWork.ID;
            item.User = user;
            item.UserLogin = evnt.Result.Login;

            var repository = evnt.UnitOfWork.GetRepository<AuditItem>();
            repository.Create(item);
            evnt.UnitOfWork.SaveChanges();
        }

        private bool IsLoginAuditEnabled
        {
            get { return GetConfig().RegisterLogIn; }
        }

        private void ProcessAuthEvent(IAuthEvent<IAuthResult> evnt, TypeAuditItem auditItemType)
        {
            //if (IsLoginAuditEnabled)
            {
                var item = Create();
                var user = evnt.UnitOfWork.GetRepository<Base.Security.User>().Filter(f => f.SysName.Equals(evnt.AuthResult.Login)).FirstOrDefault();
                item.Type = auditItemType;
                item.SessionId = evnt.UnitOfWork.ID;
                item.Entity = CreateAuditAuthSubItem(evnt);
                item.User = user;
                item.UserLogin = evnt.AuthResult.Login;

                var repository = evnt.UnitOfWork.GetRepository<AuditItem>();
                repository.Create(item);
                evnt.UnitOfWork.SaveChanges();
            }
        }

        private static LinkBaseObject CreateAuditAuthSubItem(IAuthEvent<IAuthResult> evnt)
        {
            var auditSubItem = new AuditAuthResult(evnt.AuthResult.Login, evnt.AuthResult.Status);
            var repositorySubItem = evnt.UnitOfWork.GetRepository<AuditAuthResult>();
            repositorySubItem.Create(auditSubItem);
            evnt.UnitOfWork.SaveChanges();

            return new LinkBaseObject(auditSubItem.GetType(), auditSubItem.ID);
        }

        private static LinkBaseObject CreateAuditRegisterSubItem(IRegisterEvent<IRegisterResult> evnt)
        {
            var auditSubItem = new AuditRegisterResult(evnt.Result.Login, evnt.Result.Status);
            var repositorySubItem = evnt.UnitOfWork.GetRepository<AuditRegisterResult>();
            repositorySubItem.Create(auditSubItem);
            evnt.UnitOfWork.SaveChanges();

            return new LinkBaseObject(auditSubItem.GetType(), auditSubItem.ID);
        }
        #endregion
    }
}
