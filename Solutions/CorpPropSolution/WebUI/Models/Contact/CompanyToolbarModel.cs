using Base.Contact.Entities;

namespace WebUI.Models.Contact
{
    public class CompanyToolbarModel
    {


        public CompanyToolbarModel(int? company)
        {
            CompanyID = company;
        }

        public int? CompanyID { get; set; }
    }
}