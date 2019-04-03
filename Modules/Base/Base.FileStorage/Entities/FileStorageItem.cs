using Base.Attributes;
using Base.Security.ObjectAccess;
using Base.Security.ObjectAccess.Policies;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.FileStorage
{

    [AccessPolicy(typeof(CreatorOnlyPolicy))]
    [EnableFullTextSearch]
    public class FileStorageItem : BaseObject, ICategorizedItem, IAccessibleObject
    {
        private static readonly CompiledExpression<FileStorageItem, string> extension =
            DefaultTranslationOf<FileStorageItem>.Property(x => x.Extension)
                .Is(x => x.File != null ? x.File.Extension : "");

        private static readonly CompiledExpression<FileStorageItem, long> size =
            DefaultTranslationOf<FileStorageItem>.Property(x => x.Size).Is(x => x.File != null ? x.File.Size / 1024 : 0);

        public int CategoryID { get; set; }

        [JsonIgnore]
        [ForeignKey("CategoryID")]
        public virtual FileStorageCategory Category_ { get; set; }

        public int? FileID { get; set; }

        [ListView(Order = 0)]
        [DetailView(Name = "Файл")]
        [PropertyDataType(PropertyDataType.File, Params = "Select=false")]
        [SystemProperty]
        public virtual FileData File { get; set; }

        [ListView(Order = 1)]
        [DetailView(Name = "Наименование", Description = "По умолчанию заполняется наименованием файла")]
        [MaxLength(255)]
        [FullTextSearchProperty]
        public string Title { get; set; }

        [ListView("Расширение", Width = 120, Order = 2)]
        public string Extension => extension.Evaluate(this);

        [ListView("Размер (кб)", Width = 120, Order = 3)]
        public long Size => size.Evaluate(this);

        [ListView(Hidden = true)]
        [DetailView(Name = "Описание")]
        [FullTextSearchProperty]
        public string Description { get; set; }


        [DetailView(Name = "Тэги поиска", TabName = "[1]Дополнительно")]
        [FullTextSearchProperty]
        public string Tags { get; set; }

        #region ICategorizedItem
        HCategory ICategorizedItem.Category => Category_;

        #endregion
    }
}
