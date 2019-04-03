using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Utils.Common.Attributes;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Entities.NomenclatureType
{
    [EnableFullTextSearch]
    public class Nomenclature : BaseObject, ISuperObject<Nomenclature>
    {
        //[DetailView("Код", ReadOnly = true, Order = -9), ListView]
        //[SystemProperty]
        //[FullTextSearchProperty]
        //public string CodeNomenclature { get; set; }

        [DetailView("Номенклатурный номер", Order = -9, Required = true), ListView]
        [FullTextSearchProperty]
        [MaxLength(255)]
        [UniqueIndex("NumberIndex")]
        public string Number { get; set; }

        public int? ImageID { get; set; }

        [DetailView("Изображение", Order = -10)]
        [Image()]
        [ListView(Visible = false)]
        public virtual FileData Image { get; set; }

        [MaxLength(255)]
        [FullTextSearchProperty]
        [DetailView("Наименование", Required = true, Order = -5), ListView]
        public string Title { get; set; }

        public int MeasureID { get; set; }

        [DetailView("Ед. изм.", Required = true, Order = -4), ListView]
        public virtual Measure Measure { get; set; }

        [FullTextSearchProperty]
        [DetailView(TabName = "[7]Описание")]
        [PropertyDataType(PropertyDataType.SimpleHtml)]
        public string Description { get; set; }

        [DetailView(TabName = "[8]Файлы")]
        [PropertyDataType(PropertyDataType.Files)]
        public virtual ICollection<NomenclatureFile> Files { get; set; }

        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; }

        //public string Path { get; set; }
    }

    public class NomenclatureFile : FileCollectionItem
    {
    }
}