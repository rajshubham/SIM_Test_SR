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
   public class OrganizationRepositoryTest
    {
        #region UpdateOrganizationDetailsTest
   //   [TestCaseSource("UpdateOrganizationDetails")]
        public void UpdateOrganizationDetailsTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            su.BeginTransaction();
            bool result = su.Organizations.UpdateOrganizationDetails(x.organization,x.partyId);
            su.SaveChanges();
            su.Commit();
            Assert.That(result, Is.EqualTo(x.outObj));

        }

        public static IEnumerable UpdateOrganizationDetails()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\OrganizationRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("UpdateOrganizationDetailsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var partyId = Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim()));

                var organization = new Organization();
               organization.BusinessSectorDescription= Convert.ToString(inputNode.Attributes["businessSectorDescription"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["businessSectorDescription"].Value.Trim()));
                organization.BusinessSectorId= Convert.ToInt64(inputNode.Attributes["businessSectorId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["businessSectorId"].Value.Trim()));
                organization.EmployeeSize= Convert.ToInt64(inputNode.Attributes["employeeSize"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["employeeSize"].Value.Trim()));
                organization.Status= Convert.ToInt16(inputNode.Attributes["status"].Value.Trim() == "" ? 0 : Convert.ToInt16(inputNode.Attributes["status"].Value.Trim()));
                organization.TurnOverSize= Convert.ToInt64(inputNode.Attributes["turnOverSize"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["turnOverSize"].Value.Trim()));

                var outObj = Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim())); 
                var forUpdateOrganizationDetailsTestCase = new
                {
                   partyId,
                   organization,
                   outObj

                };

                returnValue.Add(forUpdateOrganizationDetailsTestCase);
            }
            return returnValue;

        }
        #endregion

        #region UpdateOrganizationStatusTest
      // [TestCaseSource("UpdateOrganizationStatus")]
        public void UpdateOrganizationStatusTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            su.BeginTransaction();
            bool result = su.Organizations.UpdateOrganizationStatus(x.partyId,x.organizationStatus,x.userPatyId);
            su.SaveChanges();
            su.Commit();
            Assert.That(result, Is.EqualTo(x.outObj));

        }

        public static IEnumerable UpdateOrganizationStatus()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\OrganizationRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("UpdateOrganizationStatusTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var partyId = Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim()));
                var userPartyId = Convert.ToInt64(inputNode.Attributes["userPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["userPartyId"].Value.Trim()));
                var organizationStatus = Convert.ToInt16(inputNode.Attributes["organizationStatus"].Value.Trim() == "" ? 0 : Convert.ToInt16(inputNode.Attributes["organizationStatus"].Value.Trim()));
              
                var outObj = Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim()));
                var forUpdateOrganizationStatusTestCase = new
                {
                    partyId,
                    organizationStatus,
                    outObj

                };

                returnValue.Add(forUpdateOrganizationStatusTestCase);
            }
            return returnValue;

        }
        #endregion


        #region GetOrganizationDetailTest
        //  [TestCaseSource("GetOrganizationDetail")]
        public void GetOrganizationDetailTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
          
            OrganizationDetail result = su.Organizations.GetOrganizationDetail(x.organizationPartyId);
            if (result.OrganizationName != null)
            {
                Assert.That(result.OrganizationName, Is.EqualTo(x.organizationDetail.OrganizationName));
            }
else
            {
                Assert.That(result.OrganizationName, Is.EqualTo(null));
            }
        }

        public static IEnumerable GetOrganizationDetail()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\OrganizationRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetOrganizationDetailTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organizationPartyId = Convert.ToInt64(inputNode.Attributes["organizationPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["organizationPartyId"].Value.Trim()));
                var organizationDetail = new OrganizationDetail();
                organizationDetail.OrganizationName = Convert.ToString(outputNode.Attributes["orgName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["orgName"].Value.Trim()));
                var forGetOrganizationDetailTestCase = new
                {
                    organizationPartyId,
                    organizationDetail

                };

                returnValue.Add(forGetOrganizationDetailTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
