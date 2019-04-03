using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;

namespace Base.Entities.Complex
{
    [ComplexType]
    public class LinkBaseObject
    {
        public LinkBaseObject() { }

        public LinkBaseObject(BaseObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var objType = obj.GetType().GetBaseObjectType();

            ID = obj.ID;
            TypeName = objType.GetTypeName();
        }

        public LinkBaseObject(Type type, int id)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var objType = type.GetBaseObjectType();

            ID = id;
            TypeName = objType.GetTypeName();
        }

        [SystemProperty]
        public int ID { get; set; }
        [SystemProperty]
        public string TypeName { get; set; }
        [SystemProperty]
        public string Mnemonic { get; set; }
        public Type GetTypeBo()
        {
            return Type.GetType(TypeName);
        }
        public override string ToString()
        {
            return $"{TypeName}_{ID}";
        }
    }
}
