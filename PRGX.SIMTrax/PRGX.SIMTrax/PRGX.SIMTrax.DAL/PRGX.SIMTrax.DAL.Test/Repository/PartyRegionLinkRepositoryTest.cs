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
 public class PartyRegionLinkRepositoryTest
    {
        #region AddUpdatePartyRegionsTest
     //   [TestCaseSource("AddUpdatePartyRegions")]
        public void AddUpdatePartyRegionsTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            su.BeginTransaction();
          bool result=  su.PartyRegionLinks.AddUpdatePartyRegions(x.partyRegionLinkList,x.partyId);
            su.SaveChanges();
            su.Commit();

            Assert.That(result, Is.EqualTo(x.outObj));

        }

        public static IEnumerable AddUpdatePartyRegions()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyRegionLinkRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("AddUpdatePartyRegionsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

               var partyId = Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["partyId"].Value.Trim()));
                var outObj = Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim()));

                List<PartyRegionLink> partyRegionLinkList = new List<PartyRegionLink>();

                for (int j = 0; j < inputNode.ChildNodes.Count; j++)
                {
                    var tempData = new PartyRegionLink();
                 //   tempData.Id = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                   tempData.RefParty = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refParty"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refParty"].Value.Trim()));
                    tempData.RefRegion = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refRegion"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refRegion"].Value.Trim()));
                    tempData.LinkType = Convert.ToString(inputNode.ChildNodes[j].Attributes["linkType"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["linkType"].Value.Trim()));

                    partyRegionLinkList.Add(tempData);
                }

                var forAddUpdatePartyRegionsTestCase = new
                {
                    partyRegionLinkList,
                    partyId,
                    outObj
                };

                returnValue.Add(forAddUpdatePartyRegionsTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
