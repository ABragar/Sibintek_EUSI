using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Helpers
{

    /// <summary>
    /// Перечень вкладок для карточек выписок.
    /// </summary>    
    public static class ExtractTabs
    {
        public const string Info1 = "[010]Реквизиты выписки";
        public const string GeneralData1 = "[010]Общие данные";
        public const string Rights2 = "[020]Права";

        public const string Estate2 = "[020]Характеристики недвижимости";
        public const string Estate3 = "[030]Характеристики недвижимости";
        public const string Objetcs3 = "[030]ОНИ";
        public const string Land2 = "[020]Земельный участок";
        public const string Location3 = "[030]Адрес (местоположение)";
        public const string Rights4 = "[040]Права";
        public const string DocRights4 = "[040]Документы основания регистрации";
        public const string Ownerless5 = "[050]Сведения о бесхозяйном имуществе)";
        public const string Encumbrances5 = "[050]Обременения/ограничения";
        public const string SrvInfo5 = "[050]Сервисная информация";
        public const string Encumbrances6 = "[050]Обременения/ограничения";
        public const string Deals7 = "[070]Сделки, без согласия третьего лица, органа";
        public const string FileCards8 = "[080]Документы";

        public const string Logs9 = "[090]Журнал миграции";

        public static string QuickView = "[001]Быстрый просмотр";
        public static string LinkedObjects = "[002]Связанные объекты";
    }

}
