using Base;
using Base.DAL;
using Base.Enums;
using Base.Security;
using CorpProp.RosReestr.Model;
using System.Collections.Generic;
using System.Linq;

namespace CorpProp.RosReestr
{
    public class Initializer : IModuleInitializer
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="context"></param>
        public void Init(IInitializerContext context)
        {
            //инициализация модели - создание кофигураторов ListView и DetailView
            RosReestrModel.Init(context);

            context.DataInitializer("CorpProp.RosReestr", "0.1", () =>
            {
                Seed(context.UnitOfWork);
            });
        }

        /// <summary>
        /// Наполнение БД.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        private void Seed(IUnitOfWork uow)
        {
            uow.GetRepository<Migration.MigrateState>().Create(new Migration.MigrateState() { Name = "Создано", Code = "101" });
            uow.GetRepository<Migration.MigrateState>().Create(new Migration.MigrateState() { Name = "Обновлено", Code = "102" });
            uow.GetRepository<Migration.MigrateState>().Create(new Migration.MigrateState() { Name = "Ошибка", Code = "103" });
            uow.GetRepository<Migration.MigrateState>().Create(new Migration.MigrateState() { Name = "Не обновлено", Code = "104" });

            //Добавление разрешений на Выписки для Ролей
            List<string> listCodeRoleReadeRosReestr = new List<string>() { "EstateRead", "EstateWrite", "NonCoreAssetRead", "NonCoreAssetWrite", "ScheduleStateRead", "ScheduleStateWrite" };
            List<string> listCodeRoleWriteRosReestr = new List<string>() { "ImportRosreestr" };

            SetPermissionFromRole(uow, listCodeRoleReadeRosReestr, GetExtractReadePermissions(uow));
            SetPermissionFromRole(uow, listCodeRoleWriteRosReestr, GetExtractReadeAndWritePermissions(uow));
        }

        private void SetPermissionFromRole(IUnitOfWork uow, List<string> listCodeRole, List<Permission> perms)
        {

            var RoleRepo = uow.GetRepository<Base.Security.Role>();
            foreach (string code in listCodeRole)
            {
                Role tRole = RoleRepo
                            .Filter(f => !f.Hidden && f.Code == code)
                            .FirstOrDefault();
                if (tRole != null)
                {
                    // List<Permission> newPerms = new List<Permission>() { };
                    //newPerms.AddRange(tRole.Permissions);
                    //newPerms.AddRange(perms);
                    foreach (Permission itm in perms)
                    {
                        if (!tRole.Permissions.Contains(itm))
                            tRole.Permissions.Add(itm);
                            
                    }
                    RoleRepo.Update(tRole);
                }
            }
            uow.SaveChanges();
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Выписок
        /// </summary>
        /// <returns>Permission</returns>
        private static List<Permission> GetExtractReadePermissions(IUnitOfWork uow)
        {
            var perms =
                   new List<Permission>(CorpProp.UsersAndRolesInitializer.GetExtractReadePermissions())
                   {
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractObject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractSubj), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.AnotherSubject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.BaseParameter), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.BuildRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.CadNumber), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.CarParkingSpaceLocationInBuildPlans), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ContourOKSOut), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.DealRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.DocumentRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractBuild), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractLand), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractNZS), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractObject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractSubj), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Governance), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.IndividualSubject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.LandRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.LegalSubject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.NameRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Notice), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.NoticeSubj), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ObjectRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.OldNumber), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Organization), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.PermittedUse), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.PublicSubject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Refusal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RefusalSubj), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RestrictedRightsPartyOut), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RestrictRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightHolder), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightRecordNumber), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RoomLocationInBuildPlans), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.SubjectRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.SubjRight), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.TPerson), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateHistory), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateLog), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateState), TypePermission.Read | TypePermission.Navigate),
                   };

            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetBasePermissions());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndOpenPerms());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetCadastralReadePermissions());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetPropertyComplexReadePermissions());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetRightReadePermissions());

            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Выписок
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetExtractReadeAndWritePermissions(IUnitOfWork uow)
        {
            var perms =
                   new List<Permission>(CorpProp.UsersAndRolesInitializer.GetExtractReadeAndWritePermissions())
                   {
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractSubj), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.AnotherSubject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.BaseParameter), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.BuildRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.CadNumber), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.CarParkingSpaceLocationInBuildPlans), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ContourOKSOut), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.DealRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.DocumentRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractBuild), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractLand), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractNZS), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractSubj), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Governance), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.IndividualSubject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.LandRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.LegalSubject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.NameRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Notice), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.NoticeSubj), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ObjectRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.OldNumber), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Organization), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.PermittedUse), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.PublicSubject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Refusal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RefusalSubj), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RestrictedRightsPartyOut), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RestrictRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightHolder), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightRecordNumber), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RoomLocationInBuildPlans), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.SubjectRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.SubjRight), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.TPerson), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateHistory), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateLog), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateState), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                   };

            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetBasePermissions());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndOpenPerms());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetCadastralReadeAndWritePermissions());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetPropertyComplexReadeAndWritePermissions());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetRightReadeAndWritePermissions());

            return perms;
        }
    }
}
