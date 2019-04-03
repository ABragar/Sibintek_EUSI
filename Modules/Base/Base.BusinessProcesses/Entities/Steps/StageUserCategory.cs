using Base.Security;

namespace Base.BusinessProcesses.Entities.Steps
{
    public class StageUserCategory : EasyCollectionEntry<UserCategory>
    {
        public int StageID { get; set; }
        public virtual Stage Stage { get; set; }
    }


}