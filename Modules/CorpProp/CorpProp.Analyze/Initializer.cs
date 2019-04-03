using Base;
using Base.DAL;
using Base.UI.Presets;
using Base.UI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Reporting.Entities;
using Base.UI;
using CorpProp.Analyze.Entities.Accounting;
using CorpProp.Analyze.Entities.NSI;
using CorpProp.Analyze.Entities.Subject;
using CorpProp.Analyze.Services.Accounting;
using CorpProp.Analyze.Services.Subject;
using CorpProp.DefaultData;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;
using CorpProp.Services.Base;
using CorpProp.Analyze.Model;
using Base.Security;
using Base.Enums;

namespace CorpProp.Analyze
{
    public class Initializer : IModuleInitializer
    {
        private readonly IPresetRegistorService _presetRegistorService;

        public Initializer(IPresetRegistorService presetRegistorService)
        {
            _presetRegistorService = presetRegistorService;
        }


        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="context"></param>
        public void Init(IInitializerContext context)
        {
            //инициализация модели - создание кофигураторов ListView и DetailView
            AnalyzeModel.Init(context);

            context.DataInitializer("CorpProp.Analyze", "0.1", () =>
            {
                DefaultDataHelper.InstanceSingletone.InitDefaulData(context.UnitOfWork, new CorpProp.Analyze.DefaultData.FillDataStrategy());
                Seed(context.UnitOfWork);
            });
        }

        /// <summary>
        /// Наполнение БД.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        private void Seed(IUnitOfWork uow)
        {

            //Добавление разрешений на Расширенную отчетность для Ролей
            List<string> listCodeRoleReadeAnalyze = new List<string>() { "UnknownEUSI", "Admin", "ImportBusinessIntelligenceData" };
            List<string> listCodeRoleWriteAnalyze = new List<string>() { "UnknownEUSI", "Admin", "ImportBusinessIntelligenceData" };

            SetPermissionFromRole(uow, listCodeRoleReadeAnalyze, GetAnalyzeReadePermissions(uow));
            SetPermissionFromRole(uow, listCodeRoleWriteAnalyze, GetAnalyzeReadeAndWritePermissions(uow));
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
        /// Предоставляет набор разрешений для чтения Расширенной отчетности
        /// </summary>
        /// <returns>Permission</returns>
        private static List<Permission> GetAnalyzeReadePermissions(IUnitOfWork uow)
        {
            var perms =
                   new List<Permission>(CorpProp.UsersAndRolesInitializer.GetAnalyzeReadePermissions())
                   {
                    new Permission(typeof(BankAccount), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FinancialIndicatorItem), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RecordBudgetLine), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(BudgetLine), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FinancialIndicator), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(AnalyzeSociety), TypePermission.Read | TypePermission.Navigate),
                   };

            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetBasePermissions());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndOpenPerms());

            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Расширенной отчетности
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAnalyzeReadeAndWritePermissions(IUnitOfWork uow)
        {
            var perms =
                   new List<Permission>(CorpProp.UsersAndRolesInitializer.GetAnalyzeReadePermissions())
                   {
                    new Permission(typeof(BankAccount), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FinancialIndicatorItem), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(RecordBudgetLine), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(BudgetLine), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FinancialIndicator), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(AnalyzeSociety), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                   };

            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetBasePermissions());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndOpenPerms());

            return perms;
        }
    }
}