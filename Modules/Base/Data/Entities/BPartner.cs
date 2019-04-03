//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Base;

//namespace Data.Entities
//{
//    public class BPartner : BaseObject
//    {
//        public int? AppraiserID { get; set; }
//        public virtual Appraiser Appraiser { get; set; }

//        public int? GroupSocietyID { get; set; }

//        public GroupSociety GroupSociety { get; set; }

//        public BPType BpType { get; set; }

//        public BPKind BpKind { get; set; }

//        public long OGRN { get; set; }

//        public long OGRNIP { get; set; }

//        public OKOFS OKOFS { get; set; }

//        public OKOPS OKOPS { get; set; }

//        public OKOGU OKOGU { get; set; }

//        public OKVED OKVED { get; set; }

//        public long INN { get; set; }

//        public long KPP { get; set; }

//        public long OKPO { get; set; }

//        // OKATO Непонятно

//        public DateTime RegistrationDate { get; set; }

//        public Country Country { get; set; }

//        public string LegalAddress { get; set; }

//        public string FactAddress { get; set; }

//        public string FullName { get; set; }

//        public string Name { get; set; }

//        public long Phone { get; set; }

//        public long Fax { get; set; }

//        public string Email { get; set; }

//        public string HeadName { get; set; }

//        public string HeadPosition { get; set; }

//        public bool IsSubjectSME { get; set; }

//        public bool IsInBankruptcy { get; set; }

//        public bool ExistenceOfClaims { get; set; }

//        public bool NegativeFacts { get; set; }

//        public string Description { get; set; }
//    }


//    public enum BPType
//    {
//    }

//    public enum BPKind
//    {
//    }

//    public enum OKOFS
//    {
//    }

//    public enum OKOPS
//    {
//    }

//    public enum OKOGU
//    {
//    }

//    public enum OKVED
//    {
//    }

//    public enum Country
//    {
//    }


//}
