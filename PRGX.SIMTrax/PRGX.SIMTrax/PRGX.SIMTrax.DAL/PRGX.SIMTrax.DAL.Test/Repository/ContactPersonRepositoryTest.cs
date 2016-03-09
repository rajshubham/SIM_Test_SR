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
    public class ContactPersonRepositoryTest
    {

        #region GetContactDetailsByPartyIdTest
       //  [TestCaseSource("GetContactDetailsByPartyId")]
        public void GetContactDetailsByPartyIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();


            List<ContactPerson> result = su.ContactPersons.GetContactDetailsByPartyId(x.sellerPartyId);

            List<ContactPerson> outResult = x.contactPerson;

            Assert.That(result.Select(i => i.Id), Is.EqualTo(outResult.Select(i => i.Id)));
            Assert.That(result.Count, Is.EqualTo(x.total));
        }

        public static IEnumerable GetContactDetailsByPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\ContactPersonRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetContactDetailsByPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var sellerPartyId = Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim()));

                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));
                List<ContactPerson> contactPerson = new List<ContactPerson>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new ContactPerson();
                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    contactPerson.Add(tempData);
                }

                var forGetContactDetailsByPartyIdTestCase = new
                {
                    sellerPartyId,
                    contactPerson,
                    total

                };

                returnValue.Add(forGetContactDetailsByPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetContactByRoleAndPartyIdTest
     //  [TestCaseSource("GetContactByRoleAndPartyId")]
        public void GetContactByRoleAndPartyIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();


            ContactPerson result = su.ContactPersons.GetContactByRoleAndPartyId(x.sellerPartyId, x.contactType);

            if (result != null)
            {

                Assert.That(result.Id, Is.EqualTo(x.contactPerson.Id));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetContactByRoleAndPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\ContactPersonRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetContactByRoleAndPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var sellerPartyId = Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim()));
                var contactType = Convert.ToInt32(inputNode.Attributes["contactType"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["contactType"].Value.Trim()));

                var contactPerson = new ContactPerson();
                contactPerson.Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));



                var forGetContactByRoleAndPartyIdTestCase = new
                {
                    sellerPartyId,
                    contactType,
                    contactPerson


                };

                returnValue.Add(forGetContactByRoleAndPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region BuyerSupplierContactListTest
         // [TestCaseSource("BuyerSupplierContactList")]
        public void BuyerSupplierContactListTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            int totalRecords = x.totalRecords;
            List<BuyerSupplierContacts> result = su.ContactPersons.BuyerSupplierContactList(x.pageNo, x.sortParameter, x.sortDirection, x.buyerName, x.sellerId, x.contactPartyId, out totalRecords);

            List<BuyerSupplierContacts> outResult = x.addressDetails;

            Assert.That(result.Select(i => i.BuyerId), Is.EqualTo(outResult.Select(i => i.BuyerId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(totalRecords, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable BuyerSupplierContactList()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\ContactPersonRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("BuyerSupplierContactListTestData")[0].ChildNodes;
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
                var contactPartyId = Convert.ToInt64(inputNode.Attributes["contactPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["contactPartyId"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<BuyerSupplierContacts> addressDetails = new List<BuyerSupplierContacts>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerSupplierContacts();
                    tempData.BuyerId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["buyerId"].Value.Trim()));
                    addressDetails.Add(tempData);
                }


                var forBuyerSupplierContactListTestCase = new
                {
                    pageNo,
                    sortParameter,
                    sortDirection,
                    buyerName,
                    sellerId,
                    contactPartyId,
                    totalRecords,
                    addressDetails,
                    total

                };

                returnValue.Add(forBuyerSupplierContactListTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
