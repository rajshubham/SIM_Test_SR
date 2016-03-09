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
    public class CampaignInvitationRepositoryTest
    {
        #region InsertListOfPreRegSupplierTest
       // [TestCaseSource("InsertListOfPreRegSupplier")]
        public void InsertListOfPreRegSupplierTest(object o)
        {
            dynamic x = o;
            ICampaignUow cu = new CampaignUow();
            List<CampaignInvitation> campaignPreRegSupplierListInput = x.campaignPreRegSupplierList;
            bool outData = x.outData;

            //cu.BeginTransaction();
            var outresult = cu.CampaignInvitations.InsertListOfPreRegSupplier(campaignPreRegSupplierListInput);
            //cu.SaveChanges();
            //cu.Commit();

            Assert.That(outData, Is.EqualTo(outresult));
        }

        public static IEnumerable InsertListOfPreRegSupplier()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\CampaignInvitationRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("InsertListOfPreRegSupplierTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                //public bool InsertListOfPreRegSupplier(List<CampaignInvitation> campaignPreRegSupplierList)

                List<CampaignInvitation> campaignPreRegSupplierList = new List<CampaignInvitation>();

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


                    campaignPreRegSupplierList.Add(tempData);
                }
                bool outData= Convert.ToBoolean(outputNode.Attributes["outData"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outData"].Value.Trim()));

                var forInsertListOfPreRegSupplierTestCase = new
                {
                    campaignPreRegSupplierList,
                    outData
                };

                returnValue.Add(forInsertListOfPreRegSupplierTestCase);
            }
            return returnValue;

        }
        #endregion
        #region GetPreRegSupplierInCampaignTest
        //[TestCaseSource("GetPreRegSupplierInCampaign")]
        public void GetPreRegSupplierInCampaignTest(object o)
        {
            dynamic x = o;
            ICampaignUow cu = new CampaignUow();
            var campaignId = x.campaignId;
            var filterCriteria = x.filterCriteria;
            int total = x.total;
            var pageIndex = x.pageIndex;
            var size = x.size;
            List<CampaignInvitation> outData = x.campaignInvitaionOutObjList;
            int outTotal = x.outTotal;

            List<CampaignInvitation> outresult = cu.CampaignInvitations.GetPreRegSupplierInCampaign(campaignId, filterCriteria, out total, pageIndex, size);

            Assert.That(outData.Select(i => i.EmailAddress), Is.EqualTo(outresult.Select(i => i.EmailAddress)));
            Assert.That(outData.Select(i => i.RefCampaign), Is.EqualTo(outresult.Select(i => i.Id)));
            Assert.That(outTotal, Is.EqualTo(total));
        }

        public static IEnumerable GetPreRegSupplierInCampaign()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\CampaignInvitationRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetPreRegSupplierInCampaignTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                //public List<CampaignInvitation> GetPreRegSupplierInCampaign(int campaignId, string filterCriteria, out int total, int pageIndex, int size)

                int campaignId = Convert.ToInt32(inputNode.Attributes["campaignId"].Value.Trim() == "" ? 0L : Convert.ToInt32(inputNode.Attributes["campaignId"].Value.Trim()));
                string filterCriteria = Convert.ToString(inputNode.Attributes["filterCriteria"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["filterCriteria"].Value.Trim()));
                int total = Convert.ToInt32(inputNode.Attributes["total"].Value.Trim() == "" ? 0L : Convert.ToInt32(inputNode.Attributes["total"].Value.Trim()));
                int pageIndex = Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim() == "" ? 0L : Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim()));
                int size = Convert.ToInt32(inputNode.Attributes["size"].Value.Trim() == "" ? 0L : Convert.ToInt32(inputNode.Attributes["size"].Value.Trim()));

                List<CampaignInvitation> campaignInvitaionOutObjList = new List<CampaignInvitation>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    CampaignInvitation tempData = new CampaignInvitation();
                    tempData.RefCampaign = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["RefCampaign"].Value.Trim() == "" ? 0 : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["RefCampaign"].Value.Trim()));
                    tempData.EmailAddress = Convert.ToString(outputNode.ChildNodes[j].Attributes["EmailAddress"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["EmailAddress"].Value.Trim()));
                    campaignInvitaionOutObjList.Add(tempData);

                }
                int outTotal = Convert.ToInt32(outputNode.Attributes["outTotal"].Value.Trim() == "" ? 0L : Convert.ToInt32(outputNode.Attributes["outTotal"].Value.Trim()));

                var forGetPreRegSupplierInCampaignTestCase = new
                {
                    campaignId,
                    filterCriteria,
                    total,
                    pageIndex,
                    size,
                    campaignInvitaionOutObjList,
                    outTotal
                };

                returnValue.Add(forGetPreRegSupplierInCampaignTestCase);
            }
            return returnValue;

        }
        #endregion
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
        #region GetPreRegSupplierListwithPasswordStringTest
        //[TestCaseSource("GetPreRegSupplierListwithPasswordString")]
        public void GetPreRegSupplierListwithPasswordStringTest(object o)
        {
            dynamic x = o;
            ICampaignUow cu = new CampaignUow();
            var campaignId = x.campaignId;
            List<CampaignInvitation> outData = x.campaignInvitaionOutObjList;

            List<CampaignInvitation> outresult = cu.CampaignInvitations.GetPreRegSupplierListwithPasswordString(campaignId);

            Assert.That(outData.Select(i => i.EmailAddress), Is.EqualTo(outresult.Select(i => i.EmailAddress)));
            Assert.That(outData.Select(i => i.RefCampaign), Is.EqualTo(outresult.Select(i => i.Id)));
        }

        public static IEnumerable GetPreRegSupplierListwithPasswordString()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\CampaignInvitationRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetPreRegSupplierListwithPasswordStringTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                //public List<CampaignInvitation> GetPreRegSupplierListwithPasswordString(long campaignId)

                int campaignId = Convert.ToInt32(inputNode.Attributes["campaignId"].Value.Trim() == "" ? 0L : Convert.ToInt32(inputNode.Attributes["campaignId"].Value.Trim()));

                List<CampaignInvitation> campaignInvitaionOutObjList = new List<CampaignInvitation>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    CampaignInvitation tempData = new CampaignInvitation();
                    tempData.RefCampaign = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["RefCampaign"].Value.Trim() == "" ? 0 : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["RefCampaign"].Value.Trim()));
                    tempData.EmailAddress = Convert.ToString(outputNode.ChildNodes[j].Attributes["EmailAddress"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["EmailAddress"].Value.Trim()));
                    campaignInvitaionOutObjList.Add(tempData);

                }

                var forGetPreRegSupplierListwithPasswordStringTestCase = new
                {
                    campaignId,
                    campaignInvitaionOutObjList,
                };

                returnValue.Add(forGetPreRegSupplierListwithPasswordStringTestCase);
            }
            return returnValue;

        }
        #endregion
        #region GetCampaignInvitationRecordTest
       // [TestCaseSource("GetCampaignInvitationRecord")]
        public void GetCampaignInvitationRecordTest(object o)
        {
            dynamic x = o;
            ICampaignUow cu = new CampaignUow();
            var id = x.id;
            CampaignInvitation outData = x.campaignInvitaionOutObjList;

            CampaignInvitation outresult = cu.CampaignInvitations.GetCampaignInvitationRecord(id);

            Assert.That(outData.EmailAddress, Is.EqualTo(outresult.EmailAddress));
            Assert.That(outData.RefCampaign, Is.EqualTo(outresult.RefCampaign));
        }

        public static IEnumerable GetCampaignInvitationRecord()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\CampaignInvitationRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetCampaignInvitationRecordTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                
                int id = Convert.ToInt32(inputNode.Attributes["id"].Value.Trim() == "" ? 0L : Convert.ToInt32(inputNode.Attributes["id"].Value.Trim()));
              
                    CampaignInvitation campaignInvitaionOutObjList = new CampaignInvitation();
                campaignInvitaionOutObjList.RefCampaign = Convert.ToInt64(outputNode.Attributes["RefCampaign"].Value.Trim() == "" ? 0 : Convert.ToInt64(outputNode.Attributes["RefCampaign"].Value.Trim()));
                campaignInvitaionOutObjList.EmailAddress = Convert.ToString(outputNode.Attributes["EmailAddress"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["EmailAddress"].Value.Trim()));                              

                var forGetCampaignInvitationRecordTestCase = new
                {
                    id,
                    campaignInvitaionOutObjList,
                };

                returnValue.Add(forGetCampaignInvitationRecordTestCase);
            }
            return returnValue;

        }
        #endregion

    }
}
