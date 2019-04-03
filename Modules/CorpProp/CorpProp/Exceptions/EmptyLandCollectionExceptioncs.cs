using System;

namespace CorpProp.Exceptions
{
    public class EmptyLandCollectionException : Exception
    {
        public EmptyLandCollectionException(string msg, int id) : base(msg)
        {
            ID = id;
        }

        public int ID { get; private set; }
    }
}
