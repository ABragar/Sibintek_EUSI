using System.Collections;
using Base.Utils.Common;

namespace Base.PBX.Models
{
    public class PBXUsersPageResult : IPageResult
    {
        public IEnumerable Result { get; set; }
        public int Total { get; set; }
    }
}