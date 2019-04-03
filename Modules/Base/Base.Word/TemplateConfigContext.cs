using System;
using Base.DAL;

namespace Base.Word
{
    [Serializable]
    public class TemplateConfigContext
    {
        public TemplateConfigContext(IUnitOfWork unit_of_work)
        {
            UnitOfWork = unit_of_work;
        }

        public IUnitOfWork UnitOfWork { get; }
    }
}