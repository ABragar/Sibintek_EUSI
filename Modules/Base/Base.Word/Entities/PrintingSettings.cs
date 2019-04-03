using Base.Attributes;
using System.ComponentModel.DataAnnotations;
namespace Base.Word.Entities
{
    public class PrintingSettings : BaseObject
    {
        [MaxLength(255)]
        [DetailView(Required = true)]
        [ListView]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string Mnemonic { get; set; }

        [MaxLength(255)]
        [DetailView(Required = true)]
        [ListView]
        public string TemplateName { get; set; }

        [DetailView(Required = true)]
        [PropertyDataType(PropertyDataType.File, Params = "Select=true;Ext=docx")]
        public virtual FileData Template { get; set; }

        [DetailView]
        public TemplateType TemplateType { get; set; }
    }

    [UiEnum]
    public enum TemplateType
    {

        [UiEnumValue("Шаблон печати")]
        Print = 0,
        [UiEnumValue("Шаблон письма")]
        Message = 1,
    }
}