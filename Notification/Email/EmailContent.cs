using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fido_Main.Notification.Email
{
    class EmailContent
    {
        public string Body { get; private set; }
        public List<string> GaugeAttachment { get; private set; }
        public string EmailAttachment { get; private set; }

        public EmailContent(string body)
        {
            this.Body = body;
        }

        public EmailContent(string body, List<string> gaugeAttachment, string emailAttachment)
            : this(body)
        {
            this.GaugeAttachment = gaugeAttachment;
            this.EmailAttachment = emailAttachment;
        }

        
    }
}
