using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет инвентарный объект в качестве ИК.
    /// </summary>
    public class PropertyComplexIO : InventoryObject
    {
        /// <summary>
        /// Инициализирует инвентарный объект в качестве ИК.
        /// </summary>
        public PropertyComplexIO() : base()
        {
            IsPropertyComplex = true;
        }

        /// <summary>
        /// Получает или задает кадастровый номер ЗУ.
        /// </summary>
        /// <remarks>
        /// Техническое поле.
        /// </remarks>
        [ListView(Visible = false)]
        [DetailView("Кадастровый номер ЗУ", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String LandCadNumber { get; set; }

        /// <summary>
        /// Получает или задает ИД ЗУ.
        /// </summary>
        [SystemProperty]
        public int? LandID { get; set; }

        /// <summary>
        /// Получает или задает ЗУ.
        /// </summary>
        [DetailView(Name = "Земельный участок", Visible = false)]
        [ListView("Земельный участок", Visible = false)]
        public Land Land { get; set; }


        /// <summary>
        /// Город
        /// </summary>
        [DetailView(Name = "Город")]
        [ListView]
        public string City { get; set; }

        public int? CountryID { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        [DetailView(Name = "Страна")]
        [ListView]
        public SibCountry Country { get; set; }

        [SystemProperty]
        public int? SibUserID { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        [ListView(Visible = false)]
        [FullTextSearchProperty]
        [DetailView(Name = "Пользователь", Visible = true, ReadOnly = true)]
        public SibUser SibUser { get; set; }
               

        /// <summary>
        /// Количество объектов
        /// </summary>
        [ListView]
        [DetailView(Name = "Тип ИК", ReadOnly = false)]
        public PropertyComplexIOType PropertyComplexIOType { get; set; }
        public int? PropertyComplexIOTypeID { get; set; }

        /// <summary>
        /// Получает или задает Наименование ИК (ТИС СП/НПО).
        /// </summary>
        [ListView("Наименование ИК (ТИС СП/НПО)", Visible = false)]
        [DetailView("Наименование ИК (ТИС СП/НПО)", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public String NameTIS { get; set; }
    }
}
