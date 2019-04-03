using Base;
using Base.Attributes;
using Base.Entities.Complex;
using Base.Enums;
using Base.Security;
using Base.Security.Entities.Concrete;
using Base.UI.Wizard;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.WindowsAuth.Entities
{
    public class ADUserWizard : SibUserAccessWizard
    {
        [DetailView("Логин", Required = true, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public override string Login { get; set; }

        [DetailView("Пользователь из Active Directory", Required = true)]
        public ADUser ADUser { get; set; }

        public void CopyValuesToWizard(ADUser selectedUser)
        {
            if (selectedUser != null)
            {
                this.Login = selectedUser.Login;
                this.LastName = selectedUser.LastName;
                this.FirstName = selectedUser.FirstName;
                this.MiddleName = selectedUser.MiddleName;
                this.Department = selectedUser.Department;
                this.Post = selectedUser.JobTitle;
                this.Email = selectedUser.Email;
                this.Phone = selectedUser.Phone;
                this.Mobile = selectedUser.Mobile;
                this.Description = selectedUser.Description;
            }
        }
    }
}
