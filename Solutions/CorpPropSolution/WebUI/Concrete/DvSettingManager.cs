using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.BusinessProcesses.Entities;
using Base.DAL;
using Base.UI.DetailViewSetting;
using Base.UI.Service;

namespace WebUI.Concrete
{
    public class DvSettingManager : IDvSettingManager
    {
        public DvSetting GetSettingForMnemonic(IUnitOfWork unitOfWork, string mnemonic)
        {
            return null;
        }

        public bool HasSettingsForType(IUnitOfWork unitOfWork, Type type)
        {
            string objectType = type.GetTypeName();

            return unitOfWork.GetRepository<DvSettingForType>().All().Any(x => x.ObjectType == objectType);
        }

        public IEnumerable<DvSetting> GetSettingsForType(IUnitOfWork unitOfWork, Type type, BaseObject obj)
        {
            if (obj == null) return null;

            var settings = new List<DvSetting>();

            var bpObject = obj as IBPObject;

            if (bpObject != null)
            {
                var wfctx = unitOfWork.GetRepository<WorkflowContext>().Find(bpObject.WorkflowContextID);

                if (wfctx != null)
                {
                    var stages = wfctx.CurrentStages;

                    foreach (var stage in stages)
                    {
                        //TODO: необходимо обойти всю иерархию декомпозиции!!!
                        if (stage.Position?.CurrentWorkflowContainer?.DvSettingID != null)
                            settings.Add(stage.Position.CurrentWorkflowContainer.DvSetting);

                        var dvSetting = stage.Stage.DvSetting;

                        if (dvSetting != null)
                        {
                            settings.Add(dvSetting);

                            Type objType = obj.GetType().GetBaseObjectType();
                            string strType = objType.GetTypeName();

                            if (dvSetting.ObjectType != strType)
                            {
                                var childs = unitOfWork.GetRepository<DvSettingForType>()
                                    .All()
                                    .Where(x => x.sys_all_parents.Contains(dvSetting.ID.ToString()) && !x.Hidden).ToList();

                                foreach (var child in childs)
                                {
                                    var childType = Type.GetType(child.ObjectType);
                                    if (childType == null)
                                    {
                                        throw new Exception("child type is null");
                                    }


                                    if (childType.IsAssignableFrom(objType) || childType == objType)
                                    {
                                        settings.Add(child);
                                    }
                                }
                            }
                            var parents = unitOfWork.GetRepository<DvSettingForType>()
                                .All()
                                .Where(x => dvSetting.sys_all_parents.Contains(x.ID.ToString())).ToList();

                            settings.AddRange(parents);
                        }
                    }
                }
            }

            return settings.OrderBy(x => x.ID);
        }
    }
}