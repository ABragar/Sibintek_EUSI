﻿using Base.Attributes;
using Base.Entities.Complex;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Utils.Common.Attributes;

namespace Base.Localize.Entities
{
    [EnableFullTextSearch]
    public class LocalizeItem : BaseObject, ICategorizedItem
    {
        public const char Separator = '/';

        [MaxLength(255)]
        [DetailView("Ключ")]
        [ListView]
        public string Key { get; set; }

        //[DetailView("Значение")]
        //[FullTextSearchProperty]
        //[ListView]
        //public MultilanguageTextArea Value { get; set; }

        #region ICategorizedItem
        public int CategoryID { get; set; }

        [FullTextSearchProperty]
        [ForeignKey("CategoryID")]
        public virtual LocalizeItemCategory LocalizeItemCategory { get; set; }

        HCategory ICategorizedItem.Category
        {
            get { return this.LocalizeItemCategory; }
        }
        #endregion
    }
}
