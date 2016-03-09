using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Abstract;
using System.Linq;
using PRGX.SIMTrax.Domain.Model;
using System.Data.Entity;
using System.Data.SqlClient;

namespace PRGX.SIMTrax.DAL.Test.Repository
{

    [TestFixture]
    public class BuyerCampaignRepositoryTest
    {

        #region GetCampaignInfoTest
        // [TestCaseSource("GetCampaignInfo")]
        public void GetCampaignInfoTest(object o)
        {

            dynamic x = o;
            ICampaignUow cu = null;
            cu = new CampaignUow();
          BuyerCampaign result = cu.BuyerCampaigns.GetCampaignInfo(x.campaignId,x.campaignUrl);
            if (result != null)
            {
                Assert.That(result.Id, Is.EqualTo(x.campaignDetails.Id));
                Assert.That(result.CampaignName, Is.EqualTo(x.campaignDetails.CampaignName));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }
        }

        public static IEnumerable GetCampaignInfo()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerCampaignRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetCampaignInfoTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var campaignId = Convert.ToInt64(inputNode.Attributes["campaignId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["campaignId"].Value.Trim()));
                var campaignUrl = Convert.ToString(inputNode.Attributes["campaignUrl"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["campaignUrl"].Value.Trim()));

                BuyerCampaign campaignDetails = new BuyerCampaign();

                campaignDetails.Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));
                campaignDetails.CampaignName= Convert.ToString(outputNode.Attributes["campaignName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["campaignName"].Value.Trim()));



                var forGetCampaignInfoTestCase = new
                {
                   campaignId,
                   campaignUrl,
                   campaignDetails

                };

                returnValue.Add(forGetCampaignInfoTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetAssignedCampaignsTest
        //[TestCaseSource("GetAssignedCampaigns")]
        public void GetAssignedCampaignsTest(object o)
        {

            dynamic x = o;
            ICampaignUow cu = null;
            cu = new CampaignUow();
            int total = x.totalRecords;
           List<BuyerCampaign> result = cu.BuyerCampaigns.GetAssignedCampaigns(x.auditorId,x.index,out total);
            List<BuyerCampaign> outResult = x.campaignDetails;
           
                Assert.That(result.Select(i=>i.Id), Is.EqualTo(outResult.Select(i=>i.Id)));
                Assert.That(result.Select(i=>i.CampaignName), Is.EqualTo(outResult.Select(i=>i.CampaignName)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetAssignedCampaigns()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerCampaignRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetAssignedCampaignsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var auditorId = Convert.ToInt64(inputNode.Attributes["auditorId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["auditorId"].Value.Trim()));
                var index = Convert.ToInt32(inputNode.Attributes["index"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["index"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["total"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));
                List<BuyerCampaign> campaignDetails = new List<BuyerCampaign>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerCampaign();
                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    tempData.CampaignName = Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim()));
                    campaignDetails.Add(tempData);
                }

                var forGetAssignedCampaignsTestCase = new
                {
                    auditorId,
                    index,
                    totalRecords,
                    total,
                    campaignDetails

                };

                returnValue.Add(forGetAssignedCampaignsTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetCampaignsAwaitingActionTest
       // [TestCaseSource("GetCampaignsAwaitingAction")]
        public void GetCampaignsAwaitingActionTest(object o)
        {

            dynamic x = o;
            ICampaignUow cu = null;
            cu = new CampaignUow();
            int total = x.totalRecords;
            List<BuyerCampaign> result = cu.BuyerCampaigns.GetCampaignsAwaitingAction( out total, x.pageNo, x.auditorId);
            List<BuyerCampaign> outResult = x.campaignDetails;

            Assert.That(result.Select(i => i.Id), Is.EqualTo(outResult.Select(i => i.Id)));
            Assert.That(result.Select(i => i.CampaignName), Is.EqualTo(outResult.Select(i => i.CampaignName)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetCampaignsAwaitingAction()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerCampaignRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetCampaignsAwaitingActionTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var auditorId = Convert.ToInt64(inputNode.Attributes["auditorId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["auditorId"].Value.Trim()));
                var pageNo = Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["total"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));
                List<BuyerCampaign> campaignDetails = new List<BuyerCampaign>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerCampaign();
                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    tempData.CampaignName = Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim()));
                    campaignDetails.Add(tempData);
                }

                var forGetCampaignsAwaitingActionTestCase = new
                {
                    auditorId,
                    pageNo,
                    totalRecords,
                    total,
                    campaignDetails

                };

                returnValue.Add(forGetCampaignsAwaitingActionTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSubmittedOrApprovedCampaignsTest
       // [TestCaseSource("GetSubmittedOrApprovedCampaigns")]
        public void GetSubmittedOrApprovedCampaignsTest(object o)
        {

            dynamic x = o;
            ICampaignUow cu = null;
            cu = new CampaignUow();
            int total = x.totalRecords;
            List<BuyerCampaign> result = cu.BuyerCampaigns.GetSubmittedOrApprovedCampaigns(x.campaignStatus, x.index,out total);
            List<BuyerCampaign> outResult = x.campaignDetails;

            Assert.That(result.Select(i => i.Id), Is.EqualTo(outResult.Select(i => i.Id)));
            Assert.That(result.Select(i => i.CampaignName), Is.EqualTo(outResult.Select(i => i.CampaignName)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetSubmittedOrApprovedCampaigns()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerCampaignRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSubmittedOrApprovedCampaignsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var campaignStatus = Convert.ToInt16(inputNode.Attributes["campaignStatus"].Value.Trim() == "" ? 0L : Convert.ToInt16(inputNode.Attributes["campaignStatus"].Value.Trim()));
                var index = Convert.ToInt32(inputNode.Attributes["index"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["index"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["total"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));
                List<BuyerCampaign> campaignDetails = new List<BuyerCampaign>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerCampaign();
                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    tempData.CampaignName = Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim()));
                    campaignDetails.Add(tempData);
                }

                var forGetSubmittedOrApprovedCampaignsTestCase = new
                {
                    campaignStatus,
                    index,
                    totalRecords,
                    total,
                    campaignDetails

                };

                returnValue.Add(forGetSubmittedOrApprovedCampaignsTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetSupplierReferrerBuyerCampaignDetailsTest
       // [TestCaseSource("GetSupplierReferrerBuyerCampaignDetails")]
        public void GetSupplierReferrerBuyerCampaignDetailsTest(object o)
        {

            dynamic x = o;
            ICampaignUow cu = null;
            cu = new CampaignUow();
            int total = x.totalRecords;
            List<Domain.Model.SupplierReferrer> result = cu.BuyerCampaigns.GetSupplierReferrerBuyerCampaignDetails(x.pageNo,x.buyerName,x.campaignName,x.supplierId, out total);
            List<Domain.Model.SupplierReferrer> outResult = x.supplierReferrerDetails;

            Assert.That(result.Select(i => i.CampaignId), Is.EqualTo(outResult.Select(i => i.CampaignId)));
            Assert.That(result.Select(i => i.CampaignName), Is.EqualTo(outResult.Select(i => i.CampaignName)));
            Assert.That(result.Select(i => i.BuyerOrganizationId), Is.EqualTo(outResult.Select(i => i.BuyerOrganizationId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetSupplierReferrerBuyerCampaignDetails()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BuyerCampaignRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetSupplierReferrerBuyerCampaignDetailsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var pageNo = Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim()));
                var buyerName = Convert.ToString(inputNode.Attributes["buyerName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["buyerName"].Value.Trim()));
                var campaignName = Convert.ToString(inputNode.Attributes["campaignName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["campaignName"].Value.Trim()));
                var supplierId = Convert.ToInt64(inputNode.Attributes["supplierId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["supplierId"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["total"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));
                List<Domain.Model.SupplierReferrer> supplierReferrerDetails = new List<Domain.Model.SupplierReferrer>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new Domain.Model.SupplierReferrer();
                    tempData.CampaignId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["campaignId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["campaignId"].Value.Trim()));
                    tempData.CampaignName = Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["campaignName"].Value.Trim()));
                    tempData.BuyerOrganizationId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerOrgId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerOrgId"].Value.Trim()));

                    supplierReferrerDetails.Add(tempData);
                }

                var forGetSupplierReferrerBuyerCampaignDetailsTestCase = new
                {
                    pageNo,
                    buyerName,
                    campaignName,
                    supplierId,
                    totalRecords,
                    total,
                    supplierReferrerDetails

                };

                returnValue.Add(forGetSupplierReferrerBuyerCampaignDetailsTestCase);
            }
            return returnValue;

        }
        #endregion

    }
}
