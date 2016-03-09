using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Abstract;
using System.Linq;
using PRGX.SIMTrax.Domain.Model;
using System.Data.SqlClient;
using System.Data.Entity;

namespace PRGX.SIMTrax.DAL.Test.Repository
{
    [TestFixture]
   public class PartyIdentifierRepositoryTest
    {
        #region AddUpdateRangeTest
        [TestCaseSource("AddUpdateRange")]
        public void AddUpdateRangeTest(object o)
        {

            dynamic x = o;
            ISupplierUow su = null;
            su = new SupplierUow();
            su.BeginTransaction();
             su.PartyIdentifers.AddUpdateRange(x.partyIdentifiers);
            su.SaveChanges();
            List<PartyIdentifier> outData = new List<PartyIdentifier>();
            //var outData = new List();
            for (int i = 0; i < x.partyIdentifiers.Count; i++)
            {
               
              var  data = su.PartyIdentifers.GetAll().Where(c => c.RefParty == x.partyIdentifiers[i].RefParty && c.RefPartyIdentifierType==x.partyIdentifiers[i].RefPartyIdentifierType);
                outData.AddRange(data);
            }
            su.Commit();

            for (int j = 0; j < outData.Count; j++)
            {
                Assert.That(outData[j].IdentifierNumber, Is.EqualTo(x.partyIdentifiers[j].IdentifierNumber));
            }

            
        }

        public static IEnumerable AddUpdateRange()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PartyIdentifierRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("AddUpdateRangeTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                List<PartyIdentifier> partyIdentifiers = new List<PartyIdentifier>();

                for (int j = 0; j < inputNode.ChildNodes.Count; j++)
                {
                    var tempData = new PartyIdentifier();
                   tempData.Id= Convert.ToInt64(inputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    tempData.RefParty= Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refParty"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refParty"].Value.Trim()));
                    tempData.RefRegion = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refRegion"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refRegion"].Value.Trim()));

                    tempData.RefPartyIdentifierType= Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refPartyIdentifierType"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["refPartyIdentifierType"].Value.Trim()));
                    tempData.IdentifierNumber = Convert.ToString(inputNode.ChildNodes[j].Attributes["identifierNumber"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["identifierNumber"].Value.Trim()));
                    partyIdentifiers.Add(tempData);
                }
                  var forAddUpdateRangeTestCase = new
                {
                  partyIdentifiers
                };

                returnValue.Add(forAddUpdateRangeTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
