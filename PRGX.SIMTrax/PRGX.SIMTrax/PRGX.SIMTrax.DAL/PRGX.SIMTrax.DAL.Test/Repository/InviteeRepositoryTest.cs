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
    public class InviteeRepositoryTest
    {
        #region GetInviteeDetailsBySellerIdTest
      //  [TestCaseSource("GetInviteeDetailsBySellerId")]
        public void GetInviteeDetailsBySellerIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            List<Invitee> result = su.Invitees.GetInviteeDetailsBySellerId(x.sellerId);
            List<Invitee> outResult = x.inviteeList;
 
            Assert.That(result.Select(i=>i.Id), Is.EqualTo(outResult.Select(i=>i.Id)));
            Assert.That(result.Count, Is.EqualTo(x.total));
        }

        public static IEnumerable GetInviteeDetailsBySellerId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\InviteeRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetInviteeDetailsBySellerIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var sellerId = Convert.ToInt64(inputNode.Attributes["sellerId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["sellerId"].Value.Trim()));
                List<Invitee> inviteeList = new List<Invitee>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new Invitee();
                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0 : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    inviteeList.Add(tempData);
                }

                 var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));



                var forGetInviteeDetailsBySellerIdTestCase = new
                {
                    inviteeList,
                    sellerId,
                    total
                };

                returnValue.Add(forGetInviteeDetailsBySellerIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region BuyerSupplierReferenceListTest
          //[TestCaseSource("BuyerSupplierReferenceList")]
        public void BuyerSupplierReferenceListTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            int totalRecords = x.totalRecords;
            List<BuyerSupplierReferenceList> result = su.Invitees.BuyerSupplierReferenceList(x.pageNo, x.sortParameter, x.sortDirection, x.buyerName, x.sellerId, x.referenceId, out totalRecords);

            List<BuyerSupplierReferenceList> outResult = x.referenceDetails;

            Assert.That(result.Select(i => i.BuyerId), Is.EqualTo(outResult.Select(i => i.BuyerId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(totalRecords, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable BuyerSupplierReferenceList()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\InviteeRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("BuyerSupplierReferenceListTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                Address partyAddress = new Address();
                var pageNo = Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim()));
                var sortParameter = Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                var buyerName = Convert.ToString(inputNode.Attributes["buyerName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["buyerName"].Value.Trim()));
                var sellerId = Convert.ToInt64(inputNode.Attributes["sellerId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["sellerId"].Value.Trim()));
                var referenceId = Convert.ToInt64(inputNode.Attributes["referenceId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["referenceId"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<BuyerSupplierReferenceList> referenceDetails = new List<BuyerSupplierReferenceList>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerSupplierReferenceList();
                    tempData.BuyerId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerId"].Value.Trim()));
                    referenceDetails.Add(tempData);
                }


                var forBuyerSupplierReferenceListTestCase = new
                {
                    pageNo,
                    sortParameter,
                    sortDirection,
                    buyerName,
                    sellerId,
                    referenceId,
                    totalRecords,
                    referenceDetails,
                    total

                };

                returnValue.Add(forBuyerSupplierReferenceListTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
