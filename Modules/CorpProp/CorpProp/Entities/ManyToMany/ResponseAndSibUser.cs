using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using CorpProp.Entities.Request;
using CorpProp.Entities.Security;

namespace CorpProp.Entities.ManyToMany
{
    /// <summary>
    /// Представляет соисполнителей ответов в запросах.
    /// </summary>
    public class ResponseAndSibUser: ManyToManyAssociation<Response, SibUser>
    {
        public ResponseAndSibUser(): base() { }
    }
}
