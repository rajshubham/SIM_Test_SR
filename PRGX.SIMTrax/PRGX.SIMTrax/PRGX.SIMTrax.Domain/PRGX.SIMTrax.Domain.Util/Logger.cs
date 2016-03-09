using log4net;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Threading.Tasks;
using System.Web;

namespace PRGX.SIMTrax.Domain.Util
{
    public class Logger
    {

        public static void XmlConfigure()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private static readonly ILog Log = LogManager.GetLogger("ApplicationLog");

        private static readonly ILog PFwebServicelogger = LogManager.GetLogger("PathFinderWebserviceLog");

        public static void Info(string logStatement)
        {
            Log.Info(logStatement);
        }

        public static void Error(string logStatement)
        {
            Log.Error(logStatement);
            var loginId = String.Empty;
            if(HttpContext.Current != null && HttpContext.Current.Session.Keys.Count > 0)
            {
                loginId  =  (HttpContext.Current.Session[Constants.SESSION_LOGIN_ID] != null) ?
                    (Convert.ToString(HttpContext.Current.Session[Constants.SESSION_LOGIN_ID])) : String.Empty;
            }
            Task t = new Task(() => {
                Email errorMail = new Email() { Content = logStatement,
                IsErrorMail = true,
                Environment = Configuration.Environment,
                Subject = " URGENT: iPartner : Error In " + Configuration.Environment,
                isBodyHtml = true,
                LoginId = loginId
                };
                Emailer.SendEmail(errorMail); });
            t.Start();
        }

        public static void Error(string logStatement, Exception ex)
        {
            Log.Error(logStatement, ex);
            var statusCode = 0;
            if (ex.GetType().FullName.ToLower() == "system.web.httpexception")
            {
                var httpException = (System.Web.HttpException)ex;
                 statusCode = httpException.GetHttpCode();
            }
            
            if (statusCode != 404)
            {
                Task t = new Task(() =>
                {
                    Email errorMail = new Email()
                    {
                        Content = logStatement + ex.ToString(),
                        IsErrorMail = true,
                        Environment = Configuration.Environment,
                        Subject = " URGENT: iPartner : Error In " + Configuration.Environment,
                        isBodyHtml = true
                    };
                    Emailer.SendEmail(errorMail);
                });
                t.Start();
            }
        }

        public static void Debug(string logStatement)
        {
            Log.Debug(logStatement);
        }
    }
}
