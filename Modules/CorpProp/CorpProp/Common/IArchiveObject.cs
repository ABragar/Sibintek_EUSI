using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Common
{

    /// <summary>
    /// Предоставляет свойства "архивного" объекта.
    /// </summary>
    public interface IArchiveObject
    {
        bool? IsArchived { get; set; }
    }
}
