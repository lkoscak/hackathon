using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetWebApi.BusinessLogic.EmailNS.Model
{
    public class Email
    {
        public string subject = "";
        public string mailTo = "";
        public string text = "";
        public int isHtml = 1;
        public string mailFrom = "";
        public string attachmentName = "";
        public string contentType = "";
        public byte[] Content = null;
        public string attachmentName2 = "";
        public string contentType2 = "";
        public byte[] Content2 = null;

        public Email(string subject, string mailTo, string text, int isHtml, string mailFrom, string attachmentName, string contentType, byte[] Content)
        {
            this.subject = subject;
            this.mailTo = mailTo;
            this.text = text;
            this.isHtml = isHtml;
            this.mailFrom = mailFrom;
            this.attachmentName = attachmentName;
            this.contentType = contentType;
            this.Content = Content;

        }

        public Email(string subject, string mailTo, string text, int isHtml, string mailFrom, string attachmentName, string contentType, byte[] Content, string attachmentName2, string contentType2, byte[] Content2)
        {
            this.subject = subject;
            this.mailTo = mailTo;
            this.text = text;
            this.isHtml = isHtml;
            this.mailFrom = mailFrom;
            this.attachmentName = attachmentName;
            this.contentType = contentType;
            this.Content = Content;
            this.attachmentName2 = attachmentName2;
            this.contentType2 = contentType2;
            this.Content2 = Content2;

        }

    }
}