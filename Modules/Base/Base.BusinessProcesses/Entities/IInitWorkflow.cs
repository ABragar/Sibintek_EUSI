using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessProcesses.Entities
{
    public interface IInitBPObject : IBPObject
    {
        Workflow InitWorkflow { get; set; }
    }
}
