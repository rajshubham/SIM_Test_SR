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
 public class PartyRepositoryTest
    {
        #region IsOrganisationExistsTest
     //  [TestCaseSource("IsOrganisationExists")]
        public void IsOrganisationExistsTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
 
            bool result = su.Parties.IsOrganisationExists(x.organisationName);
        

            Assert.That(result, Is.EqualTo(x.outObj));

        }

        public static IEnumerable IsOrganisationExists()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("IsOrganisationExistsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organisationName = Convert.ToString(inputNode.Attributes["orgName"].Value.Trim() == "" ? "": Convert.ToString(inputNode.Attributes["orgName"].Value.Trim()));
                var outObj = Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim()));

                var forIsOrganisationExistsTestCase = new
                {
                   organisationName,
                    outObj
                };

                returnValue.Add(forIsOrganisationExistsTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetCompanyDetailsByPartyIdTest
        // [TestCaseSource("GetCompanyDetailsByPartyId")]
        public void GetCompanyDetailsByPartyIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            Party result = su.Parties.GetCompanyDetailsByPartyId(x.organizationPartyId);

            if (result != null)
            {
                Assert.That(result.Id, Is.EqualTo(x.companyDetails.Id));
                Assert.That(result.PartyName, Is.EqualTo(x.companyDetails.PartyName));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetCompanyDetailsByPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetCompanyDetailsByPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organizationPartyId = Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim()));

                var companyDetails = new Party();

                companyDetails.Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));
             companyDetails.PartyName = Convert.ToString(outputNode.Attributes["partyName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["partyName"].Value.Trim()));

                var forGetCompanyDetailsByPartyIdTestCase = new
                {
                    organizationPartyId,
                    companyDetails
                };

                returnValue.Add(forGetCompanyDetailsByPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetCapabilityDetailsByPartyIdTest
         // [TestCaseSource("GetCapabilityDetailsByPartyId")]
        public void GetCapabilityDetailsByPartyIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            Party result = su.Parties.GetCapabilityDetailsByPartyId(x.organizationPartyId);
            if (result != null)
            {
                Assert.That(result.Id, Is.EqualTo(x.companyDetails.Id));
                Assert.That(result.PartyName, Is.EqualTo(x.companyDetails.PartyName));
            }
              else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetCapabilityDetailsByPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetCapabilityDetailsByPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organizationPartyId = Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim()));

                var companyDetails = new Party();

                companyDetails.Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));
                companyDetails.PartyName = Convert.ToString(outputNode.Attributes["partyName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["partyName"].Value.Trim()));

                var forGetCapabilityDetailsByPartyIdTestCase = new
                {
                    organizationPartyId,
                    companyDetails
                };

                returnValue.Add(forGetCapabilityDetailsByPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetMarketingDetailsByPartyIdTest
      //  [TestCaseSource("GetMarketingDetailsByPartyId")]
        public void GetMarketingDetailsByPartyIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            Party result = su.Parties.GetMarketingDetailsByPartyId(x.organizationPartyId);
            if (result != null)
            {
                Assert.That(result.Id, Is.EqualTo(x.companyDetails.Id));
                Assert.That(result.PartyName, Is.EqualTo(x.companyDetails.PartyName));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetMarketingDetailsByPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetMarketingDetailsByPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organizationPartyId = Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim()));

                var companyDetails = new Party();

                companyDetails.Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));
                companyDetails.PartyName = Convert.ToString(outputNode.Attributes["partyName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["partyName"].Value.Trim()));

                var forGetMarketingDetailsByPartyIdTestCase = new
                {
                    organizationPartyId,
                    companyDetails
                };

                returnValue.Add(forGetMarketingDetailsByPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetIndustryCodesByOrganisationPartyIdTest
        // [TestCaseSource("GetIndustryCodesByOrganisationPartyId")]
        public void GetIndustryCodesByOrganisationPartyIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            List<long> result = su.Parties.GetIndustryCodesByOrganisationPartyId(x.sellerPartyId);
            List<long> outResult = x.industryCode;
            Assert.That(result, Is.EqualTo(outResult));
            Assert.That(result.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable GetIndustryCodesByOrganisationPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetIndustryCodesByOrganisationPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var sellerPartyId = Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<long> industryCode = new List<long>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                  var tempData= Convert.ToInt64(outputNode.ChildNodes[j].Attributes["industryCode"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["industryCode"].Value.Trim()));
                    industryCode.Add(tempData);
                }
                var forGetIndustryCodesByOrganisationPartyIdTestCase = new
                {
                    sellerPartyId,
                 industryCode,
                 total
                };

                returnValue.Add(forGetIndustryCodesByOrganisationPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSellerOrganizationDetailsByPartyIdTest
       //  [TestCaseSource("GetSellerOrganizationDetailsByPartyId")]
        public void GetSellerOrganizationDetailsByPartyIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            Party result = su.Parties.GetSellerOrganizationDetailsByPartyId(x.organizationPartyId);
            if (result != null)
            {
                Assert.That(result.Id, Is.EqualTo(x.companyDetails.Id));
                Assert.That(result.PartyName, Is.EqualTo(x.companyDetails.PartyName));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetSellerOrganizationDetailsByPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSellerOrganizationDetailsByPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organizationPartyId = Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim()));

                var companyDetails = new Party();

                companyDetails.Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));
                companyDetails.PartyName = Convert.ToString(outputNode.Attributes["partyName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["partyName"].Value.Trim()));

                var forGetSellerOrganizationDetailsByPartyIdTestCase = new
                {
                    organizationPartyId,
                    companyDetails
                };

                returnValue.Add(forGetSellerOrganizationDetailsByPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region SellerProfileDetailsTest
        // [TestCaseSource("SellerProfileDetails")]
        public void SellerProfileDetailsTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            Profile result = su.Parties.SellerProfileDetails(x.sellerPartyId,x.buyerPartyId,x.buyerUserPartyId);

            Assert.That(result.SellerPartyId, Is.EqualTo(x.profileDetails.SellerPartyId));
         

        }

        public static IEnumerable SellerProfileDetails()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("SellerProfileDetailsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var sellerPartyId = Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim()));
                var buyerPartyId = Convert.ToInt64(inputNode.Attributes["buyerPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["buyerPartyId"].Value.Trim()));
                var buyerUserPartyId = Convert.ToInt64(inputNode.Attributes["buyerUserPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["buyerUserPartyId"].Value.Trim()));

                var profileDetails = new Profile();

                profileDetails.SellerPartyId = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));
              
                var forSellerProfileDetailsTestCase = new
                {
                    sellerPartyId,
                    buyerPartyId,
                    buyerUserPartyId,
                    profileDetails
                };

                returnValue.Add(forSellerProfileDetailsTestCase);
            }
            return returnValue;

        }
        #endregion


    }
}
