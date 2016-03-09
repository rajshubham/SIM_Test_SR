using NUnit.Framework;
using System;
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
   public class IndustryCodeOrganizationLinksRepositoryTest
    {

        #region AddUpdateIndustryCodeOrganisationLinksTest
      //  [TestCaseSource("AddUpdateIndustryCodeOrganisationLinks")]
        public void AddUpdateIndustryCodeOrganisationLinksTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            su.BeginTransaction();
            bool result = su.IndustryCodeOrganizationLinks.AddUpdateIndustryCodeOrganisationLinks(x.industryCodeOrganisationLinks,x.organisationId);
            su.SaveChanges();
            su.Commit();
            Assert.That(result, Is.EqualTo(x.outObj));

        }

        public static IEnumerable AddUpdateIndustryCodeOrganisationLinks()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\IndustryCodeOrganizationLinksRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("AddUpdateIndustryCodeOrganisationLinksTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                List<IndustryCodeOrganizationLink> industryCodeOrganisationLinks = new List<IndustryCodeOrganizationLink>();

                for (int j = 0; j < inputNode.ChildNodes.Count; j++)
                {
                    var tempData = new IndustryCodeOrganizationLink();
                    tempData.RefOrganization = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refOrganisation"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refOrganisation"].Value.Trim()));                  
                    tempData.RefIndustryCode = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refIndustryCode"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refIndustryCode"].Value.Trim()));
                    industryCodeOrganisationLinks.Add(tempData);
                        }
                var organisationId = Convert.ToInt64(inputNode.Attributes["organisationId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["organisationId"].Value.Trim()));

                var outObj = Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim()));



                var forAddUpdateIndustryCodeOrganisationLinksTestCase = new
                {
                    industryCodeOrganisationLinks,
                    organisationId,
                    outObj
                };

                returnValue.Add(forAddUpdateIndustryCodeOrganisationLinksTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
