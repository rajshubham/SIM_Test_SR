using System.Collections;

namespace PRGX.SIMTrax.Domain.Util
{
    public class Email
    {
        public string ToEmailId { get; set; }
        public string CcEmailId { get; set; }
        public string BccEmailId { get; set; }
        public string Environment { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string AttachmentPath { get; set; }
        public string AttachmentName { get; set; }

        public string SIMLogoPath { get; set; }
        public string PRGXLogoPath { get; set; }
        public string EmailUncheckedLogoPath { get; set; }
        public string EmailCheckedLogoPath { get; set; }
        public string AppFilePhysicalPath { get; set; }
        public string LoginId { get; set; }

        public bool IsEmailOn { get; set; }
        public bool IsErrorMail { get; set; }
        public bool isBodyHtml { get; set; }
        public bool IsMultipleAttachment { get; set; }
        public Hashtable AttachmentList { get; set; }

    }
}
