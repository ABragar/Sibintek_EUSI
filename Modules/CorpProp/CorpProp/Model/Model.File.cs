using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Base;
using Base.DAL;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Security;
using CorpProp.Extentions;
using CorpProp.Services.Document;

namespace CorpProp.Model
{
    /// <summary>
    /// Представляет модель объектов имущества.
    /// </summary>
    public static class FileModel
    {
        public static bool CheckReadOnlyAccess(IUnitOfWork work, ISibAccessableObject sibAccessableObject)
        {
            bool UsersRolesEquality(
                IUnitOfWork uow,
                int? userId1,
                int? userId2)
            {
                var user1Roles = SecurityHelper.FindUserRoles(work, userId1).ToList();
                var user2Roles = SecurityHelper.FindUserRoles(work, userId2).ToList();
                var crossRoles = from user1Role in user1Roles
                                 join user2Role in user2Roles
                                 on user1Role.ID equals user2Role.ID
                                 select user1Role;
                var currentUserRolesAndfileCardAuthorRolesEqual =
                    crossRoles.Count() == user1Roles.Count();
                return currentUserRolesAndfileCardAuthorRolesEqual;
            }

            var currentSibUser = Base.Ambient.AppContext.SecurityUser.FindLinkedSibUser(work);
            //ОГ, к которому привязан Автор
            var currentUserSociety = currentSibUser?.SocietyID;
            //Автора документа
            var fileCardAuthorUserId = work.GetRepository<SibUser>().Find(sibAccessableObject.CreateUserID)?.UserID;


            if (Base.Ambient.AppContext.SecurityUser.ID == fileCardAuthorUserId ||
                Base.Ambient.AppContext.SecurityUser.IsFromCauk(work) ||
                Base.Ambient.AppContext.SecurityUser.IsAdmin)
            {
                return false;
            }
            else
            {
                switch (sibAccessableObject.FileCardPermission?.AccessModifier)
                {
                    case null:
                        return false;
                    case AccessModifier.AuthorSociety
                        when currentUserSociety == sibAccessableObject.SocietyID:
                    return true;
                    case
                        AccessModifier.AuthorSocietyWithEqualRoles
                        when currentUserSociety == sibAccessableObject.SocietyID
                             &&
                             UsersRolesEquality(
                                                work,
                                                Base.Ambient.AppContext.SecurityUser.ID,
                                                fileCardAuthorUserId):
                    return true;
                    case AccessModifier.AuthorOnly:
                    return true;
                    case AccessModifier.Everyone:
                    return false;
                    default:
                    return false;
                }
            }

        }
        public static DetailViewBuilder<T> DefaultSettingsBySibAccessableObject<T>(this DetailViewBuilder<T> builder, Action<IUnitOfWork, T, CommonEditorVmSett<T>> action = null)
            where T : BaseObject, ISibAccessableObject
        {
            builder.DefaultSettings(
                                    (work, card, arg3) =>
                                    {
                                        action?.Invoke(work, card, arg3);
                                        var ro = CheckReadOnlyAccess(work, card);
                                        if (ro)
                                            arg3.SetReadOnlyAll(true);
                                    });

            return builder;
        }

        /// <summary>
        /// Инициализация моделей объектов FileCard, CardFolder, FileData.
        /// </summary>
        /// <param name="context"></param>
        public static void Init(IInitializerContext context)
        {
            #region Document

            context.CreateVmConfig<FileCardPermission>()
                    .Title("Типы прав доступа к карточкам документов")
                    .ListView(x => x.Title("Разграничения прав доступа"))
                    .DetailView(x => x.Title("Разграничение прав доступа"))
                    .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<CardFolder>()
                .Title("Папка")
                .ListView(x => x.Title("Папки"))
                .DetailView(x => x.Title("Папка"))
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<FileCard>("FileCard")
                .Service<FileCardNonCategoryService>()
                .Title("Документ АИС КС")
                   .ListView(
                             x => x.Title("Документы АИС КС")
                                   .ListViewCategorizedItem(itemBuilder => itemBuilder.HiddenTree(true))
                                   .Columns(factory => factory.Add(card => card.Category_, builder => builder.Visible(true))
                                                              .Add(card => card.Name, builder => builder.Visible(true))
                                                              .Add(card => card.Number, builder => builder.Visible(true))
                                                              .Add(card => card.PersonFullName, builder => builder.Visible(true))
                                                              .Add(card => card.Society, builder => builder.Visible(true))
                                                              .Add(card => card.Description, builder => builder.Visible(true))
                                                              .Add(card => card.FileCardDate, builder => builder.Visible(true))
                                                              )
                                   .IsMultiSelect(true)
                .Toolbar(factory => factory
                    .Add("GetMergeToolbar", "FileCard")
                    .Add("GetMassDownloadToolbar", "FileCard")))
                .DetailView(x => x.Title("Документ АИС КС")
                                  .DefaultSettingsBySibAccessableObject())
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfigOnBase<FileCard>("FileCard", "FileMany_FileOnes")
                .Service<FileCardForFileCardManyService>();

            context.CreateVmConfigOnBase<FileCard>("FileCard", "FileCardTree")
                   .Service<IFileCardService>()
                   .ListView(
                             x => x.Title("Документы АИС КС")
                                   .ListViewCategorizedItem(itemBuilder => itemBuilder.HiddenTree(false))
                                   .Columns(factory => factory.Add(card => card.Category_, builder => builder.Visible(false))
                                                              .Add(card => card.Name, builder => builder.Visible(true))
                                                              .Add(card => card.Number, builder => builder.Visible(true))
                                                              .Add(card => card.PersonFullName, builder => builder.Visible(true))
                                                              .Add(card => card.Description, builder => builder.Visible(true))
                                                              .Add(card => card.Subject, builder => builder.Visible(false))
                                                              .Add(card => card.FileCardPermission, builder => builder.Visible(false))

                                                              ))
                                   .DetailView(builder => builder.DefaultSettingsBySibAccessableObject());


            context.CreateVmConfigOnBase<FileCard>("FileCard", "Cadastral_RightDocs")
               .Title("Документ АИС КС")
               .DetailView(x => x.Title("Документ АИС КС").DefaultSettingsBySibAccessableObject()
               .Editors(edit => edit
                .Add(ed => ed.DocTypeCode, ac => ac.Title("Код документа").Visible(true))
                .Add(ed => ed.DocTypeName, ac => ac.Title("Тип документа").Visible(true))
                .Add(ed => ed.Name, ac => ac.Title("Наименование").Visible(true))
                .Add(ed => ed.Description, ac => ac.Title("Содержание документа").Visible(true))
                .Add(ed => ed.Number, ac => ac.Title("Номер документа").Visible(true))
                .Add(ed => ed.SerialNumber, ac => ac.Title("Серия документа").Visible(true))
                .Add(ed => ed.FileCardDate, ac => ac.Title("Дата документа").Visible(true))
                .Add(ed => ed.Issuer, ac => ac.Title("Организация, выдавшая документ").Visible(true))
                .Add(ed => ed.ExecutorName, ac => ac.Title("Исполнитель").Visible(false))
               ))
               .ListView(x => x.Title("Документы АИС КС")
               .Toolbar(factory => factory.Add("GetMergeToolbar", "FileCard"))
               .Columns(col => col
               .Add(c => c.DocTypeCode, ac => ac.Title("Код документа").Visible(true).Order(1))
               .Add(c => c.DocTypeName, ac => ac.Title("Тип документа").Visible(true).Order(2))
               .Add(c => c.Name, ac => ac.Title("Наименование").Visible(true).Order(3))
               .Add(c => c.Description, ac => ac.Title("Содержание документа").Visible(true).Order(4))
               .Add(c => c.Number, ac => ac.Title("Номер документа").Visible(true).Order(5))
               .Add(c => c.SerialNumber, ac => ac.Title("Серия документа").Visible(true).Order(6))
               .Add(c => c.FileCardDate, ac => ac.Title("Дата документа").Visible(true).Order(7))
               .Add(c => c.Issuer, ac => ac.Title("Организация, выдавшая документ").Visible(true).Order(8))
               .Add(c => c.PersonFullName, ac => ac.Visible(false))
               .Add(c => c.Category_, ac => ac.Visible(false))
               .Add(c => c.ExecutorName, ac => ac.Visible(false))
               ))
               .LookupProperty(x => x.Text(t => t.Name));


            context.CreateVmConfig<FileCardMany>()
               .Service<IFileCardManyService>()
               .Title("Пакет документов")
               .ListView(x => x.Title("Пакеты документов"))
               .DetailView(x => x.Title("Пакет документов")
               .Editors(ed => ed.AddManyToManyRigthAssociation<FileCardOneAndFileCardMany>("FileCardMany_FileCardOnes"
               , y => y.TabName("[1]Карточки одиночных документов").Mnemonic("FileMany_FileOnes")))
               ).LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<FileData>("FileCardOneVersions")
                   .Service<IFileDataVersionsService>()
                   .ListView(x => x.Title("FileCardOneVersions")
                   //.ListView(x => x.Title("Версии файлов")
                   .Columns(config => config.Add(data => data.CreationDate, h => h.Visible(true)))
                   .HiddenActions(new[] { LvAction.Create, LvAction.Edit, LvAction.Delete, LvAction.Link })
                   .DataSource(ds => ds.Sort(factory => factory.Add(data => data.CreationDate, ListSortDirection.Descending))));

            context.CreateVmConfig<FileCardOne>()
               .Service<IFileCardOneService>()
               .Title("Одиночный документ")
               .ListView(x => x.Title("Одиночные документы"))
               .DetailView(x => x.Title("Одиночный документ")
                           .DefaultSettingsBySibAccessableObject()
                           .Editors(edt => edt
                           .AddOneToManyAssociation<FileData>("FileCardOneToManyAssociation", b => b.TabName("[1]Версии файлов")
                                                                                           .IsReadOnly(true)
                                                                                           .FilterExtended(FileDataHelper.VersionsFilter)
                                                                                           .Mnemonic("FileCardOneVersions"))
                           .AddManyToManyLeftAssociation<FileCardOneAndFileCardMany>("FileCardOne_FileCardManys",
                                                                                    y => y.TabName("[1]Пакеты карточек документов")
                                                                                    .Mnemonic("FileMany_FileOnes")
                                                                                    )
                ))
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<ViewSettingsByMnemonic>()
                   .ListView(builder => builder.Title("Настройки отображения"))
                   .DetailView(builder => builder.Title("Настройка отображения"));

            #endregion



        }
    }
}
