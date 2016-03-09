using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository
{
    public class EmailTemplateRepository : GenericRepository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(DbContext context) : base(context) { }

        public EmailTemplate GetEmailTemplate(string mnemonic, long refLocaleId = 0)
        {
            try
            {
                Logger.Info("EmailTemplateRepository : GetEmailTemplate() : Enter the method");
                EmailTemplate emailTemplate = null;
                if (refLocaleId == 0)
                {
                    emailTemplate = this.All().FirstOrDefault(v => v.Mnemonic.ToLower() == mnemonic.ToLower());
                }
                else
                {
                    emailTemplate = this.All().FirstOrDefault(v => v.Mnemonic.ToLower() == mnemonic.ToLower() && v.RefLocale == refLocaleId);
                }
                Logger.Info("EmailTemplateRepository : GetEmailTemplate() : Exit the method");
                return emailTemplate;
            }
            catch (Exception ex)
            {
                Logger.Error("EmailTemplateRepository : GetEmailTemplate() : Caught an exception" + ex);
                throw;
            }
        }
    }
}
