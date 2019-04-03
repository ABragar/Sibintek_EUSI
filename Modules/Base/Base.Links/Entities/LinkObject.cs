using System;
using Base.Attributes;
using Base.Entities.Complex;
using Base.Translations;

namespace Base.Links.Entities
{
    public class LinkItem : BaseObject
    {
        private static readonly CompiledExpression<LinkItem, string> _sourceTitle =
            DefaultTranslationOf<LinkItem>.Property(x => x.SourceTitle).Is(x => x.SourceObject.Link.Title);

        private static readonly CompiledExpression<LinkItem, string> _sourceType =
            DefaultTranslationOf<LinkItem>.Property(x => x.SourceType).Is(x => x.SourceObject.Link.TypeName);

        private static readonly CompiledExpression<LinkItem, string> _destTitle =
            DefaultTranslationOf<LinkItem>.Property(x => x.DestTitle).Is(x => x.DestObject.Link.Title);

        private static readonly CompiledExpression<LinkItem, string> _destType =
            DefaultTranslationOf<LinkItem>.Property(x => x.DestType).Is(x => x.DestObject.Link.TypeName);


        public int SourceObjectID { get; set; }
        [DetailView("Источник")]
        [PropertyDataType("LinkItemBaseObject")]
        public virtual LinkItemBaseObject SourceObject { get; set; }

        [ListView("Источник(Наименование)")]
        public string SourceTitle => _sourceTitle.Evaluate(this);

        [ListView("Источник(Тип)")]
        public string SourceType => _sourceType.Evaluate(this);


        public int DestObjectID { get; set; }        
        [DetailView("Назначение")]
        [PropertyDataType("LinkItemBaseObject")]
        public virtual LinkItemBaseObject DestObject { get; set; }


        [ListView("Назначение(Наименование)")]
        public string DestTitle => _destTitle.Evaluate(this);

        [ListView("Назначение(Тип)")]
        public string DestType => _destType.Evaluate(this);

    }

    public class LinkItemComplexBaseObject : LinkBaseObject
    {
        [SystemProperty]
        [DetailView(ReadOnly = true, Name = "Наименование объекта")]
        [ListView]
        public string Title { get; set; }

        public LinkItemComplexBaseObject()
        {

        }

        public LinkItemComplexBaseObject(BaseObject obj)
            : base(obj)
        {

        }
    }

    public class LinkItemBaseObject : BaseObject
    {
        [ListView]
        [DetailView("Объект")]
        [SystemProperty]
        public LinkItemComplexBaseObject Link { get; set; } = new LinkItemComplexBaseObject();
    }
}
