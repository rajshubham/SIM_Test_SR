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
    public class SupplierRepositoryTest
    {
        #region UpdateSellerDetailsTest
      //  [TestCaseSource("UpdateSellerDetails")]
        public void UpdateSellerDetailsTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            su.BeginTransaction();
           bool result = su.Suppliers.UpdateSellerDetails(x.supplierDetails, x.partyId);
            su.SaveChanges();
            su.Commit();
           
            Assert.That(result, Is.EqualTo(x.outObj));
       

        }

        public static IEnumerable UpdateSellerDetails()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\SupplierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("UpdateSellerDetailsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var partyId = Convert.ToInt32(inputNode.Attributes["partyId"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["partyId"].Value.Trim()));
                var outObj = Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim()));

                var supplierDetails = new Supplier();

                supplierDetails.BusinessDescription = Convert.ToString(inputNode.Attributes["businessDescription"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["businessDescription"].Value.Trim()));
                supplierDetails.EstablishedYear = Convert.ToString(inputNode.Attributes["estabishedYear"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["estabishedYear"].Value.Trim()));
                supplierDetails.FacebookAccount = Convert.ToString(inputNode.Attributes["facebookAccount"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["facebookAccount"].Value.Trim()));
                supplierDetails.IsSubsidary = Convert.ToBoolean(inputNode.Attributes["isSubsidiary"].Value.Trim() == "" ? false : Convert.ToBoolean(inputNode.Attributes["isSubsidiary"].Value.Trim()));
                supplierDetails.LinkedInAccount = Convert.ToString(inputNode.Attributes["linkedInAccount"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["linkedInAccount"].Value.Trim()));
                supplierDetails.MaxContractValue = Convert.ToString(inputNode.Attributes["maxContractValue"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["maxContractValue"].Value.Trim()));
                supplierDetails.MinContractValue = Convert.ToString(inputNode.Attributes["minContractValue"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["minContractValue"].Value.Trim()));
                supplierDetails.RefCreatedBy = Convert.ToInt64(inputNode.Attributes["refCreatedBy"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["refCreatedBy"].Value.Trim()));
                supplierDetails.RefLastUpdatedBy = Convert.ToInt64(inputNode.Attributes["refLastUpdatedBy"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["refLastUpdatedBy"].Value.Trim()));
                supplierDetails.RegisteredCountry = Convert.ToInt16(inputNode.Attributes["regCountry"].Value.Trim() == "" ? 0 : Convert.ToInt16(inputNode.Attributes["regCountry"].Value.Trim()));
                supplierDetails.TradingName = Convert.ToString(inputNode.Attributes["tradingName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["tradingName"].Value.Trim()));
                supplierDetails.TwitterAccount = Convert.ToString(inputNode.Attributes["twitterAccount"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["twitterAccount"].Value.Trim()));
                supplierDetails.TypeOfSeller = Convert.ToInt32(inputNode.Attributes["typeOfSeller"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["typeOfSeller"].Value.Trim()));
                supplierDetails.UltimateParent = Convert.ToString(inputNode.Attributes["ultimateParent"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["ultimateParent"].Value.Trim()));
                supplierDetails.WebsiteLink = Convert.ToString(inputNode.Attributes["websiteLink"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["websiteLink"].Value.Trim()));

                var forUpdateSellerDetailsTestCase = new
                {
                    supplierDetails,
                    outObj,
                    partyId
                };

                returnValue.Add(forUpdateSellerDetailsTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSellerProfilePercentageTest
         // [TestCaseSource("GetSellerProfilePercentage")]
        public void GetSellerProfilePercentageTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
        
            ProfileSummary result = su.Suppliers.GetSellerProfilePercentage(x.sellerPartyId,x.sellerId,x.organisationId);
     

            Assert.That(result.TotalScore, Is.EqualTo(x.score));


        }

        public static IEnumerable GetSellerProfilePercentage()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\SupplierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSellerProfilePercentageTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var sellerPartyId = Convert.ToInt32(inputNode.Attributes["sellerPartyId"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sellerPartyId"].Value.Trim()));
                var sellerId = Convert.ToInt32(inputNode.Attributes["sellerId"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sellerId"].Value.Trim()));
                var organisationId = Convert.ToInt32(inputNode.Attributes["organisationId"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["organisationId"].Value.Trim()));
                var score = Convert.ToInt64(outputNode.Attributes["score"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["score"].Value.Trim()));

            
                var forGetSellerProfilePercentageTestCase = new
                {
                  sellerPartyId,
                  sellerId,
                  organisationId,
                  score
                };

                returnValue.Add(forGetSellerProfilePercentageTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSupplierOrganizationTest
       //  [TestCaseSource("GetSupplierOrganization")]
        public void GetSupplierOrganizationTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            int total = x.totalRecords;
            List<SupplierOrganization> result = su.Suppliers.GetSupplierOrganization(x.fromdate,x.toDate,out total,x.pageIndex,x.source,x.size,x.sortDirection,x.supplierId,x.supplierName,x.status,x.referrerName);
            List<SupplierOrganization> outResult = x.supplierList;

            Assert.That(result.Select(i=>i.SupplierUserId), Is.EqualTo(outResult.Select(i=>i.SupplierUserId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetSupplierOrganization()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\SupplierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSupplierOrganizationTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var fromdate = Convert.ToString(inputNode.Attributes["fromdate"].Value.Trim() == "" ? "": Convert.ToString(inputNode.Attributes["fromdate"].Value.Trim()));
                var toDate = Convert.ToString(inputNode.Attributes["toDate"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["toDate"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var pageIndex = Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim()));
                var source = Convert.ToInt16(inputNode.Attributes["source"].Value.Trim() == "" ? 0 : Convert.ToInt16(inputNode.Attributes["source"].Value.Trim()));
                var size = Convert.ToInt32(inputNode.Attributes["size"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["size"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                var supplierId = Convert.ToInt64(inputNode.Attributes["supplierId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["supplierId"].Value.Trim()));
                var supplierName = Convert.ToString(inputNode.Attributes["supplierName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["supplierName"].Value.Trim()));
                var status = Convert.ToInt64(inputNode.Attributes["status"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["status"].Value.Trim()));
                var referrerName = Convert.ToString(inputNode.Attributes["referrerName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["referrerName"].Value.Trim()));

                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<SupplierOrganization> supplierList = new List<SupplierOrganization>();

                for(int j=0;j<outputNode.ChildNodes.Count;j++)
                {
                    var tempData = new SupplierOrganization();
                    tempData.SupplierUserId= Convert.ToInt64(outputNode.ChildNodes[j].Attributes["supplierUserId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["supplierUserId"].Value.Trim()));
                    supplierList.Add(tempData);
                }


                var forGetSupplierOrganizationTestCase = new
                {fromdate,
                toDate,
                totalRecords,
                pageIndex,
                source,
                size,
                sortDirection,
                supplierId,
                supplierName,
                status,
                referrerName,
                supplierList,
                total
                 
                };

                returnValue.Add(forGetSupplierOrganizationTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSupplierDetailsForDashboardTest
         // [TestCaseSource("GetSupplierDetailsForDashboard")]
        public void GetSupplierDetailsForDashboardTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
          
           SupplierOrganization result = su.Suppliers.GetSupplierDetailsForDashboard(x.supplierPartyId);
           if (result != null)
           {
               Assert.That(result.SupplierUserId, Is.EqualTo(x.supplierUserId));
           }
           else
           {
               Assert.That(result, Is.EqualTo(null));
           }
        }

        public static IEnumerable GetSupplierDetailsForDashboard()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\SupplierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSupplierDetailsForDashboardTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var supplierPartyId = Convert.ToInt64(inputNode.Attributes["supplierPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["supplierPartyId"].Value.Trim()));     
                  var supplierUserId = Convert.ToInt64(outputNode.Attributes["supplierUserId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["supplierUserId"].Value.Trim()));
             
                var forGetSupplierDetailsForDashboardTestCase = new
                {
                    
                  supplierPartyId,
                  supplierUserId

                };

                returnValue.Add(forGetSupplierDetailsForDashboardTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetNotVerifiedSupplierNamesTest
      //   [TestCaseSource("GetNotVerifiedSupplierNames")]
        public void GetNotVerifiedSupplierNamesTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();


            List<string> result = su.Suppliers.GetNotVerifiedSupplierNames(x.supplierOrg);

            List<string> outResult = x.notVerifiedSuppliers;

            Assert.That(result, Is.EqualTo(outResult));
            Assert.That(result.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable GetNotVerifiedSupplierNames()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\SupplierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetNotVerifiedSupplierNamesTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var supplierOrg = Convert.ToString(inputNode.Attributes["supplierOrg"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["supplierOrg"].Value.Trim()));

                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<string> notVerifiedSuppliers = new List<string>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = Convert.ToString(outputNode.ChildNodes[j].Attributes["supplierName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["supplierName"].Value.Trim()));
                    notVerifiedSuppliers.Add(tempData);
                }


                var forGetNotVerifiedSupplierNamesTestCase = new
                {
                    supplierOrg,
                    notVerifiedSuppliers,
                    total

                };

                returnValue.Add(forGetNotVerifiedSupplierNamesTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSuppliersListForRegistrationTest
         //  [TestCaseSource("GetSuppliersListForRegistration")]
        public void GetSuppliersListForRegistrationTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();


            List<string> result = su.Suppliers.GetSuppliersListForRegistration(x.companyName);

            List<string> outResult = x.supplierList;

            Assert.That(result, Is.EqualTo(outResult));
            Assert.That(result.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable GetSuppliersListForRegistration()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\SupplierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSuppliersListForRegistrationTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var companyName = Convert.ToString(inputNode.Attributes["companyName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["companyName"].Value.Trim()));

                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<string> supplierList = new List<string>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = Convert.ToString(outputNode.ChildNodes[j].Attributes["supplierName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["supplierName"].Value.Trim()));
                    supplierList.Add(tempData);
                }


                var forGetSuppliersListForRegistrationTestCase = new
                {
                    companyName,
                    supplierList,
                    total

                };

                returnValue.Add(forGetSuppliersListForRegistrationTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSuppliersForVerificationTest
          //[TestCaseSource("GetSuppliersForVerification")]
        public void GetSuppliersForVerificationTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            int total = x.totalRecords;
            List<SupplierOrganization> result = su.Suppliers.GetSuppliersForVerification(x.pageNo,x.sortParameter, x.sortDirection, out total, x.sourceCheck,x.viewOptions, x.pageSize, x.referrerName);
            List<SupplierOrganization> outResult = x.supplierList;

            Assert.That(result.Select(i => i.SupplierUserId), Is.EqualTo(outResult.Select(i => i.SupplierUserId)));
            Assert.That(result.Select(i => i.SupplierId), Is.EqualTo(outResult.Select(i => i.SupplierId)));
            Assert.That(result.Select(i => i.SupplierOrganizationName), Is.EqualTo(outResult.Select(i => i.SupplierOrganizationName)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetSuppliersForVerification()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\SupplierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSuppliersForVerificationTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var pageNo = Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim()));
                var sourceCheck = Convert.ToInt32(inputNode.Attributes["sourceCheck"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sourceCheck"].Value.Trim()));
                var pageSize = Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim()));
                var viewOptions = Convert.ToInt32(inputNode.Attributes["viewOptions"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["viewOptions"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                var sortParameter = Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim()));
                var referrerName = Convert.ToString(inputNode.Attributes["referrerName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["referrerName"].Value.Trim()));

                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<SupplierOrganization> supplierList = new List<SupplierOrganization>();

                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new SupplierOrganization();
                    tempData.SupplierId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["supplierId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["supplierId"].Value.Trim()));
                    tempData.SupplierUserId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["supplierUserId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["supplierUserId"].Value.Trim()));
                    tempData.SupplierOrganizationName = Convert.ToString(outputNode.ChildNodes[j].Attributes["supplierOrgName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["supplierOrgName"].Value.Trim()));

                    supplierList.Add(tempData);
                }


                var forGetSuppliersForVerificationTestCase = new
                {
               
                    totalRecords,
                    pageNo,
                    sourceCheck,
                    pageSize,
                    sortDirection,
                    sortParameter,
                    viewOptions,
                    referrerName,
                    supplierList,
                    total

                };

                returnValue.Add(forGetSuppliersForVerificationTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSuppliersCountBasedOnStageTest
        [TestCaseSource("GetSuppliersCountBasedOnStage")]
        public void GetSuppliersCountBasedOnStageTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            
            List<SupplierCountBasedOnStage> result = su.Suppliers.GetSuppliersCountBasedOnStage( x.sourceCheck, x.viewOptions, x.referrerName);
            List<SupplierCountBasedOnStage> outResult = x.scoreList;
            Assert.That(result.Select(i => i.Stage), Is.EqualTo(outResult.Select(i => i.Stage)));
            Assert.That(result.Select(i => i.DetailsScore), Is.EqualTo(outResult.Select(i => i.DetailsScore)));
            Assert.That(result.Select(i => i.ProfileScore), Is.EqualTo(outResult.Select(i => i.ProfileScore)));
            Assert.That(result.Select(i => i.SanctionScore), Is.EqualTo(outResult.Select(i => i.SanctionScore)));
        }

        public static IEnumerable GetSuppliersCountBasedOnStage()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\SupplierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSuppliersCountBasedOnStageTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var sourceCheck = Convert.ToInt32(inputNode.Attributes["sourceCheck"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sourceCheck"].Value.Trim()));
                var viewOptions = Convert.ToInt32(inputNode.Attributes["viewOptions"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["viewOptions"].Value.Trim()));
                var referrerName = Convert.ToString(inputNode.Attributes["referrerName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["referrerName"].Value.Trim()));

              
                List<SupplierCountBasedOnStage> scoreList = new List<SupplierCountBasedOnStage>();

                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new SupplierCountBasedOnStage();
                    tempData.ProfileScore = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["profileScore"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["profileScore"].Value.Trim()));
                    tempData.DetailsScore = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["detailScore"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["detailScore"].Value.Trim()));
                    tempData.SanctionScore = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["sanctionScore"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["sanctionScore"].Value.Trim()));
                    tempData.Stage = Convert.ToString(outputNode.ChildNodes[j].Attributes["stage"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["stage"].Value.Trim()));

                    scoreList.Add(tempData);
                }


                var forGetSuppliersCountBasedOnStageTestCase = new
                {
                    sourceCheck,
                    viewOptions,
                    referrerName,
                    scoreList

                };

                returnValue.Add(forGetSuppliersCountBasedOnStageTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
