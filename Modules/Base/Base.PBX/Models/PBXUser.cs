using Base.Attributes;

namespace Base.PBX.Models
{
    public class PBXUser : BaseObject, IPBXUser, IPBXAccount
    {
        public new int ID => user_id;

        #region IPBXUser

        [SystemProperty]
        public int user_id { get; set; }

        [DetailView("Имя", TabName = "[0]Основная информация", Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string first_name { get; set; }

        [DetailView("Фамилия", TabName = "[0]Основная информация", Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string last_name { get; set; }

        [DetailView("Пароль", TabName = "[0]Основная информация", Required = true)]
        [PropertyDataType(PropertyDataType.Password)]
        public string user_password { get; set; }

        [DetailView("Отправлять e-mail", TabName = "[0]Основная информация")]
        public bool email_to_user { get; set; }

        [DetailView("E-mail", TabName = "[0]Основная информация")]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        public string email { get; set; }

        [DetailView("Телефон", TabName = "[0]Основная информация")]
        [PropertyDataType(PropertyDataType.PhoneNumber)]
        public string phone_number { get; set; }

        #endregion

        #region IPBXAccount

        [DetailView("Добавочный номер", TabName = "[1]Настройки", ReadOnly = true), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string extension { get; set; }

        [DetailView("Номер CallerID", TabName = "[1]Настройки")]
        [PropertyDataType(PropertyDataType.Text)]
        public string cidnumber { get; set; }

        
        //SIP(WebRTC) if WebRTC is True, this is just readonly systemdescription
        [DetailView("Тип аккаунта", TabName = "[1]Настройки", ReadOnly = true), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string account_type { get; set; }


        [DetailView("Ф.И.О.", TabName = "[1]Настройки", ReadOnly = true, Visible = false), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string fullname { get; set; }

        [DetailView("Пароль SIP/IAX", TabName = "[1]Настройки", Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string secret { get; set; }

        [DetailView("Пароль голосовой почты", TabName = "[1]Настройки", Required = true)]
        [PropertyDataType(PropertyDataType.Number)]
        public string vmsecret { get; set; } //just numbers


        [DetailView("Пропустить проверку пароля голосовой почты", TabName = "[1]Настройки")]
        public bool skip_vmsecret { get; set; }




        [DetailView("Использовать WebRTC", TabName = "[1]Настройки")]
        public bool enable_webrtc { get; set; }


        [DetailView("Голосовая почта", TabName = "[1]Настройки")]
        public bool hasvoicemail { get; set; }


        public bool auto_record { get; set; }

        [DetailView("Отключен", TabName = "[1]Настройки")]
        public bool out_of_service { get; set; }
        #endregion

    }
}