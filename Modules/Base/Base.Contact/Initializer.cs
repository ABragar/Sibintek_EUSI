using System;
using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Entities.Complex;
using Base.Links.Entities;
using Base.Security;
using Base.UI;
using Base.UI.ViewModal;
using System.Collections.Generic;
using System.Linq;
using Base.Attributes;
using Base.Contact.Service.Concrete;
using Base.Security.Entities.Concrete;
using Base.Settings;


namespace Base.Contact
{
    public class Initializer : IModuleInitializer
    {
        private readonly ILinkBuilder _linkBuilder;
        private readonly ISettingService<CompanySetting> _settingCompanyService;

        public Initializer(ILinkBuilder linkBuilder, ISettingService<CompanySetting> settingCompanyService)
        {
            _linkBuilder = linkBuilder;
            _settingCompanyService = settingCompanyService;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<BaseContact>()
                .Service<IBaseContactService<BaseContact>>()
                .Icon("glyphicon glyphicon-group")
                .Title("Контрагенты")
                .Name("Contact")
                .Preview(p => p.Select(ProjectBaseContact))
                .LookupProperty(x => x
                    .Text(e => e.Title)
                    .Image(e => e.Image));

            context.CreateVmConfigOnBase<BaseContact, Company>()
                .LookupProperty(x => x.Text(t => t.Name).NoImage())
                .Service<IBaseContactService<Company>>()
                .Icon(new Icon("glyphicon glyphicon-king") { Color = "#5cb85c" })
                .Title("Баз. Компании")
                .ListView(x => x.Title("Компании").Columns(columns =>
                {
                    columns.Add(p => p.ExtraID, p => p.Visible(false));
                }))
                .DetailView(x => x.Title("Компания").Editors(editors =>
                {
                    editors.Add(p => p.Image, e => e.Title("Логотип"));
                    editors.Add(p => p.Title, e => e.Title("Наименование"));
                }).Toolbar(t =>
                {
                    t.Add("GetCompanyToolbar", "Contact", p => p.ListParams(l => l.Add("companyID", "[ID]")));
                }));

            context.CreateVmConfigOnBase<Company, SimpleCompany>()
                .Title("Компании");


            context.CreateVmConfig<Department>()
                .Title("Отделы")
                .DetailView(d => d.Title("Отдел"))
                .ListView(x => x.Title("Отделы"));


            context.CreateVmConfig<BaseEmployee>()
                .Service<IBaseEmployeeService<BaseEmployee>>()
                .Icon("glyphicon glyphicon-group")
                .Title("Базовый контакт")
                .ListView(x => x.Title("Контакты").Columns(columns =>
                {
                    columns.Add(p => p.ExtraID, p => p.Visible(false));
                    columns.Add(p => p.Image, p => p.Title("Фотография"));
                    columns.Add(p => p.Title, p => p.Title("Ф.И.О."));
                }))
                .DetailView(x => x.Title("Базовый контакт").Editors(editors =>
                {
                    editors.Add(p => p.Image, e => e.Title("Фотография"));
                    editors.Add(p => p.Company, e => e.IsReadOnly(false));
                    editors.Add(p => p.Department, e => e.CascadeFrom(c => c.Company));
                    editors.Add(p => p.Responsible, e => e.Visible(false).IsRequired(false));
                }));

            context.CreateVmConfigOnBase<BaseEmployee, Employee>()
               .Service<IEmployeeService>()
               .ListView(x => x.Title("Контакты"))
               .Icon("glyphicon glyphicon-pawn")
               .DetailView(x => x.Title("Контакт").Editors(editors =>
               {
                   editors.Add(p => p.Title, e => e.Visible(false).IsRequired(false));
               }));

            context.CreateVmConfigOnBase<BaseEmployee, EmployeeUser>()
              .Service<IEmployeeUserService>()
              .Icon("glyphicon glyphicon-knight")
              .Title("Сотрудник")
              .DetailView(x => x.Title("Сотрудник").Wizard("EmployeeUserWizard").Editors(editors =>
              {
                  editors.Add(p => p.Image, e => e.Title("Фотография").IsReadOnly().Order(0));
                  editors.Add(p => p.Title, e => e.Title("Ф.И.О.").IsReadOnly().Order(10));
                  editors.Add(p => p.Gender, e => e.IsReadOnly().Order(40));
                  editors.Add(p => p.BirthDate, e => e.IsReadOnly().Order(50));
                  editors.Add(p => p.Source, e => e.Order(60));

                  editors.Add(p => p.FirstName, e => e.Visible(false).IsRequired(false));
                  editors.Add(p => p.LastName, e => e.Visible(false).IsRequired(false));
                  editors.Add(p => p.MiddleName, e => e.Visible(false).IsRequired(false));

                  editors.Add<ProfileInfo>("ProfileInfo", e => e.DataType(PropertyDataType.Custom).Title(" ").Order(70));


                  editors.Add(p => p.Address, e => e.IsReadOnly());
                  editors.Add(p => p.Phones, e => e.IsReadOnly());
                  editors.Add(p => p.Emails, e => e.IsReadOnly());
              }).DefaultSettings((uow, o, commonEditorViewModel) =>
              {
                  commonEditorViewModel.Visible("ProfileInfo", Ambient.AppContext.SecurityUser.IsAdmin);
              }));


            context.ModifyVmConfig<User>().Preview(a => a.Select((uofw, user) =>
            {
                var imageId = Guid.Empty;
                string fullName = "";
                string job = "";

                var employee = uofw.GetRepository<EmployeeUser>().All()
                    .Where(x => x.UserID == user.ID).Select(x => new
                {
                    FullName = x.Title,
                    ImageId = x.Image != null ? x.Image.FileID  : Guid.Empty,
                    Job = x.Company != null ? x.Company.Title : null
                }).SingleOrDefault();

                if (employee != null)
                {
                    imageId = employee.ImageId;
                    fullName = employee.FullName;
                    job = employee.Job;
                }
                else
                {
                    if (user.BaseProfileID != null)
                    {
                        var profile =
                            uofw.GetRepository<BaseProfile>().All()
                                .Where(x => x.ID == user.BaseProfileID.Value)
                                .Select(x => new { x.FullName, ImageId = x.Image != null ? x.Image.FileID : Guid.Empty })
                                .Single();

                        imageId = profile.ImageId;
                        fullName = profile.FullName;
                    }
                }

                return new
                {
                    user.ID,
                    FullName = fullName,
                    Image = new FileData()
                    {
                        FileID = imageId
                    },
                    Job = job
                };
            }));

            context.CreateVmConfig<EmployeeUserWizard>()
                .Title("Мастер создания контакта")
                .Service<IEmployeeWizardService>()
                .WizzardDetailView(w => w
                    .Steps(steps => steps
                        .Add("selectuser", s => s
                            .Title("Выбор пользователя")
                            .StepProperties(props => props.Add(p => p.User)))
                        .Add("selectDepartment", s =>
                        {
                            s.Title("Выбор отдела");
                            s.StepProperties(props => props.Add(p => p.Category));
                            s.Rule(f => f.CategoryID == 0 ? StepRuleResult.Ok : StepRuleResult.Forward);
                        })
                        .Add("main", s => s
                            .Title("Основная информация")
                            .StepProperties(props => props
                                .Add(p => p.Source)
                                ))
                        .Add("job", s => s
                            .Title("Место работы")
                            .StepProperties(props => props
                                    .Add(p => p.Post)))
                        .Add("description", s => s
                            .Title("Примечание")
                            .StepProperties(props => props
                                    .Add(p => p.Description)))
            ));

            context.CreateVmConfig<FamilyMember>()
                .Title("Член семьи")
                .Service<IFamilyMemberService>()
                .ListView(x => x.Title("Член семьи"))
                .DetailView(x => x.Title("Член семьи"));


            context.CreateVmConfig<BasePersonAgent>()
                .Title("Представитель")
                .Service<IBasePersonAgentService>()
                .ListView(x => x.Title("Представитель"))
                .DetailView(x => x.Title("Представитель"));


            context.CreateVmConfig<ContactInterest>()
                .Title("Интересы контакта")
                .Service<IBaseContactInterestService<ContactInterest>>()
                .ListView(l => l.Title("Интересы контакта"))
                .DetailView(d => d.Title("Интерес контакта"));

            context.CreateVmConfig<BaseContactCategory>()
                .Title("Категория контакта");

            context.CreateVmConfig<CompanyRole>()
                .Title("Роль компании")
                .ListView(l => l.Title("Роли компании"))
                .DetailView(d => d.Title("Роль компании"));

            context.CreateVmConfig<CompanyAddress>()
                .Title("Адрес Базовый")
                .DetailView(d => d.Title("Адрес"));

            context.CreateVmConfig<EmployeePost>()
             .Title("Должность")
             .DetailView(d => d.Title("Должность"));

            context.CreateVmConfig<OkvedType>()
                .LookupProperty(l => l.Text(t => t.Title))
                .Title("ОКВЭД Базовый")
                .DetailView(x => x.Title("ОКВЭД"))
                .ListView(x => x.Title("ОКВЭД"));

            context.CreateVmConfig<Bank>()
                .Title("Банк Базовый")
                .DetailView(x => x.Title("Банк"))
                .ListView(x => x.Title("Банк"));

            context.CreateVmConfig<PaymentDetail>()
                .Title("Платежные реквизиты")
                .ListView(x => x.Title("Платежные реквизиты"));

            context.CreateVmConfig<CompanySetting>()
                .Service<ISettingService<CompanySetting>>()
                .Title("Настройки Компании")
                .DetailView(x => x.Title("Настройки Компании"))
                .ListView(x => x.Title("Настройки Компании").HiddenActions(new[] { LvAction.Create, LvAction.Delete }));


            context.CreateVmConfig<CompanyType>()
                .Title("Тип компании")
                .ListView(x => x.Title("Тип компании"));


            InitLinks();

            context.DataInitializer("Contact", "0.1", () =>
            {
                InitContact(context.UnitOfWork);
            });
        }

        private void InitLinks()
        {
            _linkBuilder.Register<Employee, Company>().Config((source, dest) =>
            {

            });

            _linkBuilder.Register<Company, Department>().Config((company, department) =>
            {
                department.Company = company;
            });

            _linkBuilder.Register<Company, Company>();
        }

        private void InitContact(IUnitOfWork unitOfWork)
        {
            if (!_settingCompanyService.Any(unitOfWork))
            {
                _settingCompanyService.Create(unitOfWork, new CompanySetting()
                {
                    Title = "Настройки Компании",
                    Company = new SimpleCompany()
                    {
                        Title = "Наша Компания"
                    }
                });
            }
        }

        private object ProjectBaseContact(IUnitOfWork uofw, BaseContact baseContact)
        {
            var obj = new
            {
                baseContact.ID,
                baseContact.Image,
                baseContact.Title,
                Props = new Dictionary<string, string>()
            };

            //switch (baseContact.BoType.Mnemonic)
            //{
            //    case nameof(Company):
            //        var company = uofw.GetRepository<Company>().Find(x => x.ID == baseContact.ID);
            //        if (company != null)
            //        {
            //            obj.Props.Add("Address", company.Addresses?.FirstOrDefault()?.Address.Title);

            //            if (company.MainContact != null)
            //            {
            //                obj.Props.Add("MainContact_ID", company.MainContact.ID.ToString());
            //                obj.Props.Add("MainContact_ImageID", company.MainContact.Image?.FileID.ToString());
            //                obj.Props.Add("MainContact_Mnemonic", company.MainContact.BoType.Mnemonic);
            //                obj.Props.Add("MainContact_Title", company.MainContact.Title);
            //            }

            //            if (company.Responsible != null)
            //            {
            //                obj.Props.Add("Responsible_ID", company.Responsible.ID.ToString());
            //                obj.Props.Add("Responsible_ImageID", company.Responsible.Image?.FileID.ToString());
            //                obj.Props.Add("Responsible_Title", company.Responsible.FullName);
            //            }
            //        }
            //        break;
            //    case nameof(Employee):
            //        var employee = uofw.GetRepository<Employee>().Find(x => x.ID == baseContact.ID);
            //        if (employee != null)
            //        {
            //            obj.Props.Add("Job", employee.Department?.Company?.Title);
            //            obj.Props.Add("Email", employee.Emails?.FirstOrDefault()?.Email);
            //            obj.Props.Add("Phone", employee.Phones?.FirstOrDefault()?.Phone.ToString());
            //        }
            //        break;
            //    case nameof(EmployeeUser):
            //        var employeeUser = uofw.GetRepository<EmployeeUser>().Find(x => x.ID == baseContact.ID);
            //        if (employeeUser != null)
            //        {
            //            obj.Props.Add("Job", employeeUser.Department?.Company?.Title);
            //            obj.Props.Add("Email", employeeUser.Emails?.FirstOrDefault()?.Email);
            //            obj.Props.Add("Phone", employeeUser.Phones?.FirstOrDefault()?.Phone.ToString());
            //        }
            //        break;
            //}

            return obj;
        }
    }
}
