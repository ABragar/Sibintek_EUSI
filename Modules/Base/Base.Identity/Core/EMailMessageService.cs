using System;
using System.Threading.Tasks;
using Base.Service;
using Microsoft.AspNet.Identity;

namespace Base.Identity.Core
{
    public class EMailMessageService : IIdentityMessageService
    {
        private readonly IEmailService _internal;


        public EMailMessageService(IEmailService mail_service)
        {
            _internal = mail_service;
            
        }

        public Task SendAsync(IdentityMessage message)
        {

            bool failed = false;
            try
            {
                failed = !_internal.SendMail(message.Destination, message.Subject, message.Body, true);
            }
            catch (Exception ex)
            {

                throw new MailServiceException("Не удалось отправить письмо", ex);
            }
            if (failed)
                throw new MailServiceException("Не удалось отправить письмо");



            return Task.CompletedTask;
        }
    }


    public class MailServiceException : Exception
    {


        public MailServiceException(string message) : base(message)
        {
        }

        public MailServiceException(string message, Exception inner) : base(message, inner)
        {

        }


    }
}