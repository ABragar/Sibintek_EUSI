using Base.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    public class ExtractLand : ExtractObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractBuild.
        /// </summary>
        public ExtractLand() : base()
        {

        }


        #region Сведения о ЗУ LandRecordBaseParams 

        /// <summary>
        /// Вид земельного участка
        /// </summary>     
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Код вида ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubtypeCode { get; set; }

        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubtypeName { get; set; }

        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Subtype { get; set; }

        public int? Date_removed_cad_accountID { get; set; }
        /// <summary>
        /// Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд
        /// </summary>      
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Сведения об изъятии", TabName = TabName22, Visible = false
            //, Description = "Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд")]

        public DocumentRecord Date_removed_cad_account { get; set; }

        /// <summary>
        /// Дата постановки по документу
        /// </summary>       
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Дата постановки по документу", TabName = TabName22, Visible = false)]
        public System.DateTime? Reg_date_by_doc { get; set; }



        /// <summary>
        /// Вид категории
        /// </summary>       
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Код категории ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CategoryCode { get; set; }
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CategoryName { get; set; }

        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Category { get; set; }

        /// <summary>
        /// вид разрешенного использования По документу
        /// </summary>       
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид разрешенного использования по документу", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedBy_document { get; set; }
        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с ранее использовавшимся классификатором
        /// </summary>       
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Код вида разрешенного использования (старый)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_useCode { get; set; }
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид разрешенного использования (старый)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_useName { get; set; }
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид разрешенного использования (старый)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_use { get; set; }
        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>       
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Код вида разрешенного использования (приказ 540 от 01.09.2014)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_use_merCode { get; set; }
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид разрешенного использования (приказ 540 от 01.09.2014)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_use_merName { get; set; }

        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид разрешенного использования (приказ 540 от 01.09.2014)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_use_mer { get; set; }

        /// <summary>
        /// Реестровый номер границы  градостроительному регламенту
        /// </summary>
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Реестровый номер границы", TabName = TabName22//, Description = "Вид разрешенного использования по градостроительному регламенту", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_Reg_numb_border { get; set; }
        /// <summary>
        /// Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Код вида использования по градостроительному регламенту", TabName = TabName22, Visible = false
            //, Description = "Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_Land_useCode { get; set; }
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид использования по градостроительному регламенту", TabName = TabName22, Visible = false
           //, Description = "Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_Land_useName { get; set; }
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид использования по градостроительному регламенту", TabName = TabName22, Visible = false
          //, Description = "Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_Land_use { get; set; }
        /// <summary>
        /// Разрешенное использование (текстовое описание)
        /// </summary>
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Разрешенное использование", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_use_text { get; set; }




        #region Местоположение относительно ориентира LocationByARefPoint
        /// <summary>
        /// Ориентир расположен в границах участка
        /// </summary>
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = " Ориентир расположен в границах участка", TabName = TabName3, Visible = false)]
        public bool In_boundaries_mark { get; set; } = false;
        /// <summary>
        /// Наименование ориентира
        /// </summary>
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Наименование ориентира", TabName = TabName3, Visible = false)]
        public string Ref_point_name { get; set; }
        /// <summary>
        /// Расположение относительно ориентира
        /// </summary>
        [ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Расположение относительно ориентира", TabName = TabName3, Visible = false)]
        public string Location_description { get; set; }
        #endregion

        #endregion

    }
}
