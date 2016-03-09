﻿using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace PRGX.SIMTrax.Domain.Util
{
    public class Emailer
    {
        public static bool SendEmail(Email email)
        {
            Logger.Info("Emailer :SendEmail(Email email): Enter into method ");
            var result = false;
            try
            {
                MailMessage mailMessage = new MailMessage();
                FileStream fileStream = null;


                #region Set To, Cc, Bcc
                if (email.IsErrorMail)
                {
                    if (!(string.IsNullOrWhiteSpace(Configuration.ErrorMailReceiver1)))
                        mailMessage.To.Add(new MailAddress(Configuration.ErrorMailReceiver1));

                    if (!(string.IsNullOrWhiteSpace(Configuration.ErrorMailReceiver2)))
                        mailMessage.To.Add(new MailAddress(Configuration.ErrorMailReceiver2));

                    if (!(string.IsNullOrWhiteSpace(Configuration.ErrorMailReceiver3)))
                        mailMessage.To.Add(new MailAddress(Configuration.ErrorMailReceiver3));

                    if (!(string.IsNullOrWhiteSpace(Configuration.ErrorMailReceiver4)))
                        mailMessage.To.Add(new MailAddress(Configuration.ErrorMailReceiver4));

                    if (!(string.IsNullOrWhiteSpace(Configuration.ErrorMailReceiver5)))
                        mailMessage.To.Add(new MailAddress(Configuration.ErrorMailReceiver5));

                    if (!(string.IsNullOrWhiteSpace(Configuration.ErrorMailReceiver6)))
                        mailMessage.To.Add(new MailAddress(Configuration.ErrorMailReceiver6));
                    mailMessage.Body = email.Content + "<br/><br/><div>Login ID :" + email.LoginId + "<br/>Machine Name :" + Environment.MachineName + "<br/>Environment :" + email.Environment + "</div>";
                    mailMessage.IsBodyHtml = email.isBodyHtml;
                }
                else if (!string.IsNullOrWhiteSpace(Configuration.Environment) && Configuration.Environment == Constants.ENVIRONMENT_UAT)
                {
                    mailMessage.To.Add(new MailAddress("paul.fagg@prgx.com"));
                    mailMessage.Bcc.Add(string.Empty);
                    mailMessage.CC.Add(string.Empty);
                }

                else if (!string.IsNullOrWhiteSpace(Configuration.Environment) && Configuration.Environment == Constants.ENVIRONMENT_PROD)
                {
                    mailMessage.To.Add(new MailAddress(email.ToEmailId));
                    mailMessage.Bcc.Add(new MailAddress(email.BccEmailId));
                    mailMessage.CC.Add(new MailAddress(email.CcEmailId));
                }
                else
                {
                    var to = string.IsNullOrWhiteSpace(Configuration.ErrorMailReceiver3) ? Configuration.ErrorMailReceiver3 : "shubham.raj@raremile.com";
                    mailMessage.To.Add(to);
                    mailMessage.Bcc.Add(string.Empty);
                    mailMessage.CC.Add(string.Empty);
                }
                #endregion

                mailMessage.From = new MailAddress(Constants.FROM_EMAIL_ID);
                mailMessage.Subject = email.Subject;

                #region Template setting
                if (!email.IsErrorMail)
                {
                    string commonEmailTemplate = CommonMethods.GetEmailTemplate();
                    commonEmailTemplate = string.Format(commonEmailTemplate, email.Content);

                    String tmp = @"<style>
                                    @font-face
                                    {
                                        font-family: Aptifer Sans LT;
                                        src: url(cid:email_font_Aptifer) format('truetype');
                                    }
                                    </style>";
                    commonEmailTemplate = String.Concat(tmp, commonEmailTemplate);
                    // If there is any emailLogoURL then attach the image
                    // otherwise call the other method to embedd the image.
                    if (string.IsNullOrWhiteSpace(email.SIMLogoPath) && (File.Exists(email.SIMLogoPath) == false))
                    {
                        //// Set the body of the mail message
                        mailMessage.Body = email.Content;
                        // Set the format of the mail message body as HTML
                        mailMessage.IsBodyHtml = email.isBodyHtml;
                    }
                    else
                    {
                        // embbed image-url
                        EmbbedImageIntoMailMessage(mailMessage, commonEmailTemplate, email.SIMLogoPath, email.PRGXLogoPath, email.EmailCheckedLogoPath, email.EmailUncheckedLogoPath, email.AppFilePhysicalPath);

                        if (email.IsMultipleAttachment)
                        {
                            if (email.AttachmentList != null)
                            {
                                int attachmentsListSize = email.AttachmentList.Count;
                                IEnumerator iEnumAttachments = email.AttachmentList.Keys.GetEnumerator();
                                for (int index = 0; index < email.AttachmentList.Count; index++)
                                {
                                    iEnumAttachments.MoveNext();
                                    string attachmentName = iEnumAttachments.Current.ToString();
                                    string attachmentPath = (string)email.AttachmentList[attachmentName];
                                    FileStream fs = new FileStream(attachmentPath, FileMode.Open, FileAccess.Read, FileShare.Read, 256, FileOptions.None);
                                    mailMessage.Attachments.Add(new Attachment(fs, attachmentName));
                                }
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(email.AttachmentPath) == false)
                        {
                            try
                            {
                                fileStream = new FileStream(@email.AttachmentPath, FileMode.Open, FileAccess.Read, FileShare.Read, 256, FileOptions.None);
                                mailMessage.Attachments.Add(new Attachment(fileStream, email.AttachmentName));
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("Emailer:SendMail():Exception while attaching the pdf file" + ex);
                            }
                        }
                    }

                    // Set the priority of the mail message to normal
                    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                }
                #endregion

                SmtpClient mSmtpClient = GetSMTPClient();

                // Send the mail message
                mSmtpClient.Send(mailMessage);

                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Emailer:SendMailMessageUsingSmtp():Caught Exception." + ex);
                //throw;
            }
            return result;
        }

        private static void EmbbedImageIntoMailMessage(MailMessage mailMessage, string mailBody, string imageSIMLogoURL, string imagePRGXLogoURL, string imageCheckedLogoUrl, string imageUncheckedLogoUrl, string appFilePhysicalPath)
        {
            Logger.Info("Emailer:EmbbedImageIntoMailMessage(): Entering method.");
            try
            {
                //first we create the Plain Text part
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/plain");

                //then we create the Html part
                //to embed images, we need to use the prefix 'cid' in the img src value
                //the cid value will map to the Content-Id of a Linked resource.
                //thus <img src='cid:companylogo'> will map to a LinkedResource with a ContentId of 'companylogo'
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");

                if ((string.IsNullOrWhiteSpace(imageSIMLogoURL) == false) && File.Exists(imageSIMLogoURL))
                {
                    //create the LinkedResource (embedded image)
                    LinkedResource logoSIM = new LinkedResource(imageSIMLogoURL);
                    logoSIM.ContentId = "email_SIM_logo";
                    //add the LinkedResource to the appropriate view
                    htmlView.LinkedResources.Add(logoSIM);
                }

                if ((string.IsNullOrWhiteSpace(imagePRGXLogoURL) == false) && File.Exists(imagePRGXLogoURL))
                {
                    //create the LinkedResource (embedded image)
                    LinkedResource logoPRGX = new LinkedResource(imagePRGXLogoURL);
                    logoPRGX.ContentId = "email_prgx_logo";
                    //add the LinkedResource to the appropriate view
                    htmlView.LinkedResources.Add(logoPRGX);
                }

                //if ((string.IsNullOrWhiteSpace(imageCheckedLogoUrl) == false) && File.Exists(imageCheckedLogoUrl))
                //{
                //    //create the LinkedResource (embedded image)
                //    LinkedResource logoChecked = new LinkedResource(imageCheckedLogoUrl);
                //    logoChecked.ContentId = "email_checked_logo";
                //    //add the LinkedResource to the appropriate view
                //    htmlView.LinkedResources.Add(logoChecked);
                //}

                //if ((string.IsNullOrWhiteSpace(imageUncheckedLogoUrl) == false) && File.Exists(imageUncheckedLogoUrl))
                //{
                //    //create the LinkedResource (embedded image)
                //    LinkedResource logoUnchecked = new LinkedResource(imageUncheckedLogoUrl);
                //    logoUnchecked.ContentId = "email_unchecked_logo";
                //    //add the LinkedResource to the appropriate view
                //    htmlView.LinkedResources.Add(logoUnchecked);
                //}

                //add the views
                mailMessage.AlternateViews.Add(plainView);
                mailMessage.AlternateViews.Add(htmlView);
            }
            catch (Exception ex)
            {
                Logger.Error("Emailer:EmbbedImageIntoMailMessage(): Caught an exception.");
                throw ex;
            }
            Logger.Info("Emailer:EmbbedImageIntoMailMessage(): Entering method.");
        }

        private static SmtpClient GetSMTPClient()
        {
            Logger.Info("Emailer:GetSMTPClient(): Entering method.");
            SmtpClient smtpClient = null;
            try
            {

                smtpClient = new SmtpClient
                {
                    Host = Constants.EMAIL_HOST,
                    Port = Constants.EMAIL_PORT,
                    EnableSsl = Constants.EMAIL_SSLON,

                };
                if (Constants.EMAIL_NEEDS_AUTHENTICATION)
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(Constants.EMAIL_USER_ID, Constants.EMAIL_PASSWORD);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Emailer:GetSMTPClient(): Caught an exception." + ex);
                throw;
            }

            Logger.Info("Emailer:GetSMTPClient(): Exiting method.");

            // return
            return smtpClient;
        }

        #region CommentedCode
        //public static void NotifyErrorMail(string logStatement)
        //{
        //    try
        //    {
        //        if (Constants.IS_EMAIL_ON)
        //        {
        //            Logger.Info("Emailer:NotifyErrorMail(): Notifying admin about exception: " + logStatement);                    
        //            //SendMail(content, true, SIMConstants.ADMIN_EMAIL_ID, " URGENT: iPartner : Error In " + SIMConstants.ENVIRONMENT, null, null);
        //            SendErrorMail(logStatement, true, "", " URGENT: iPartner : Error In " + Configuration.Environment, Configuration.Environment, Environment.MachineName);
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        Logger.Error("Emailer.NotifyErrorMail(): Caught exception while sending email" + exp);
        //        throw;
        //    }
        //}

        //public static int SendErrorMail(string content, bool isBodyHtml, string emailId, string subject, string environment, string machineName)
        //{
        //    string CCemailID = string.Empty;
        //    //if ((ConfigurationManager.AppSettings["DeployementLocation"] != null) && (ConfigurationManager.AppSettings["DeployementLocation"].ToUpper() == "UAT"))
        //    //{
        //    // emailId = "paul.fagg@prgx.com";
        //    //}
        //    emailId = "sourabh@raremile.com";
        //    string emailId2 = "vinodh.kumar@raremile.com";
        //    int returnStatus = 0;
        //    Logger.info("Emailer:SendMail():Entered the method");
        //    Logger.debug("Emailer:SendMail():Sending email to :" + emailId);
        //    try
        //    {
        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(Constants.FROM_EMAIL_ID);
        //        message.To.Add(new MailAddress(emailId));
        //        message.To.Add(new MailAddress(emailId2));

        //        //set the subject and body content.
        //        message.Subject = subject;
        //        message.Body = content;// +EmailHelper.SIGNATURE_TEXT;
        //        message.IsBodyHtml = isBodyHtml;

        //        //Create SMTP client and set the credentials
        //        SmtpClient client = new SmtpClient();
        //        client.Host = SIMConstants.EMAIL_HOST;
        //        client.Port = SIMConstants.EMAIL_PORT;

        //        if (SIMConstants.EMAIL_NEEDS_AUTHENTICATION)
        //        {
        //            client.UseDefaultCredentials = false;
        //            client.Credentials = new System.Net.NetworkCredential(SIMConstants.EMAIL_USER_ID, SIMConstants.EMAIL_PASSWORD);
        //        }

        //        client.EnableSsl = SIMConstants.EMAIL_SSLON;
        //        Logger.debug("SSL Enabled for mail :" + client.EnableSsl.ToString());
        //        try
        //        {

        //            client.Send(message);

        //        }
        //        catch (Exception exp)
        //        {
        //            Logger.error("Exception in sending mail :" + exp.Message, exp);
        //            Logger.error("Stack Trace :" + exp.StackTrace);
        //            returnStatus = 1;
        //        }
        //        Logger.info("Emailer:SendMail():Leaving the method");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.fatal("Emailer:SendMail(): Exception in sending mail to " + emailId + " : " + ex.Message);
        //        returnStatus = 1;
        //    }
        //    finally
        //    {
        //        Logger.debug("Emailer:SendMail(): Closed the pdf file");

        //    }

        //    //return
        //    return returnStatus;
        //}

        //        public static int SendMailWithMultipleAttachments(string ccEmailId, string content, string emailId, string subject, Hashtable attachmentsList)
        //        {
        //            if ((ConfigurationManager.AppSettings["DeployementLocation"] != null) && (ConfigurationManager.AppSettings["DeployementLocation"].ToUpper() == "UAT"))
        //            {
        //                emailId = "paul.fagg@prgx.com";
        //                ccEmailId = string.Empty;
        //            }
        //            Logger.info("Emailer:SendMail():Entering method ");
        //            int iStatus = 1;
        //            Logger.debug("Emailer:SendMail():Sending email to :" + emailId);
        //            //bool isEmailIdValid = false;
        //            try
        //            {
        //                MailMessage message = new MailMessage();
        //                message.From = new MailAddress(SIMConstants.FROM_EMAIL_ID);
        //                if (emailId.Contains(";"))
        //                {
        //                    string[] emails = emailId.Split(';');
        //                    foreach (string email in emails)
        //                    {
        //                        if (email != null && !email.Equals(""))
        //                        {
        //                            message.To.Add(new MailAddress(email));
        //                            //isEmailIdValid = true;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    message.To.Add(new MailAddress(emailId));
        //                }
        //                if (ccEmailId != null && !ccEmailId.Equals(""))
        //                {
        //                    if (ccEmailId.Contains(";"))
        //                    {
        //                        string[] ccEmailIds = ccEmailId.Split(';');
        //                        foreach (string ccEmail in ccEmailIds)
        //                        {
        //                            if (ccEmail != null && !ccEmail.Equals(""))
        //                            {
        //                                message.CC.Add(new MailAddress(ccEmail));
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        message.CC.Add(new MailAddress(ccEmailId));
        //                    }
        //                }
        //                message.Subject = subject;
        //                message.Body = content;// +EmailHelper.SIGNATURE_TEXT;
        //                message.IsBodyHtml = true;

        //                if (attachmentsList != null)
        //                {
        //                    int attachmentsListSize = attachmentsList.Count;
        //                    IEnumerator iEnumAttachments = attachmentsList.Keys.GetEnumerator();
        //                    for (int index = 0; index < attachmentsList.Count; index++)
        //                    {
        //                        iEnumAttachments.MoveNext();
        //                        string attachmentName = iEnumAttachments.Current.ToString();
        //                        string attachmentPath = (string)attachmentsList[attachmentName];
        //                        FileStream fs = new FileStream(attachmentPath, FileMode.Open, FileAccess.Read, FileShare.Read, 256, FileOptions.None);
        //                        message.Attachments.Add(new Attachment(fs, attachmentName));
        //                    }
        //                }
        //                SmtpClient client = new SmtpClient();
        //                client.Host = SIMConstants.EMAIL_HOST;
        //                client.Port = SIMConstants.EMAIL_PORT;

        //                if (SIMConstants.EMAIL_NEEDS_AUTHENTICATION)
        //                {
        //                    client.UseDefaultCredentials = false;
        //                    client.Credentials = new System.Net.NetworkCredential(SIMConstants.EMAIL_USER_ID, SIMConstants.EMAIL_PASSWORD);
        //                }
        //                else
        //                {
        //                    Logger.error("Emailer:SendMail():EMAIL_NEEDS_AUTHENTICATION flag is false ");
        //                }

        //                client.EnableSsl = SIMConstants.EMAIL_SSLON;
        //                client.Send(message);
        //                Logger.info("Emailer:SendMail():Exiting the method ");

        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.fatal("Emailer:SendMail(): Exception in sending mail to " + emailId + " : " + ex.Message);
        //                iStatus = 0;
        //            }
        //            return iStatus;
        //        }
        #endregion
        delegate void sendMailDelegate(string to, string bcc, string cc, string subject, string body, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "");
        /// <summary>
        /// Send Mail using SMTP server.
        /// </summary>
        /// <param name="to">Multiple mail ids separated by ; </param>
        /// <param name="bcc">Multiple mail ids separated by ; </param>
        /// <param name="cc">Multiple mail ids separated by ; </param>
        /// <param name="subject"></param>
        /// <param name="body">HTML mail content</param>
        public static void SendMailMessageUsingSmtp(string to, string bcc, string cc, string subject, string body, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "")
        {
            Logger.Info("Emailer:SendMailMessageUsingSmtp():Entering method ");
            try
            {

                if (attachmentPath == string.Empty)
                {
                    sendMailDelegate sendMailDelegate;
                    sendMailDelegate = new sendMailDelegate(sendMailthroughSMTP);
                    sendMailDelegate.BeginInvoke(to, bcc, cc, subject, body, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath, attachmentPath, attachmentName, null, null);
                }
                else
                {
                    sendMailthroughSMTP(to, bcc, cc, subject, body, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath, attachmentPath, attachmentName);
                }
            }
            catch
            {
                Logger.Error("");
                throw;
            }
            Logger.Info("Emailer:SendMailMessageUsingSmtp():Exiting method ");
        }

        static void sendMailthroughSMTP(string to, string bcc, string cc, string subject, string body, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "")
        {
            //For Uat
            if ((ConfigurationManager.AppSettings["Environment"] != null) && (ConfigurationManager.AppSettings["Environment"].ToUpper() == "UAT"))
            {
                to = "paul.fagg@prgx.com";
                bcc = string.Empty;
                cc = string.Empty;
            }
            //For Developement
            else if ((ConfigurationManager.AppSettings["Environment"] != null) && (ConfigurationManager.AppSettings["Environment"].ToUpper() == "DEV"))
            {
                to = "shubham.raj@raremile.com";
                bcc = string.Empty;
                cc = string.Empty;
            }

            try
            {
                FileStream fileStream = null;
                string fromMail = Constants.FROM_EMAIL_ID;

                // Instantiate a new instance of MailMessage
                MailMessage mMailMessage = new MailMessage();

                // Set the sender address of the mail message
                mMailMessage.From = new MailAddress(fromMail);
                //if ((ConfigurationManager.AppSettings["DeployementLocation"] != null) && (ConfigurationManager.AppSettings["DeployementLocation"].ToUpper().Equals("PRODUCTION")))
                //{
                //    //Add from mail as BCC for tracking purpose.
                //    //mMailMessage.Bcc.Add(new MailAddress(SIMConstants.ADMIN_EMAIL_ID));
                //}
                // Set the recipient address of the mail message
                if (!string.IsNullOrEmpty(to))
                    mMailMessage.To.Add(to);

                // set CC address
                if (!string.IsNullOrEmpty(cc))
                    mMailMessage.CC.Add(cc);


                ////Set the subject of the mail message
                mMailMessage.Subject = subject;

                string commonEmailTemplate = CommonMethods.GetEmailTemplate();
                commonEmailTemplate = string.Format(commonEmailTemplate, body);
                String tmp = @"<style>
                                    @font-face
                                    {
                                        font-family: Aptifer Sans LT;
                                        src: url(cid:email_font_Aptifer) format('truetype');
                                    }
                                    </style>";
                commonEmailTemplate = tmp + commonEmailTemplate;
                // If there is any emailLogoURL then attach the image
                // otherwise call the other method to embedd the image.
                //if (string.IsNullOrWhiteSpace(emailSIMLogoUrl) && (File.Exists(emailSIMLogoUrl) == false))
                //{

                //    //// Set the body of the mail message
                //    mMailMessage.Body = body;
                //    // Set the format of the mail message body as HTML
                //    mMailMessage.IsBodyHtml = true ;
                //}
                //else
                //{
                // embbed image-url
                EmbbedImageIntoMailMessage(mMailMessage, commonEmailTemplate, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath);

                if (string.IsNullOrWhiteSpace(attachmentPath) == false)
                {
                    try
                    {
                        fileStream = new FileStream(@attachmentPath, FileMode.Open, FileAccess.Read, FileShare.Read, 256, FileOptions.None);
                        mMailMessage.Attachments.Add(new Attachment(fileStream, attachmentName));
                        Logger.Debug("Emailer:SendMail(): Attached the pdf in the mail");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Emailer:SendMail():Exception while attaching the pdf file" + ex);
                    }
                }
                //}

                // Set the priority of the mail message to normal
                mMailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                //
                SmtpClient mSmtpClient = GetSMTPClient();

                // Send the mail message
                mSmtpClient.Send(mMailMessage);

                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Emailer:SendMailMessageUsingSmtp():Caught Exception." + ex + " IN SENDING MAIL" + subject);
                throw;
            }
        }


        public static bool SendMail(string to, string bcc, string cc, string subject, string body, string emailSIMLogoUrl, string emilPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "")
        {
            var response = false;
            //For Uat
            if ((ConfigurationManager.AppSettings["Environment"] != null) && (ConfigurationManager.AppSettings["Environment"].ToUpper() == "UAT"))
            {
                to = "paul.fagg@prgx.com";
                bcc = string.Empty;
                cc = string.Empty;
            }
            //For Developement
            else if ((ConfigurationManager.AppSettings["Environment"] != null) && (ConfigurationManager.AppSettings["Environment"].ToUpper() == "DEV"))
            {
                to = "shubham.raj@raremile.com";
                bcc = string.Empty;
                cc = string.Empty;
            }

            try
            {
                FileStream fileStream = null;
                string fromMail = Constants.FROM_EMAIL_ID;

                // Instantiate a new instance of MailMessage
                MailMessage mMailMessage = new MailMessage();

                // Set the sender address of the mail message
                mMailMessage.From = new MailAddress(fromMail);
                // Set the recipient address of the mail message
                if (!string.IsNullOrEmpty(to))
                    mMailMessage.To.Add(to);

                // set CC address
                if (!string.IsNullOrEmpty(cc))
                    mMailMessage.CC.Add(cc);


                ////Set the subject of the mail message
                mMailMessage.Subject = subject;

                string commonEmailTemplate = CommonMethods.GetEmailTemplate();
                commonEmailTemplate = string.Format(commonEmailTemplate, body);
                String tmp = @"<style>
                                    @font-face
                                    {
                                        font-family: Aptifer Sans LT;
                                        src: url(cid:email_font_Aptifer) format('truetype');
                                    }
                                    </style>";
                commonEmailTemplate = tmp + commonEmailTemplate;
                // If there is any emailLogoURL then attach the image
                // otherwise call the other method to embedd the image.                
                // embbed image-url
                EmbbedImageIntoMailMessage(mMailMessage, commonEmailTemplate, emailSIMLogoUrl, emilPRGXLogoUrl, emailCheckedLogoUrlFullPath, emailUncheckedLogoUrlFullPath, appFilePhysicalPath);

                if (string.IsNullOrWhiteSpace(attachmentPath) == false)
                {
                    try
                    {
                        fileStream = new FileStream(@attachmentPath, FileMode.Open, FileAccess.Read, FileShare.Read, 256, FileOptions.None);
                        mMailMessage.Attachments.Add(new Attachment(fileStream, attachmentName));
                        Logger.Debug("Emailer:SendMail(): Attached the pdf in the mail");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Emailer:SendMail():Exception while attaching the pdf file" + ex);
                    }
                }

                // Set the priority of the mail message to normal
                mMailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                SmtpClient mSmtpClient = GetSMTPClient();

                // Send the mail message
                mSmtpClient.Send(mMailMessage);

                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }


                response = true;
            }
            catch (Exception ex)
            {
                Logger.Error("Emailer:SendMailMessageUsingSmtp():Caught Exception." + ex + " IN SENDING MAIL" + subject);
                response = false;
                //throw;
            }
            return response;

        }

        public static bool SendReportsMail(string to, string bcc, string cc, string subject, string body, string directory, string companyName, string emailSIMLogoUrl, string emailPRGXLogoUrl, string emailCheckedLogoUrlFullPath, string emailUncheckedLogoUrlFullPath, string appFilePhysicalPath, string attachmentPath = "", string attachmentName = "")
        {
            var response = false;
            //For Uat
            if ((ConfigurationManager.AppSettings["Environment"] != null) && (ConfigurationManager.AppSettings["Environment"].ToUpper() == "UAT"))
            {
                to = "paul.fagg@prgx.com";
                bcc = string.Empty;
                cc = string.Empty;
            }
            //For Developement
            else if ((ConfigurationManager.AppSettings["Environment"] != null) && (ConfigurationManager.AppSettings["Environment"].ToUpper() == "DEV"))
            {
                to = "shubham.raj@raremile.com";
                bcc = string.Empty;
                cc = string.Empty;
            }

            try
            {
                FileStream fileStream = null;
                string fromMail = Constants.FROM_EMAIL_ID;

                // Instantiate a new instance of MailMessage
                MailMessage mMailMessage = new MailMessage();

                // Set the sender address of the mail message
                mMailMessage.From = new MailAddress(fromMail);
                // Set the recipient address of the mail message
                if (!string.IsNullOrEmpty(to))
                    mMailMessage.To.Add(to);

                // set CC address
                if (!string.IsNullOrEmpty(cc))
                    mMailMessage.CC.Add(cc);


                ////Set the subject of the mail message
                mMailMessage.Subject = subject + " - " + companyName;

                string commonEmailTemplate = CommonMethods.GetEmailTemplate();
                commonEmailTemplate = string.Format(commonEmailTemplate, body);
                String tmp = @"<style>
                                    @font-face
                                    {
                                        font-family: Aptifer Sans LT;
                                        src: url(cid:email_font_Aptifer) format('truetype');
                                    }
                                    </style>";
                commonEmailTemplate = tmp + commonEmailTemplate;

                if (string.IsNullOrWhiteSpace(attachmentPath) == false)
                {
                    try
                    {
                        fileStream = new FileStream(@attachmentPath, FileMode.Open, FileAccess.Read, FileShare.Read, 256, FileOptions.None);
                        mMailMessage.Attachments.Add(new Attachment(fileStream, attachmentName));
                        Logger.Debug("Emailer:SendMail(): Attached the pdf in the mail");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Emailer:SendMail():Exception while attaching the pdf file" + ex);
                    }
                }

                // Set the priority of the mail message to normal
                mMailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                SmtpClient mSmtpClient = GetSMTPClient();


                AlternateView reportsView = AlternateView.CreateAlternateViewFromString(commonEmailTemplate, null, "text/html");

                //Add Image

                LinkedResource ProfileImage = new LinkedResource(directory + "\\Profile.png");
                ProfileImage.ContentId = "ProfileImage";

                LinkedResource FITImage = new LinkedResource(directory + "\\FinanceInsuranceTax.png");
                FITImage.ContentId = "FITImage";

                LinkedResource HSImage = new LinkedResource(directory + "\\HealthSafety.png");
                HSImage.ContentId = "HSImage";

                LinkedResource DSImage = new LinkedResource(directory + "\\DataSecurity.png");
                DSImage.ContentId = "DSImage";

                LinkedResource logoPRGX = new LinkedResource(emailPRGXLogoUrl);
                logoPRGX.ContentId = "email_prgx_logo";

                LinkedResource SimlogoPRGX = new LinkedResource(emailSIMLogoUrl);
                SimlogoPRGX.ContentId = "email_SIM_logo";

                //Add the Image to the Alternate view
                reportsView.LinkedResources.Add(ProfileImage);
                reportsView.LinkedResources.Add(FITImage);
                reportsView.LinkedResources.Add(DSImage);
                reportsView.LinkedResources.Add(HSImage);
                reportsView.LinkedResources.Add(logoPRGX);
                reportsView.LinkedResources.Add(SimlogoPRGX);

                //Add view to the Email Message
                mMailMessage.AlternateViews.Add(reportsView);
                // Send the mail message
                mSmtpClient.Send(mMailMessage);

                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
                ProfileImage.Dispose();
                FITImage.Dispose();
                DSImage.Dispose();
                HSImage.Dispose();
                Directory.Delete(directory, true);

                response = true;
            }
            catch (Exception ex)
            {
                Logger.Error("Emailer:SendMailMessageUsingSmtp():Caught Exception." + ex + " IN SENDING MAIL" + subject);
                response = false;
                //throw;
            }
            return response;

        }
    }
}


