using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Util
{
    public static class CommonValidations
    {
        /// <summary>
        /// This method validate the email address
        /// </summary>
        /// <param name="strEmail">input text to be validate against email-id</param>
        /// <returns>true/false based on match</returns>

        public static bool IsEmailValid(string strEmail)
        {
            string pattern = @"^([\w\.\-]+)@([\w\-\.]+)((\.(\w){2,})+)$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(strEmail);
            
            return (match.Success);
        }
    }
}
