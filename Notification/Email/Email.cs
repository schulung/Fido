using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fido_Main.Notification.Email
{
    class Email
    {
        public EmailAddress EmailAddress { get; private set; }
        public EmailContent EmailContent { get; private set; }
        public string Subject { get; private set; }

        public Email(EmailAddress address, string subject, EmailContent content)
        {
            this.EmailAddress = address;
            this.EmailContent = content;
            this.Subject = subject;
        }
    }
}
