namespace Base.PBX.Models
{
    public interface IPBXAccount
    {
        string extension { get; set; }
        string account_type { get; }
        string fullname { get; }
        bool hasvoicemail { get; set; }
        string cidnumber { get; set; }
        string secret { get; set; }
        string vmsecret { get; set; }
        bool skip_vmsecret { get; set; }
        bool auto_record { get; set; }
        bool enable_webrtc { get; set; }
        bool out_of_service { get; set; }


        //string context { get; set; }
        //string ring_timeout { get; set; }
        //bool encryption { get; set; }
        //bool faxdetect { get; set; }
        //bool fax_gateway { get; set; }
        //int strategy_ipacl { get; set; }
        //string local_network1 { get; set; }
        //string local_network2 { get; set; }
        //string local_network3 { get; set; }
        //string local_network4 { get; set; }
        //string local_network5 { get; set; }
        //string local_network6 { get; set; }
        //string local_network7 { get; set; }
        //string local_network8 { get; set; }
        //string local_network9 { get; set; }
        //string local_network10 { get; set; }
        //string specific_ip { get; set; }
        //bool bypass_outrt_auth { get; set; }
        //string type { get; set; }
        //string disallow { get; set; }
        //string allow { get; set; }
        //string permission { get; set; }

        //string host { get; set; }
        //int call_limit { get; set; }
        //bool callcounter { get; set; }
        //bool nat { get; set; }
        //bool directmedia { get; set; }
        //string dtmfmode { get; set; }
        //string insecure { get; set; }
        //string transport { get; set; }
        //bool enable_qualify { get; set; }
        //int qualify { get; set; }
        //int qualifyfreq { get; set; }
        //string authid { get; set; }
        //string tel_uri { get; set; }
        //string cfb { get; set; }
        //string cfn { get; set; }
        //string cfu { get; set; }
        //bool enablehotdesk { get; set; }
        //string user_outrt_passwd { get; set; }
        //bool out_of_service { get; set; }
        //int cfu_timetype { get; set; }
        //int cfn_timetype { get; set; }
        //int cfb_timetype { get; set; }
        //string mohsuggest { get; set; }
        //string cc_agent_policy { get; set; }
        //string cc_monitor_policy { get; set; }
        //int ccbs_available_timer { get; set; }
        //int ccnr_available_timer { get; set; }
        //int cc_offer_timer { get; set; }
        //int cc_max_agents { get; set; }
        //int cc_max_monitors { get; set; }
        //bool en_ringboth { get; set; }
        //string external_number { get; set; }
        //int ringboth_timetype { get; set; }
        //int skip_auth_timetype { get; set; }
        //bool enable_ldap { get; set; }
        //string limitime { get; set; }
        //bool t38_udptl { get; set; }
        //int max_contacts { get; set; }
        //bool fax_intelligent_route { get; set; }
        //bool fax_intelligent_route_destination { get; set; }
        //bool use_avpf { get; set; }
        //bool ice_support { get; set; }
        //bool media_encryption { get; set; }
        //bool intranet_ip_filter { get; set; }
        //string alertinfo { get; set; }
        //bool dnd { get; set; }
        //int dnd_timetype { get; set; }
    }
}