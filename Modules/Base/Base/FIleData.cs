using Base.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using Base.Utils.Common.Attributes;

namespace Base
{
    [EnableFullTextSearch]   
    [Serializable]
    public sealed class FileData : BaseObject
    {
        public FileData()
        {
            CreationDate = DateTime.Now;
            ChangeDate = DateTime.Now;
        }

        [SystemProperty]
        [DetailView(Visible = false)]
        [UniqueIndex("Index_FileID")]
        public Guid FileID { get; set; }

        [MaxLength(500)]
        [DetailView(Name = "Имя файла", Order = 0)]
        [FullTextSearchProperty]
        [ListView]
        public string FileName { get; set; }

        //[DetailView(Name = "Размер", Order = 1, ReadOnly = true)]
        //[ListView]
        public long Size { get; set; }

        [ListView("   ")]
        [FullTextSearchProperty]
        // the maximum length for a path is MAX_PATH, which is defined as 260 characters.
        // https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247.aspx#maxpath
        [MaxLength(260-1)]
        [SystemProperty]
        public string Extension { get; set; }

        [DetailView(Name = "Дата загрузки", Order = 2, ReadOnly = true)]
        public DateTime CreationDate { get; set; }

        [DetailView(Name = "Дата последнего изменения", Order = 3, ReadOnly = true)]
        public DateTime ChangeDate { get; set; }

        ////TODO: убрать!!!
        //public Guid Key
        //{
        //    get { return FileID; }
        //}
    }

    public class FileCollectionItem : EasyCollectionEntry<FileData>
    {

    }
}
