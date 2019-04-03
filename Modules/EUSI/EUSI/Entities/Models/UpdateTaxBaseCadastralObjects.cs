using System;
using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Common;

namespace EUSI.Entities.Models
{
    public class UpdateTaxBaseCadastralObjects : BaseObject, IDialogObject
    {
        [DetailView("Год", Required = true)]
        [FullTextSearchProperty]
        public DateTime Year { get; set; }
    }
}