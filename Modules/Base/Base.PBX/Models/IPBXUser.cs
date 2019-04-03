namespace Base.PBX.Models
{
    public interface IPBXUser
    {
        int user_id { get; set; }
        string first_name { get; }
        string last_name { get; }
        string user_password { get; set; }

        bool email_to_user { get; set; }
        string email { get; }
        string phone_number { get; }

        //string department { get; set; }
        //bool enable_multiple_extension { get; set; }
        //string multiple_extension { get; set; }

        //string cookie { get; set; }
        //string family_number { get; set; }
        //string language { get; set; }
        //string login_time { get; set; }
        //int privilege { get; set; }
    }




}


