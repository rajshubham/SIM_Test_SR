using PRGX.SIMTrax.ViewModel;

namespace PRGX.SIMTrax.ServiceFacade.Abstract
{
    public interface IEmailServiceFacade
    {
        EmailTemplate GetEmailMessage(string mailMnemonic, long localeId = 0, params object[] parameters);
        bool AddEmailAudit(EmailAudit emailAudit);
        void SendReportsEmail(string to, string bcc, string cc, string subject, string body, string directory, string companyName, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "");
        void SendEmail(string to, string bcc, string cc, string subject, string body, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "");
    }
}
