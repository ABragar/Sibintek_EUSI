using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Base.Contact.Entities;
using Base.Contact.Service.Concrete;
using Base.DAL;
using Base.MailAdmin.Entities;
using Base.MailAdmin.Services;
using Base.PBX.Entities;
using Base.PBX.Services.Abstract;
using Base.Security;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.Settings;
using Base.UI.ViewModal;
using Base.Utils.Common;
using Common.Data.Entities;
using WebApi.Proxies.Models;

namespace Common.Data.Service.Concrete
{
    public class ConcreteAccessWizardService : AccessUserWizardService<СoncreteAccessWizard>
    {
        private readonly ISettingService<CompanySetting> _companySettingService;
        private readonly ISettingService<MailAdminSettings> _mailSettingService;
        private readonly EmployeeUserService _employeeUserService;
        private readonly ISIPAccountService _sipAccountService;
        private readonly IPBXUserService _pbxUserService;
        private readonly IBaseObjectService<PBXServer> _pbxServerService;
        private readonly IPasswordService _passwordService;
        private readonly IMailAdminClient _mail_admin_client;

        public ConcreteAccessWizardService(IAccessUserService baseService,
            IAccessService accessService,
            ILoginProvider loginProvider,
            ISettingService<CompanySetting> companySettingService,
            EmployeeUserService employeeUserService,
            ISIPAccountService sipAccountService,
            IPBXUserService pbxUserService,
            IBaseObjectService<PBXServer> pbxServerService,
            IPasswordService passwordService, IMailAdminClient mailAdminClient,
            ISettingService<MailAdminSettings> mailSettingService)
            : base(baseService, accessService, loginProvider)
        {
            _companySettingService = companySettingService;
            _employeeUserService = employeeUserService;
            _sipAccountService = sipAccountService;
            _pbxUserService = pbxUserService;
            _pbxServerService = pbxServerService;
            _passwordService = passwordService;
            _mail_admin_client = mailAdminClient;
            _mailSettingService = mailSettingService;
        }

        public override async Task<User> CompleteAsync(IUnitOfWork unitOfWork, СoncreteAccessWizard obj)
        {
            if (obj.CreateMailAccount)
            {
                obj.Email = $"{obj.MailAccount}@{_mailSettingService.Get().Domain}";
            }

            var user = await base.CompleteAsync(unitOfWork, obj);

            if (obj.CreateEmployee)
            {
                _employeeUserService.Create(unitOfWork, new EmployeeUser()
                {
                    Department = obj.Department,
                    Post = obj.Post,
                    UserID = user.ID,
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    MiddleName = obj.MiddleName,
                });
            }

            if (obj.CreateMailAccount)
            {
                try
                {
                    await
                        _mail_admin_client.CreateAccountAsync(new CreateAccountModel
                        {
                            Name = obj.Email,
                            Password = obj.Password
                        });
                }
                catch (Exception ex)
                {
                    throw new ValidationException("Не удалось создать почтовый ящик : " + ex.ToString());
                }
            }

            if (obj.CreateSipAccount)
            {
                try
                {
                    var server = _pbxServerService.Get(unitOfWork, obj.PBXServer.ID);
                    var number = int.Parse(obj.extension);

                    if (number < server.MinNumber)
                        throw new ValidationException(
                            "Указанный добавочный номер не является допустимым для данного сервера.");

                    if (await _pbxUserService.IsNumberExistAsync(server, number))
                        throw new ValidationException("Данный номер уже зарегестрирован в системе.");
                }
                catch (FormatException)
                {
                    throw new ValidationException("Неправильный формат добавочного номера!");
                }

                _sipAccountService.Create(unitOfWork, new SIPAccount()
                {
                    PBXServer = obj.PBXServer,
                    User = user,
                    out_of_service = false,
                    hasvoicemail = obj.hasvoicemail,
                    email_to_user = obj.email_to_user,
                    auto_record = obj.auto_record,
                    cidnumber = obj.cidnumber,
                    enable_webrtc = obj.enable_webrtc,
                    secret = obj.secret,
                    skip_vmsecret = false,
                    user_password = obj.user_password,
                    vmsecret = obj.vmsecret,
                    extension = obj.extension
                });
            }

            return user;
        }

        public override async Task OnAfterStepChangeAsync(IUnitOfWork unitOfWork, ViewModelConfig config,
            string prevStepName, СoncreteAccessWizard obj)
        {
            switch (obj.Step)
            {
                case "create_mail_account":
                    obj.MailAccount =
                        Transliteration.Front($"{obj.LastName}.{obj.FirstName?.First()}{obj.MiddleName?.First()}");
                    obj.MailDomain = _mailSettingService.Get().Domain;
                    break;
                case "create_employee":
                    obj.Company = _companySettingService.Get().Company;
                    break;
                case "sipserver":
                    var servers = _pbxServerService.GetAll(unitOfWork);

                    if (servers.Any())
                    {
                        obj.PBXServer = servers.FirstOrDefault();

                        if (servers.Count() == 1)
                            await NextStepAsync(unitOfWork, obj, config);
                    }
                    break;
                case "sipaccount":
                    if (obj.PBXServer != null)
                    {
                        var server = _pbxServerService.Get(unitOfWork, obj.PBXServer.ID);
                        obj.extension = await _pbxUserService.GetAvailableNumberAsync(server);

                        obj.user_password = _passwordService.Generate(12, PasswordCharacters.AlphaNumeric);
                        obj.secret = _passwordService.Generate(12, PasswordCharacters.AlphaNumeric);
                        obj.vmsecret = _passwordService.Generate(6, PasswordCharacters.Numbers);
                    }
                    break;
            }

            await base.OnAfterStepChangeAsync(unitOfWork, config, prevStepName, obj);
        }
    }
}