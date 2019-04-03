using Base.Attributes;
using Base.Translations;

namespace Base.PBX.Entities
{
    public class PBXServer : BaseObject, IPBXServer
    {
        private static readonly CompiledExpression<PBXServer, string> _url =
            DefaultTranslationOf<PBXServer>.Property(x => x.Url).Is(x => "https://" + x.ServerAddress + ":8089");

        public string Url => _url.Evaluate(this);

        [DetailView(Name = "Наименование"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Title { get; set; }

        [DetailView(Name = "IP Адрес"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string ServerAddress { get; set; }

        [DetailView(Name = "Пользователь")]
        [PropertyDataType(PropertyDataType.Text)]
        public string User { get; set; }

        [DetailView(Name = "Пароль")]
        [PropertyDataType(PropertyDataType.Password)]
        public string Password { get; set; }

        [DetailView(Name = "Минимально допустимый номер телефона", Description = "Если меньше нуля, то будет задан первый свободный номер")]
        public int MinNumber { get; set; }
    }
}