using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace OrderImportClasses
{
    public class MailHelper

    {

        public List<string> AttachmentPath { get; set; }

        public string Body { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FromEmail { get; set; }

        public bool IsHtmlBody { get; set; }

        public bool ReadBodyFromFile { get; set; }

        public string Subject { get; set; }

        public string ToEmail { get; set; }

        public string HostName { get; set; }

        public string SiteName { get; set; }

        public string SMTPEmail { get; set; }

        public string SMTPPassword { get; set; }

        public int SMTPPort { get; set; }

        public bool SendEmail()

        {

            StringBuilder strBody = new StringBuilder();

            string MailTo = this.ToEmail;

            string HostName = this.HostName;

            MailMessage mailObj = new MailMessage();

            mailObj.From = new MailAddress(this.FromEmail, this.SiteName);

            mailObj.Subject = this.Subject;

            mailObj.Body = this.Body;

            mailObj.To.Add(this.ToEmail);

            if (this.AttachmentPath != null)

                foreach (string filePath in this.AttachmentPath)

                {

                    if (System.IO.File.Exists(filePath))

                    {

                        Attachment attachment;

                        attachment = new Attachment(filePath);

                        mailObj.Attachments.Add(attachment);

                    }

                }

            mailObj.IsBodyHtml = true;



            mailObj.Priority = MailPriority.Normal;

            SmtpClient SMTPServer = new SmtpClient(HostName);

            SMTPServer.UseDefaultCredentials = false;

            SMTPServer.Port = SMTPPort;

            SMTPServer.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPPassword);

            SMTPServer.EnableSsl = true;

            SMTPServer.Send(mailObj);



            return true;

        }

    }


}
