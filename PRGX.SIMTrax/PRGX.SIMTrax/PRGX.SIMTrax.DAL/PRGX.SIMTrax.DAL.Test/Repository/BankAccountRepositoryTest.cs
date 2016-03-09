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
    public class BankAccountRepositoryTest
    {

        #region GetBankDetailsByOrganisationIdTest
       // [TestCaseSource("GetBankDetailsByOrganisationId")]
        public void GetBankDetailsByOrganisationIdTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();


            List<BankAccount> result = su.BankAccounts.GetBankDetailsByOrganisationId(x.organisationId);

            List<BankAccount> outResult = x.accountDetails;

            Assert.That(result.Select(i => i.AccountName.Trim()), Is.EqualTo(outResult.Select(i => i.AccountName.Trim())));
            Assert.That(result.Count, Is.EqualTo(x.total));
        }

        public static IEnumerable GetBankDetailsByOrganisationId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BankAccountRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetBankDetailsByOrganisationIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organisationId = Convert.ToInt64(inputNode.Attributes["organisationId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["organisationId"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<BankAccount> accountDetails = new List<BankAccount>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BankAccount();
                    tempData.AccountName = Convert.ToString(outputNode.ChildNodes[j].Attributes["accountName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["accountName"].Value.Trim()));
                    accountDetails.Add(tempData);
                }
                var forGetBankDetailsByOrganisationIdTestCase = new
                {
                    organisationId,
                    accountDetails,
                    total

                };

                returnValue.Add(forGetBankDetailsByOrganisationIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region BuyerSupplierBankListTest
        //  [TestCaseSource("BuyerSupplierBankList")]
        public void BuyerSupplierBankListTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();

            int totalRecords = x.totalRecords;
            List<BuyerSupplierBankAccount> result = su.BankAccounts.BuyerSupplierBankList(x.pageNo, x.sortParameter, x.sortDirection, x.buyerName, x.organisationId, x.bankId, out totalRecords);

            List<BuyerSupplierBankAccount> outResult = x.addressDetails;

            Assert.That(result.Select(i => i.BankId), Is.EqualTo(outResult.Select(i => i.BankId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(totalRecords, Is.EqualTo(x.totalRecords));

        }

        public static IEnumerable BuyerSupplierBankList()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\BankAccountRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("BuyerSupplierBankListTestData")[0].ChildNodes;
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
                var organisationId = Convert.ToInt64(inputNode.Attributes["organisationId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["organisationId"].Value.Trim()));
                var bankId = Convert.ToInt64(inputNode.Attributes["bankId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["bankId"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["total"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["total"].Value.Trim()));

                List<BuyerSupplierBankAccount> addressDetails = new List<BuyerSupplierBankAccount>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new BuyerSupplierBankAccount();
                    tempData.BankId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["bankId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["bankId"].Value.Trim()));
                    addressDetails.Add(tempData);
                }

                 
                var forBuyerSupplierBankListTestCase = new
                {
                    pageNo,
                    sortParameter,
                    sortDirection,
                    buyerName,
                    organisationId,
                    bankId,
                    totalRecords,
                    addressDetails,
                    total

                };

                returnValue.Add(forBuyerSupplierBankListTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
