using System;
using System.Collections.Generic;

namespace Base.ComplexKeyObjects.Common
{
    public interface ITypeRelationService
    {

        string FindName(Type type);

        IReadOnlyCollection<string> GetRelations(Type type);

        void AddRelation(Type item_type, string name);

        void AddRelation(Type item_type, Type child_type);

        void ResolveNames();
    }
}