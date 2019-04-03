using System.Linq;
using Base.DAL;
using EUSI.Entities.NSI;

namespace EUSI.Services.Estate
{
    public static class EstateStatesHelper
    {
        public static int CreatedStateID { get; set; }
        public static int DirectedStateID { get; set; }
        public static int RejectedStateID { get; set; }
        public static int RedirectedStateID { get; set; }
        public static int VerifiedStateID { get; set; }
        public static int CreationStateID { get; set; }
        public static int CompletedStateID { get; set; }
        public static void UpdateCodeIds(IUnitOfWork uow)
        {
            var states =
                uow.GetRepository<EstateRegistrationStateNSI>().
                    Filter(nsi => !nsi.Hidden).
                    ToDictionary(nsi => nsi.Code, nsi => nsi.ID);
            CreatedStateID = states["CREATED"];
            DirectedStateID = states["DIRECTED"];
            RejectedStateID = states["REJECTED"];
            RedirectedStateID = states["REDIRECTED"];
            VerifiedStateID = states["VERIFIED"];
            CreationStateID = states["ESTATECREATION"];
            CompletedStateID = states["COMPLETED"];
        }
    }
}
