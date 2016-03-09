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
    public class AddressRepositoryTest
    {
        #region AddUpdateAddressTest
       // [TestCaseSource("AddUpdateAddress")]
        public void AddUpdateAddressTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            var partyAddress = new Address();
            partyAddress = x.partyAddress;

            su.BeginTransaction();
            su.Addresses.AddUpdateAddress(partyAddress, x.contactMethodId);
            su.SaveChanges();
            var data = su.Addresses.GetAll().Where(c => c.RefContactMethod == x.contactMethodId).ToList();
            su.Commit();
            Assert.That(data[0].Line1, Is.EqualTo(x.partyAddress.Line1));
            Assert.That(data[0].State, Is.EqualTo(x.partyAddress.State));

        }
      

        public static IEnumerable AddUpdateAddress()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\AddressRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("AddUpdateAddressTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                Address partyAddress = new Address();
                var contactMethodId = Convert.ToInt64(inputNode.Attributes["contactMethodId"].Value == "" ? 0L : Convert.ToInt64(inputNode.Attributes["contactMethodId"].Value));
                partyAddress.Line1 = Convert.ToString(inputNode.Attributes["Line1"].Value == "" ? "" : Convert.ToString(inputNode.Attributes["Line1"].Value));
                partyAddress.Line2 = Convert.ToString(inputNode.Attributes["Line2"].Value == "" ? "" : Convert.ToString(inputNode.Attributes["Line2"].Value));
                partyAddress.Line3 = Convert.ToString(inputNode.Attributes["Line3"].Value == "" ? "" : Convert.ToString(inputNode.Attributes["Line3"].Value));
                partyAddress.Zone = Convert.ToString(inputNode.Attributes["Zone"].Value == "" ? "" : Convert.ToString(inputNode.Attributes["Zone"].Value));
                partyAddress.City = Convert.ToString(inputNode.Attributes["City"].Value == "" ? "" : Convert.ToString(inputNode.Attributes["City"].Value));
                partyAddress.State = Convert.ToString(inputNode.Attributes["State"].Value == "" ? "" : Convert.ToString(inputNode.Attributes["State"].Value));
                partyAddress.RefCountryId = Convert.ToInt64(inputNode.Attributes["RefCountryId"].Value == "" ? 0L : Convert.ToInt64(inputNode.Attributes["RefCountryId"].Value));
                partyAddress.ZipCode = Convert.ToString(inputNode.Attributes["ZipCode"].Value == "" ? "" : Convert.ToString(inputNode.Attributes["ZipCode"].Value));
                partyAddress.AddressType = Convert.ToInt16(inputNode.Attributes["AddressType"].Value == "" ? 0 : Convert.ToInt16(inputNode.Attributes["AddressType"].Value));
                partyAddress.RefContactMethod = Convert.ToInt64(inputNode.Attributes["RefContactMethod"].Value == "" ? 0L : Convert.ToInt64(inputNode.Attributes["RefContactMethod"].Value));

                var forAddUpdateAddressTestCase = new
                {
                    contactMethodId,
                    partyAddress

                };

                returnValue.Add(forAddUpdateAddressTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetAddressDetailsByPartyIdTest
        // [TestCaseSource("GetAddressDetailsByPartyId")]
        public void GetAddressDetailsByPartyIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            var PartyId = x.partyId;
            List<Address> result = su.Addresses.GetAddressDetailsByPartyId(PartyId);

            List<Address> outResult = x.addressDetails;

            Assert.That(result.Select(i => i.Id), Is.EqualTo(outResult.Select(i => i.Id)));
            Assert.That(result.Count, Is.EqualTo(x.total));
        }

        public static IEnumerable GetAddressDetailsByPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\AddressRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetAddressDetailsByPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var partyId = Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim()));
                List<Address> addressDetails = new List<Address>();
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new Address();
                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    addressDetails.Add(tempData);
                }


                var forGetAddressDetailsByPartyIdTestCase = new
                {
                    partyId,
                    addressDetails,
                    total

                };

                returnValue.Add(forGetAddressDetailsByPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetAddressDetailsByContactMethodIdTest
        // [TestCaseSource("GetAddressDetailsByContactMethodId")]
        public void GetAddressDetailsByContactMethodIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            var contactMethodID = x.contactMethodID;
            Address result = su.Addresses.GetAddressDetailsByContactMethodId(contactMethodID);
            if (result != null)
            {
                Assert.That(result.Id, Is.EqualTo(x.Id));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }


        }

        public static IEnumerable GetAddressDetailsByContactMethodId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\AddressRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetAddressDetailsByContactMethodIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var contactMethodID = Convert.ToInt64(inputNode.Attributes["contactMethodID"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["contactMethodID"].Value.Trim()));

                var Id = Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["Id"].Value.Trim()));




                var forGetAddressDetailsByContactMethodIdTestCase = new
                {
                    contactMethodID,
                    Id

                };

                returnValue.Add(forGetAddressDetailsByContactMethodIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region BuyerSupplierAddressListTest
        // [TestCaseSource("BuyerSupplierAddressList")]
        public void BuyerSupplierAddressListTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            int totalRecords = 0;
            List<BuyerSupplierAddressList> result = su.Addresses.BuyerSupplierAddressList(x.pageNo, x.sortParameter, x.sortDirection, x.buyerName, x.sellerPartyId, x.addressId, out totalRecords);

            List<BuyerSupplierAddressList> outResult = x.addressDetails;

            Assert.That(result.Select(i => i.AddressId), Is.EqualTo(outResult.Select(i => i.AddressId)));
            Assert.That(result.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable BuyerSupplierAddressList()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\AddressRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("BuyerSupplierAddressListTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var pageNo = Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim()));
                var sortParameter = Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                var buyerName = Convert.ToString(inputNode.Attributes["buyerName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["buyerName"].Value.Trim()));
                var sellerPartyId = Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["sellerPartyId"].Value.Trim()));
                var addressId = Convert.ToInt64(inputNode.Attributes["addressId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["addressId"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<BuyerSupplierAddressList> addressDetails = new List<BuyerSupplierAddressList>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerSupplierAddressList();
                    tempData.AddressId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["addressId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["addressId"].Value.Trim()));
                    addressDetails.Add(tempData);
                }


                var forBuyerSupplierAddressListTestCase = new
                {
                    pageNo,
                    sortParameter,
                    sortDirection,
                    buyerName,
                    sellerPartyId,
                    addressId,
                    totalRecords,
                    addressDetails,
                    total

                };

                returnValue.Add(forBuyerSupplierAddressListTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
