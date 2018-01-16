using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fido_Main.Notification.Email
{
    class EmailAddress
    {
        public string SendTo { get; private set; }
        public string SendFrom { get; private set; }
        public string SendCopy { get; private set; }

        public bool HasSendCopyField
        {
            get
            {
                return !string.IsNullOrEmpty(SendCopy);
            }
        }

        public bool HasSendToField
        {
            get
            {
                return !string.IsNullOrEmpty(SendTo);
            }
        }

        public EmailAddress(string sendTo, string sendFrom)
        {
            this.SendTo = sendTo;
            this.SendFrom = sendFrom;
        }

        public EmailAddress(string sendTo, string sendFrom, string sendCopy)
            : this(sendTo, sendFrom)
        {
            this.SendCopy = sendCopy;
        }

    }
}

