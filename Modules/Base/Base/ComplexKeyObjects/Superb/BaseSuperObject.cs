using System;
using Base.Attributes;

namespace Base.ComplexKeyObjects.Superb
{
    public abstract class BaseSuperObject<TSuperObject> : BaseObject, ISuperObject<TSuperObject>
        where TSuperObject : BaseSuperObject<TSuperObject>
    {
        [ListView]
        public string ExtraID {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}