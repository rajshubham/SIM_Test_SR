using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Abstract;
using System.Linq;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;

namespace PRGX.SIMTrax.DAL.Test.Repository
{
    [TestFixture]
    public class BuyerRepositoryTest
    {
        #region GetBuyerOrganizationTest
      //[TestCaseSource("GetBuyerOrganization")]
        public void GetBuyerOrganizationTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();

            int totalRecords = x.totalRecords;
            List<BuyerOrganization> result = bu.Buyers.GetBuyerOrganization(x.status, x.buyerRole, x.fromDate, x.toDate, out totalRecords, x.pageIndex, x.pageSize, x.sortDirection, x.sortParameter, x.buyerName);

            List<BuyerOrganization> outResult = x.buyerDetails;

            Assert.That(result.Select(i => i.BuyerId), Is.EqualTo(outResult.Select(i => i.BuyerId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(totalRecords, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetBuyerOrganization()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetBuyerOrganizationTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var status = Convert.ToInt32(inputNode.Attributes["status"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["status"].Value.Trim()));
                var sortParameter = Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                var buyerRole = Convert.ToInt64(inputNode.Attributes["buyerRole"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["buyerRole"].Value.Trim()));
                var buyerName = Convert.ToString(inputNode.Attributes["buyerName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["buyerName"].Value.Trim()));
                var pageIndex = Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var pageSize = Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim()));
                var fromDate = Convert.ToString(inputNode.Attributes["fromDate"].Value.Trim() == "" ? "1/1/2001" : Convert.ToString(inputNode.Attributes["fromDate"].Value.Trim()));
                var toDate = Convert.ToString(inputNode.Attributes["toDate"].Value.Trim() == "" ? " " :Convert.ToString(inputNode.Attributes["toDate"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<BuyerOrganization> buyerDetails = new List<BuyerOrganization>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerOrganization();
                    tempData.BuyerId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerId"].Value.Trim()));
                    buyerDetails.Add(tempData);
                }


                var forGetBuyerOrganizationTestCase = new
                {
                    status,
                    sortParameter,
                    sortDirection,
                    buyerRole,
                    buyerName,
                    pageIndex,
                    totalRecords,
                    pageSize,
                    fromDate,
                    toDate,
                    buyerDetails,
                    total

                };

                returnValue.Add(forGetBuyerOrganizationTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetBuyerOrganizationDetailsByPartyIdTest
       // [TestCaseSource("GetBuyerOrganizationDetailsByPartyId")]
        public void GetBuyerOrganizationDetailsByPartyIdTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();

          
            Buyer result = bu.Buyers.GetBuyerOrganizationDetailsByPartyId(x.organizationPartyId);

            Buyer outResult = x.buyer;
            if (result != null)
            {
                Assert.That(result.Id, Is.EqualTo(outResult.Id));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetBuyerOrganizationDetailsByPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetBuyerOrganizationDetailsByPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var organizationPartyId = Convert.ToInt64(inputNode.Attributes["organisationPartyId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["organisationPartyId"].Value.Trim()));
             
             
                    var buyer = new Buyer();
                    buyer.Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));
          


                var forGetBuyerOrganizationDetailsByPartyIdTestCase = new
                {
                    organizationPartyId,
                    buyer

                };

                returnValue.Add(forGetBuyerOrganizationDetailsByPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetBuyerPrimaryContactPartyIdTest
        //[TestCaseSource("GetBuyerPrimaryContactPartyId")]
        public void GetBuyerPrimaryContactPartyIdTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();


            long result = bu.Buyers.GetBuyerPrimaryContactPartyId(x.buyerPartyId);

            long outResult =x.buyerContactPartyId;


            Assert.That(result, Is.EqualTo(outResult));

        }

        public static IEnumerable GetBuyerPrimaryContactPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetBuyerPrimaryContactPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var buyerPartyId = Convert.ToInt64(inputNode.Attributes["buyerPartyId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["buyerPartyId"].Value.Trim()));
              var  buyerContactPartyId = Convert.ToInt64(outputNode.Attributes["buyerContactPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["buyerContactPartyId"].Value.Trim()));



                var forGetBuyerPrimaryContactPartyIdTestCase = new
                {
                    buyerPartyId,
                    buyerContactPartyId

                };

                returnValue.Add(forGetBuyerPrimaryContactPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetNotActivatedBuyerOrganizationTest
       // [TestCaseSource("GetNotActivatedBuyerOrganization")]
        public void GetNotActivatedBuyerOrganizationTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();

            int totalRecords = x.totalRecords;
            List<BuyerOrganization> result = bu.Buyers.GetNotActivatedBuyerOrganization(x.pageIndex,x.pageSize, x.sortDirection, x.sortParameter, out totalRecords);

            List<BuyerOrganization> outResult = x.buyerDetails;

            Assert.That(result.Select(i => i.BuyerId), Is.EqualTo(outResult.Select(i => i.BuyerId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(totalRecords, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetNotActivatedBuyerOrganization()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetNotActivatedBuyerOrganizationTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                Address partyAddress = new Address();
                var pageIndex = Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim()));
                var pageSize = Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim()));
                var sortParameter = Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
               var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<BuyerOrganization> buyerDetails = new List<BuyerOrganization>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerOrganization();
                    tempData.BuyerId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerId"].Value.Trim()));
                    buyerDetails.Add(tempData);
                }


                var forGetNotActivatedBuyerOrganizationTestCase = new
                {
                    pageIndex,
                    pageSize,
                    sortParameter,
                    sortDirection,
                    totalRecords,
                    buyerDetails,
                    total

                };

                returnValue.Add(forGetNotActivatedBuyerOrganizationTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSuppliersTest
         //[TestCaseSource("GetSuppliers")]
        public void GetSuppliersTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();

            int totalRecords = x.totalRecords;
            List<SupplierDetails> result = bu.Buyers.GetSuppliers(x.model,x.companyPartyId,x.userPartyId,out totalRecords);

            List<SupplierDetails> outResult = x.buyerDetails;

            Assert.That(result.Select(i => i.CompanyId), Is.EqualTo(outResult.Select(i => i.CompanyId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(totalRecords, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetSuppliers()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSuppliersTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                BuyerSupplierSearchFilter model = new BuyerSupplierSearchFilter();
                var companyPartyId = Convert.ToInt32(inputNode.Attributes["companyPartyId"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["companyPartyId"].Value.Trim()));
                var userPartyId = Convert.ToInt32(inputNode.Attributes["userPartyId"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["userPartyId"].Value.Trim()));
                model.SupplierName = Convert.ToString(inputNode.Attributes["supplierName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["supplierName"].Value.Trim()));

                model.SortParameter = Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim()));
                model.SortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                model.PageNo = Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim()));
                model.PageSize = Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<SupplierDetails> buyerDetails = new List<SupplierDetails>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new SupplierDetails();
                    tempData.CompanyId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["companyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["companyId"].Value.Trim()));
                    buyerDetails.Add(tempData);
                }


                var forGetSuppliersTestCase = new
                {
                    model,
                    companyPartyId,
                    userPartyId,
                    totalRecords,
                    buyerDetails,
                    total

                };

                returnValue.Add(forGetSuppliersTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetVerifiedBuyerNamesTest
        // [TestCaseSource("GetVerifiedBuyerNames")]
        public void GetVerifiedBuyerNamesTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();

           
            List<string> result = bu.Buyers.GetVerifiedBuyerNames(x.buyerOrg);

            List<string> outResult = x.verifiedBuyers;

            Assert.That(result, Is.EqualTo(outResult));
            Assert.That(result.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable GetVerifiedBuyerNames()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetVerifiedBuyerNamesTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var buyerOrg = Convert.ToString(inputNode.Attributes["buyerOrg"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["buyerOrg"].Value.Trim()));

                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<string> verifiedBuyers = new List<string>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = Convert.ToString(outputNode.ChildNodes[j].Attributes["buyerName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["buyerName"].Value.Trim()));
                    verifiedBuyers.Add(tempData);
                }


                var forGetVerifiedBuyerNamesTestCase = new
                {
                    buyerOrg,
                    verifiedBuyers,
                    total

                };

                returnValue.Add(forGetVerifiedBuyerNamesTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetBuyerDetailsForDashboardTest
        // [TestCaseSource("GetBuyerDetailsForDashboard")]
        public void GetBuyerDetailsForDashboardTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();


          BuyerOrganization result = bu.Buyers.GetBuyerDetailsForDashboard(x.partyId);
          if (result != null)
          {
              Assert.That(result.BuyerId, Is.EqualTo(x.buyerId));
          }
          else
          {
              Assert.That(result, Is.EqualTo(null));
          }
        }

        public static IEnumerable GetBuyerDetailsForDashboard()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetBuyerDetailsForDashboardTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var partyId = Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim()));

              
                    var buyerId = Convert.ToInt64(outputNode.Attributes["buyerId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["buyerId"].Value.Trim()));
            


                var forGetBuyerDetailsForDashboardTestCase = new
                {
                   partyId,
                   buyerId
                 

                };

                returnValue.Add(forGetBuyerDetailsForDashboardTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetBuyersListTest
       // [TestCaseSource("GetBuyersList")]
        public void GetBuyersListTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();


            List<ItemList> result = bu.Buyers.GetBuyersList();

            List<ItemList> outResult = x.buyersList;

            Assert.That(result.Select(i=>i.Value), Is.EqualTo(outResult.Select(i=>i.Value)));
            Assert.That(result.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable GetBuyersList()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetBuyersListTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
               
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<ItemList> buyersList = new List<ItemList>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new ItemList();
                     tempData.Value = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["value"].Value.Trim() == "" ? 0L: Convert.ToInt64(outputNode.ChildNodes[j].Attributes["value"].Value.Trim()));
                    buyersList.Add(tempData);
                }


                var forGetBuyersListTestCase = new
                {
                 
                  buyersList,
                    total

                };

                returnValue.Add(forGetBuyersListTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetAllVouchersTest
       //  [TestCaseSource("GetAllVouchers")]
        public void GetAllVouchersTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();

            int total = x.totalRecords;
            List<DiscountVoucher> result = bu.Buyers.GetAllVouchers(x.currentPage, x.sortParameter, x.sortDirection, out total,x.count,x.buyerPartyId);

            List<DiscountVoucher> outResult = x.voucherDetails;

            Assert.That(result.Select(i => i.Id), Is.EqualTo(outResult.Select(i => i.Id)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

         public static IEnumerable GetAllVouchers()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetAllVouchersTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var buyerPartyId = Convert.ToInt64(inputNode.Attributes["buyerPartyId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["buyerPartyId"].Value.Trim()));
               
                var currentPage = Convert.ToInt32(inputNode.Attributes["currentPage"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["currentPage"].Value.Trim()));
                var count = Convert.ToInt32(inputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["count"].Value.Trim()));
                var sortParameter = Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<DiscountVoucher> voucherDetails = new List<DiscountVoucher>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new DiscountVoucher();
                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    voucherDetails.Add(tempData);
                }


                var forGetAllVouchersTestCase = new
                {buyerPartyId,
                    currentPage,
                    count,
                    sortParameter,
                    sortDirection,
                    totalRecords,
                    voucherDetails,
                    total

                };

                returnValue.Add(forGetAllVouchersTestCase);
            }
            return returnValue;

        }
        #endregion


        #region GetBuyerCampaignDetailsForDashboardTest
     //  [TestCaseSource("GetBuyerCampaignDetailsForDashboard")]
        public void GetBuyerCampaignDetailsForDashboardTest(object o)
        {

            dynamic x = o;
            IBuyerUow bu = null;
            bu = new BuyerUow();

            int totalCampaigns = x.totalCampaigns;
            List<BuyerCampaign> result = bu.Buyers.GetBuyerCampaignDetailsForDashboard(out totalCampaigns,x.partyId,x.pageNumber,x.sortDirection);

            List<BuyerCampaign> outResult = x.campaignDetails;

            Assert.That(result.Select(i => i.Id), Is.EqualTo(outResult.Select(i => i.Id)));
            Assert.That(result.Select(i => i.CampaignName), Is.EqualTo(outResult.Select(i => i.CampaignName)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(totalCampaigns, Is.EqualTo(x.totalCampaigns));

        }

        public static IEnumerable GetBuyerCampaignDetailsForDashboard()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetBuyerCampaignDetailsForDashboardTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var partyId = Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim()));
                var pageNumber = Convert.ToInt32(inputNode.Attributes["pageNumber"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNumber"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                var totalCampaigns = Convert.ToInt32(inputNode.Attributes["totalCampaigns"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalCampaigns"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<BuyerCampaign> campaignDetails = new List<BuyerCampaign>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerCampaign();
                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    tempData.CampaignName= Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim() == "" ? "": Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim()));
                    campaignDetails.Add(tempData);
                }


                var forGetBuyerCampaignDetailsForDashboardTestCase = new
                {
                   partyId,
                   pageNumber,
                   sortDirection,
                   totalCampaigns,
                   total,
                   campaignDetails

                };

                returnValue.Add(forGetBuyerCampaignDetailsForDashboardTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
