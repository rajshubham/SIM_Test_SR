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
  public class IndustryCodeRepositoryTest
    {
        #region GetIndustryCodesTest
        // [TestCaseSource("GetIndustryCodes")]
        public void GetIndustryCodesTest(object o)
        {

            dynamic x = o;
            IMasterDataUow su = null;
            su = new MasterDataUow();
          
            List<IndustryCode> result = su.IndustryCodeValues.GetIndustryCodes(x.regionIdentifier,x.parentId,x.allSICCodes);
            List<IndustryCode> outResult = x.List;
            List<IndustryCode> industryCodeResult = result.Take(3).ToList();
          
            if (result != null)
            {
                Assert.That(result.Count, Is.EqualTo(x.total));
                Assert.That(industryCodeResult.Select(i => i.Id), Is.EqualTo(outResult.Select(i => i.Id)));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetIndustryCodes()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\IndustryCodeRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetIndustryCodesTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var regionIdentifier = Convert.ToString(inputNode.Attributes["regionIdentifier"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["regionIdentifier"].Value.Trim()));
                var parentId = Convert.ToInt32(inputNode.Attributes["parentId"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["parentId"].Value.Trim()));
                var allSICCodes = Convert.ToBoolean(inputNode.Attributes["allSICCodes"].Value.Trim() == "" ? false : Convert.ToBoolean(inputNode.Attributes["allSICCodes"].Value.Trim()));
                  var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<IndustryCode> List = new List<IndustryCode>();

                for (int j = 0; j < outputNode.ChildNodes.Count; j++)

                {
                    var tempData = new IndustryCode();

                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    List.Add(tempData);
                }
                var forGetIndustryCodesTestCase = new
                {
                   regionIdentifier,
                   parentId,
                   allSICCodes,
                    List,
                    total
                };

                returnValue.Add(forGetIndustryCodesTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
