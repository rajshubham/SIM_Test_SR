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
    public class MasterDataRepositoryTest
    {
        #region GetMasterDataValuesForSellerRegistrationTest
       // [TestCaseSource("GetMasterDataValuesForSellerRegistration")]
        public void GetMasterDataValuesForSellerRegistrationTest(object o)
        {

            dynamic x = o;
            IMasterDataUow mu = null;
            mu = new MasterDataUow();
            SellerRegisterMasterData result = mu.MasterDataValues.GetMasterDataValuesForSellerRegistration(x.regionCode);

            Assert.That(result.RefRegionId, Is.EqualTo(x.masterDataValues.RefRegionId));

        }

        public static IEnumerable GetMasterDataValuesForSellerRegistration()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\MasterDataRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetMasterDataValuesForSellerRegistrationTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var regionCode = Convert.ToString(inputNode.Attributes["regionCode"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["regionCode"].Value.Trim()));

                var masterDataValues = new SellerRegisterMasterData();
                masterDataValues.RefRegionId = Convert.ToInt64(outputNode.Attributes["refRegionId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["refRegionId"].Value.Trim()));

                var forGetMasterDataValuesForSellerRegistrationTestCase = new
                {
                    regionCode,
                    masterDataValues
                };

                returnValue.Add(forGetMasterDataValuesForSellerRegistrationTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetMasterDataValuesForBuyerRegistrationTest
      //  [TestCaseSource("GetMasterDataValuesForBuyerRegistration")]
        public void GetMasterDataValuesForBuyerRegistrationTest(object o)
        {

            dynamic x = o;
            IMasterDataUow mu = null;
            mu = new MasterDataUow();
            BuyerRegisterMasterData result = mu.MasterDataValues.GetMasterDataValuesForBuyerRegistration(x.regionCode);

            Assert.That(result.EmployeesNumberList.Count, Is.EqualTo(x.employeeNumberList));

        }

        public static IEnumerable GetMasterDataValuesForBuyerRegistration()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\MasterDataRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetMasterDataValuesForBuyerRegistrationTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var regionCode = Convert.ToString(inputNode.Attributes["regionCode"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["regionCode"].Value.Trim()));              
             var employeeNumberList = Convert.ToInt64(outputNode.Attributes["employeeNumberList"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["employeeNumberList"].Value.Trim()));
               
                var forGetMasterDataValuesForBuyerRegistrationTestCase = new
                {
                    regionCode,
                    employeeNumberList
                };

                returnValue.Add(forGetMasterDataValuesForBuyerRegistrationTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetMasterValuesForSearchFiltersTest
       // [TestCaseSource("GetMasterValuesForSearchFilters")]
        public void GetMasterValuesForSearchFiltersTest(object o)
        {

            dynamic x = o;
            IMasterDataUow mu = null;
            mu = new MasterDataUow();
            BuyerSupplierSearchFilter result = mu.MasterDataValues.GetMasterValuesForSearchFilters(x.model);
            if (result.SectorText != null)
            {
                Assert.That(result.SectorText, Is.EqualTo(x.sectorText));
            }
            else
            {
                Assert.That(result.SectorText, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetMasterValuesForSearchFilters()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\MasterDataRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetMasterValuesForSearchFiltersTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                BuyerSupplierSearchFilter model = new BuyerSupplierSearchFilter();
                List<long> turnOver = new List<long>();
                for(int j=0;j<inputNode.ChildNodes.Count;j++)
                {
                    var tempData =Convert.ToInt64(inputNode.ChildNodes[j].Attributes["value"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["value"].Value.Trim()));
                    turnOver.Add(tempData);
                }
                model.Sector = turnOver;
                model.SortParameter = Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["sortParameter"].Value.Trim()));
                model.SortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));
                model.PageNo = Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNo"].Value.Trim()));
                model.PageSize = Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim()));
                 
                var sectorText = Convert.ToString(outputNode.Attributes["sectorText"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["sectorText"].Value.Trim()));

                var forGetMasterValuesForSearchFiltersTestCase = new
                {
                    model,
                    sectorText
                };

                returnValue.Add(forGetMasterValuesForSearchFiltersTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
