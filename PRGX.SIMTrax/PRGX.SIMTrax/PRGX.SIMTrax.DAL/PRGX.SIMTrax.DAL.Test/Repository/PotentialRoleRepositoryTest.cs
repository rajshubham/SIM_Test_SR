using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Abstract;
using System.Linq;
using PRGX.SIMTrax.Domain.Model;

namespace PRGX.SIMTrax.DAL.Test.TestData
{
    [TestFixture]
    public class PotentialRoleRepositoryTest
    {
        #region GetAllAccessTypesTest
      //  [TestCaseSource("GetAllAccessTypes")]
        public void GetAllAccessTypesTest(object o)
        {

            dynamic x = o;
            IRoleUow ru = null;
            ru = new RoleUow();
            int total = x.totalRecords;
            List<PotentialRole> result = ru.PotentialRoles.GetAllAccessTypes(x.accessType, x.pageSize, x.index, out total);
            List<PotentialRole> outResult = x.List;
            Assert.That(result.Select(i => i.Id), Is.EqualTo(outResult.Select(i => i.Id)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetAllAccessTypes()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PotentialRoleRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetAllAccessTypesTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var accessType = Convert.ToInt32(inputNode.Attributes["accessType"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["accessType"].Value.Trim()));
                var pageSize = Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim()));
                var index = Convert.ToInt32(inputNode.Attributes["index"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["index"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<PotentialRole> List = new List<PotentialRole>();

                for (int j = 0; j < outputNode.ChildNodes.Count; j++)

                {
                    var tempData = new PotentialRole();

                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    List.Add(tempData);
                }
                var forGetAllAccessTypesTestCase = new
                {
                    accessType,
                    pageSize,
                    index,
                    totalRecords,
                    List,
                    total
                };

                returnValue.Add(forGetAllAccessTypesTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
