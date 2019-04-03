using System.Collections.Generic;
using Base.Attributes;


namespace Base.Document.Entities
{    
    public class UnifiedDocument : BaseDocument
    {
        //private static readonly CompiledExpression<UnifiedDocument, UnifiedDocumentChangeHistory> _lastChange =
        //    DefaultTranslationOf<UnifiedDocument>.Property(x => x.LastChange)
        //        .Is(x => x.FileHistory != null ? x.FileHistory.LastOrDefault() : null);

        public int? FileID { get; set; }
        [DetailView("Файл", Order = 3, Required = true), ListView(Width = 100)]
        [PropertyDataType(PropertyDataType.File)]
        public virtual FileData File { get; set; }

        [ListView]
        [DetailView(Name = "Статус", Visible = false, Order = 5)]
        public UnifiedDocumentStatus UnifiedDocumentStatus { get; set; }

        //[ListView]
        //public virtual UnifiedDocumentChangeHistory LastChange => _lastChange.Evaluate(this);

        [DetailView(TabName = "[1]История изменений файла", ReadOnly = true)]
        public virtual ICollection<UnifiedDocumentChangeHistory> FileHistory { get; set; }=new List<UnifiedDocumentChangeHistory>();
    }

    [UiEnum]
    public enum UnifiedDocumentStatus
    {
        [UiEnumValue("Черновик")]
        Draft = 0,
        [UiEnumValue("В работе")]
        OnWork = 1,
        [UiEnumValue("На доработке")]
        OnReWork=2,
        [UiEnumValue("Доработан")]
        ReWorked = 3,
        [UiEnumValue("Завершен")]
        Finished=4
    }
}
