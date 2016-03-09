using PRGX.SIMTrax.DAL;
using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using System;

namespace PRGX.SIMTrax.ServiceFacade
{
    public class EmailServiceFacade : IEmailServiceFacade
    {

        private readonly IEmailUow _emailUow;

        public EmailServiceFacade()
        {
            _emailUow = new EmailUow();
        }
        public PRGX.SIMTrax.ViewModel.EmailTemplate GetEmailMessage(string mailMnemonic, long localeId = 0, params object[] parameters)
        {
            Logger.Info("EmailServiceFacade : GetEmailMessage() : Entering the method");
            try
            {
                var emailTemplatePM = _emailUow.EmailTemplates.GetEmailTemplate(mailMnemonic, localeId);
                Logger.Info("EmailServiceFacade : GetEmailMessage() : Entering the method");
                var emailTemplate = new PRGX.SIMTrax.ViewModel.EmailTemplate();
                if (emailTemplatePM != null)
                {
                    emailTemplate.Id = emailTemplatePM.Id;
                    emailTemplate.Mnemonic = emailTemplatePM.Mnemonic;
                    emailTemplate.Subject = emailTemplatePM.Subject;
                    emailTemplate.Content = string.Format(emailTemplatePM.Content, parameters);
                    emailTemplate.RefLocale = emailTemplatePM.RefLocale; 
                }
                return emailTemplate;
            }
            catch (System.Exception ex)
            {
                Logger.Error("EmailServiceFacade : GetEmailMessage() : Caught an exception " + ex);
                throw ex;
            }
        }

        //public Email AddEmail(EmailDM emailDM)
        //{
        //    Logger.Info("EmailService : AddEmail(Email email) : Enter into method");
        //    Email email = null;
        //    try
        //    {
        //        var emailPM = _mailRepository.AddEmail(emailDM.ToDAL());
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("EmailService : AddEmail(Email email) : Caught an error" + ex);
        //        throw;
        //    }
        //    Logger.Info("EmailService : AddEmail(Email email) : Exit from method");
        //    return email;
        //}

        //public List<EmailThread> GetEmails(long senderOrReceiverId, bool isSentItems)
        //{
        //    Logger.Info("EmailService : GetEmails(long receiverId) : Enter into method");
        //    List<EmailThread> thread = null;
        //    try
        //    {
        //        var threadPM = _mailRepository.GetEmails(senderOrReceiverId, isSentItems);
        //        thread = threadPM.ToDomain();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("EmailService : GetEmails(long receiverId) : Caught an error" + ex);
        //        throw;
        //    }
        //    Logger.Info("EmailService :GetEmails(long receiverId): Exit from method");
        //    return thread;
        //}

        //public EmailDM GetEmail(long emailId)
        //{
        //    Logger.Info("EmailService : GetEmail(long receiverId) : Enter into method");
        //    EmailDM email = null;
        //    try
        //    {
        //        var emailPM = _mailRepository.GetEmail(emailId);
        //        email = emailPM.ToDomain();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("EmailService : GetEmail(long receiverId) : Caught an error" + ex);
        //        throw;
        //    }
        //    Logger.Info("EmailService :GetEmail(long receiverId): Exit from method");
        //    return email;
        //}

        public bool AddEmailAudit(PRGX.SIMTrax.ViewModel.EmailAudit emailAudit)
        {
            Logger.Info("EmailServiceFacade : AddEmailAudit(EmailAudit emailAudit) : Enter into method");
            bool response = false;
            try
            {
                var emailAuditPM = new EmailAudit();
               
                emailAuditPM.Id = emailAudit.Id;
                emailAuditPM.Bcc = emailAudit.Bcc;
                emailAuditPM.From = emailAudit.From;
                emailAuditPM.To = emailAudit.To;
                emailAuditPM.SentDate = emailAudit.SentDate;
                emailAuditPM.Subject = emailAudit.Subject;
                emailAuditPM.Body = emailAudit.Body;
                emailAuditPM.IsEmailSent = emailAudit.IsEmailSent;
                var result = _emailUow.EmailAudits.Add(emailAuditPM);
                if (result != null)
                {
                    response = true;
                } 
                _emailUow.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error("EmailServiceFacade : AddEmailAudit(EmailAudit emailAudit) : Caught an error" + ex);
                throw;
            }
            Logger.Info("EmailServiceFacade : AddEmailAudit(EmailAudit emailAudit) : Exit from method");
            return response;
        }

        //public bool UpdateEmailAudit(EmailAudit emailAudit)
        //{
        //    Logger.Info("EmailService : UpdateEmailAudit(EmailAudit emailAudit) : Enter into method");
        //    bool response = false;
        //    try
        //    {
        //        response = _emailAuditRepository.UpdateEmailAudit(emailAudit.ToDAL());
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("EmailService : UpdateEmailAudit(EmailAudit emailAudit) : Caught an error" + ex);
        //        throw;
        //    }
        //    Logger.Info("EmailService : UpdateEmailAudit(EmailAudit emailAudit) : Exit from method");
        //    return response;
        //}

        delegate void SendEmailDelegate(string to, string bcc, string cc, string subject, string body, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "");

        public void SendEmail(string to, string bcc, string cc, string subject, string body, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "")
        {
            Logger.Info("EmailService : SendEmail() : Enter into method");
            try
            {
                if (attachmentPath == string.Empty)
                {
                    SendEmailDelegate sendEmailDelegate;
                    sendEmailDelegate = new SendEmailDelegate(SendEmailUsingDelegate);
                    sendEmailDelegate.BeginInvoke(to, bcc, cc, subject, body, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath, attachmentPath, attachmentName, null, null);
                }
                else
                {
                    SendEmailUsingDelegate(to, bcc, cc, subject, body, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath, attachmentPath, attachmentName);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("");
                throw;
            }
            Logger.Info("EmailService:SendEmail():Exiting method ");
        }

        private void SendEmailUsingDelegate(string to, string bcc, string cc, string subject, string body, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "")
        {
            Logger.Info("EmailService : SendEmailUsingDelegate() : Enter into method");
            try
            {
                var isEmailSent = Emailer.SendMail(to, string.Empty, string.Empty, subject, body, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath, attachmentPath, attachmentName);
                var emailAudit = new PRGX.SIMTrax.ViewModel.EmailAudit()
                {
                    Bcc = bcc,
                    From = Constants.FROM_EMAIL_ID,
                    To = to,
                    cc = cc,
                    SentDate = DateTime.Now,
                    Subject = subject,
                    Body = isEmailSent ? string.Empty : body,
                    IsEmailSent = isEmailSent,
                };
                AddEmailAudit(emailAudit);
            }
            catch (Exception ex)
            {
                Logger.Error("EmailService : SendEmailUsingDelegate() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("EmailService : SendEmailUsingDelegate() : Exit from method");
        }



        public void SendReportsEmail(string to, string bcc, string cc, string subject, string body, string directory, string companyName, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "")
        {
            Logger.Info("EmailService : SendReportsEmail() : Enter into method");
            try
            {
                if (attachmentPath == string.Empty)
                {
                    SendReportsEmailDelegate sendEmailDelegate;
                    sendEmailDelegate = new SendReportsEmailDelegate(SendReportsEmailUsingDelegate);
                    sendEmailDelegate.BeginInvoke(to, bcc, cc, subject, body, directory, companyName, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath, attachmentPath, attachmentName, null, null);
                }
                else
                {
                    SendReportsEmailUsingDelegate(to, bcc, cc, subject, body, directory, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath, attachmentPath, attachmentName);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("EmailService : SendReportsEmail() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("EmailService:SendEmail():Exiting method ");
        }

        delegate void SendReportsEmailDelegate(string to, string bcc, string cc, string subject, string body, string directory, string companyName, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "");

        private void SendReportsEmailUsingDelegate(string to, string bcc, string cc, string subject, string body, string directory, string companyName, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "")
        {
            Logger.Info("EmailService : SendReportsEmailUsingDelegate() : Enter into method");
            try
            {
                var isEmailSent = Emailer.SendReportsMail(to, string.Empty, string.Empty, subject, body, directory, companyName, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath, attachmentPath, attachmentName);
                var emailAudit = new PRGX.SIMTrax.ViewModel.EmailAudit()
                {
                    Bcc = bcc,
                    From = Constants.FROM_EMAIL_ID,
                    To = to,
                    cc = cc,
                    SentDate = DateTime.Now,
                    Subject = subject,
                    Body = isEmailSent ? string.Empty : body,
                    IsEmailSent = isEmailSent,
                };
                AddEmailAudit(emailAudit);
            }
            catch (Exception ex)
            {
                Logger.Error("EmailService : SendReportsEmailUsingDelegate() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("EmailService : SendReportsEmailUsingDelegate() : Exit from method");
        }
    }
}
