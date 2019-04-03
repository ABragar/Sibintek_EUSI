using System.Collections.Generic;
using Base.Service;

namespace Base.Mail.Service
{
    public class DummyEmailService : IEmailService
    {
        public bool SendMail(string mailto, string caption, string message, bool isBodyHtml = false)
        {
            return false;
        }

        public int SendMail(IEnumerable<string> mailto, string caption, string message, bool isBodyHtml = false)
        {
            return 0;
        }

        public bool SendMail(string mailto, string caption, string templateName, Dictionary<string, string> placeholders, bool isBodyHtml = false)
        {
            return false;
        }

        public int SendMail(IEnumerable<string> mailto, string caption, string templateName, Dictionary<string, string> placeholders, bool isBodyHtml = false)
        {
            return 0;
        }
    }
}