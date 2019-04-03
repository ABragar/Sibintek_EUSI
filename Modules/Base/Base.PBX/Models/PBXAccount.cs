namespace Base.PBX.Models
{
    public class PBXAccount : IPBXAccount
    {
        public string extension { get; set; }

        public string account_type { get; set; }

        public string fullname { get; set; }


        public bool hasvoicemail { get; set; }

        public string cidnumber { get; set; }

        public string secret { get; set; }

        public string vmsecret { get; set; }

        public bool skip_vmsecret { get; set; }

        public bool auto_record { get; set; }

        public bool enable_webrtc { get; set; }
        public bool out_of_service { get; set; }
    }
}