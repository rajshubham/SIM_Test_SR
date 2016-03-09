using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
   public  interface IEmailTemplateRepository:IGenericRepository<EmailTemplate>
    {
         EmailTemplate GetEmailTemplate(string mnemonic, long refLocaleId = 0);
    }
}
