using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace Data.Entities
{
    public class GroupSociety : BaseObject
    {
       // public CategoryOG CategoryOG { get; set; }

        public bool isMain { get; set; }

        public bool IsJoint { get; set; }

        public bool IsResident { get; set; }

        public bool IsControlled { get; set; }

      //  public Currency CurrencyOfCompany { get; set; }

      //  public ConsolidationUnit ConsolidationUnit{ get; set; }

      //  public ConsolidationUnit BusinessDirection { get; set; } Какой-то бред

        public double ShareInTheCapital { get; set; }

        public double ShareInVotingShares { get; set; }

        public long SizeOfAuthorizedCapital { get; set; }

        public DateTime DateOfInclusionInGroup { get; set; }

        public DateTime DateOfExclusionFromGroup { get; set; }

        public DateTime DateOfInclusionInPerimeter { get; set; }

        public DateTime DateOfExclusionFromPerimeter { get; set; }

        public string NameEIO { get; set; }

        public string EIO { get; set; }

        public string Curator { get; set; }

        public string ActualActivity { get; set; }

    }
}
