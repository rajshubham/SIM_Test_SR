using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Abstract;
using System.Linq;
using PRGX.SIMTrax.Domain.Model;

namespace PRGX.SIMTrax.DAL.Test.Repository
{
    [TestFixture]
    public class EmailTemplateRepositoryTest
    {
        #region GetEmailTemplateTest
        // [TestCaseSource("GetEmailTemplate")]
        public void GetEmailTemplateTest(object o)
        {

            dynamic x = o;
            IEmailUow u = null;
            u = new EmailUow();

            EmailTemplate result = u.EmailTemplates.GetEmailTemplate(x.mnemonic,x.refLocaleId);
            if (result != null)
            {
                Assert.That(result.Id, Is.EqualTo(x.emailTemplateId.Id));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetEmailTemplate()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\EmailTemplateRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetEmailTemplateTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var refLocaleId = Convert.ToInt64(inputNode.Attributes["refLocaleId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["refLocaleId"].Value.Trim()));
                var mnemonic = Convert.ToString(inputNode.Attributes["mnemonic"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["mnemonic"].Value.Trim()));
                var emailTemplateId = new EmailTemplate();
               emailTemplateId.Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));



                var forGetEmailTemplateTestCase = new
                {
                   refLocaleId,
                   mnemonic,
                   emailTemplateId
                };

                returnValue.Add(forGetEmailTemplateTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
