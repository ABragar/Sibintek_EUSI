using System.Collections.Generic;
using Base.Multimedia.Models;

namespace Base.Multimedia.Entities
{
    public class MultimediaObject : BaseObject
    {
        public virtual List<SourceFile> SourceFiles { get; set; }
        public MultimediaType Type { get; set; }
    }


    public class SourceFile : EasyCollectionEntry<FileData>
    {

    }
}