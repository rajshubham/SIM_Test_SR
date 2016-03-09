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
using System.Data.SqlClient;

namespace PRGX.SIMTrax.DAL.Test.Repository
{
    [TestFixture]
    public class Class1
    {
        #region UpdateListOfPreRegSupplierTest
        //[TestCaseSource("UpdateListOfPreRegSupplier")]
        public void UpdateListOfPreRegSupplierTest(object o)
        {
            dynamic x = o;
            ICampaignUow cu = new CampaignUow();
            List<CampaignInvitation> campaignPreRegSupplierInputList = x.campaignPreRegSupplierInputList;
            bool outData = x.outData;

            var outresult = cu.CampaignInvitations.UpdateListOfPreRegSupplier(campaignPreRegSupplierInputList);

            Assert.That(outData, Is.EqualTo(outresult));
        }

        public static IEnumerable UpdateListOfPreRegSupplier()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\CampaignInvitationRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("UpdateListOfPreRegSupplierTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                //public bool UpdateListOfPreRegSupplier(List<CampaignInvitation> campaignPreRegSupplierList)

                List<CampaignInvitation> campaignPreRegSupplierInputList = new List<CampaignInvitation>();

                for (int j = 0; j < inputNode.ChildNodes.Count; j++)
                {
                    var tempData = new CampaignInvitation();
                    //tempData.Id = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    tempData.AddressLine1 = Convert.ToString(inputNode.ChildNodes[j].Attributes["AddressLine1"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["AddressLine1"].Value.Trim()));
                    tempData.AddressLine2 = Convert.ToString(inputNode.ChildNodes[j].Attributes["AddressLine2"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["AddressLine2"].Value.Trim()));
                    tempData.City = Convert.ToString(inputNode.ChildNodes[j].Attributes["City"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["City"].Value.Trim()));
                    tempData.CreatedOn = Convert.ToDateTime(inputNode.ChildNodes[j].Attributes["CreatedOn"].Value.Trim() == "" ? Convert.ToDateTime("1/1/2015") : Convert.ToDateTime(inputNode.ChildNodes[j].Attributes["CreatedOn"].Value.Trim()));

                    tempData.EmailAddress = Convert.ToString(inputNode.ChildNodes[j].Attributes["EmailAddress"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["EmailAddress"].Value.Trim()));
                    tempData.FirstName = Convert.ToString(inputNode.ChildNodes[j].Attributes["FirstName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["FirstName"].Value.Trim()));
                    tempData.Identifier1 = Convert.ToString(inputNode.ChildNodes[j].Attributes["Identifier1"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["Identifier1"].Value.Trim()));
                    tempData.Identifier2 = Convert.ToString(inputNode.ChildNodes[j].Attributes["Identifier2"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["Identifier2"].Value.Trim()));
                    tempData.Identifier3 = Convert.ToString(inputNode.ChildNodes[j].Attributes["Identifier3"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["Identifier3"].Value.Trim()));

                    tempData.IdentifierType1 = Convert.ToString(inputNode.ChildNodes[j].Attributes["IdentifierType1"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["IdentifierType1"].Value.Trim()));
                    tempData.IdentifierType2 = Convert.ToString(inputNode.ChildNodes[j].Attributes["IdentifierType2"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["IdentifierType2"].Value.Trim()));
                    tempData.IdentifierType3 = Convert.ToString(inputNode.ChildNodes[j].Attributes["IdentifierType3"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["IdentifierType3"].Value.Trim()));
                    tempData.InvalidComments = Convert.ToString(inputNode.ChildNodes[j].Attributes["InvalidComments"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["InvalidComments"].Value.Trim()));
                    tempData.IsRegistered = Convert.ToBoolean(inputNode.ChildNodes[j].Attributes["IsRegistered"].Value.Trim() == "" ? false : Convert.ToBoolean(inputNode.ChildNodes[j].Attributes["IsRegistered"].Value.Trim()));

                    tempData.IsSubsidary = Convert.ToBoolean(inputNode.ChildNodes[j].Attributes["IsSubsidary"].Value.Trim() == "" ? false : Convert.ToBoolean(inputNode.ChildNodes[j].Attributes["IsSubsidary"].Value.Trim()));
                    tempData.IsValid = Convert.ToBoolean(inputNode.ChildNodes[j].Attributes["IsValid"].Value.Trim() == "" ? false : Convert.ToBoolean(inputNode.ChildNodes[j].Attributes["IsValid"].Value.Trim()));
                    tempData.JobTitle = Convert.ToString(inputNode.ChildNodes[j].Attributes["JobTitle"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["JobTitle"].Value.Trim()));
                    tempData.LastName = Convert.ToString(inputNode.ChildNodes[j].Attributes["LastName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["LastName"].Value.Trim()));
                    tempData.LastUpdatedOn = Convert.ToDateTime(inputNode.ChildNodes[j].Attributes["LastUpdatedOn"].Value.Trim() == "" ? Convert.ToDateTime("1/1/2015") : Convert.ToDateTime(inputNode.ChildNodes[j].Attributes["LastUpdatedOn"].Value.Trim()));

                    tempData.RefCampaign = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefCampaign"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefCampaign"].Value.Trim()));
                    tempData.RefCountry = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefCountry"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefCountry"].Value.Trim()));
                    tempData.RefCreatedBy = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefCreatedBy"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefCreatedBy"].Value.Trim()));
                    tempData.RefLastUpdatedBy = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefLastUpdatedBy"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefLastUpdatedBy"].Value.Trim()));
                    tempData.RefSupplier = Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefSupplier"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.ChildNodes[j].Attributes["RefSupplier"].Value.Trim()));

                    tempData.RegistrationCode = Convert.ToString(inputNode.ChildNodes[j].Attributes["RegistrationCode"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["RegistrationCode"].Value.Trim()));
                    tempData.State = Convert.ToString(inputNode.ChildNodes[j].Attributes["State"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["State"].Value.Trim()));
                    tempData.SupplierCompanyName = Convert.ToString(inputNode.ChildNodes[j].Attributes["SupplierCompanyName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["SupplierCompanyName"].Value.Trim()));
                    tempData.Telephone = Convert.ToString(inputNode.ChildNodes[j].Attributes["Telephone"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["Telephone"].Value.Trim()));
                    tempData.UltimateParent = Convert.ToString(inputNode.ChildNodes[j].Attributes["UltimateParent"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["UltimateParent"].Value.Trim()));
                    tempData.ZipCode = Convert.ToString(inputNode.ChildNodes[j].Attributes["ZipCode"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.ChildNodes[j].Attributes["ZipCode"].Value.Trim()));


                    campaignPreRegSupplierInputList.Add(tempData);
                }


                bool outData = Convert.ToBoolean(outputNode.Attributes["outData"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outData"].Value.Trim()));

                var forUpdateListOfPreRegSupplierTestCase = new
                {
                    campaignPreRegSupplierInputList,
                    outData
                };

                returnValue.Add(forUpdateListOfPreRegSupplierTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
