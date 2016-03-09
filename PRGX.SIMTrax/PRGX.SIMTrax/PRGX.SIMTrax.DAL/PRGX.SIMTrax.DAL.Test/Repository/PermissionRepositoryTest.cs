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
    public class PermissionRepositoryTest
    {
        #region GetAllPermissionsTest
       // [TestCaseSource("GetAllPermissions")]
        public void GetAllPermissionsTest(object o)
        {

            dynamic x = o;
            IRoleUow ru = null;
            ru = new RoleUow();

            List<RolePermission> result = ru.Permissions.GetAllPermissions(x.roleId);
            List<RolePermission> roleList = result.Take(3).ToList();
            List<RolePermission> outResult = x.List;
            Assert.That(roleList.Select(i=>i.Description), Is.EqualTo(outResult.Select(i=>i.Description)));
            Assert.That(roleList.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable GetAllPermissions()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\PermissionRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetAllPermissionsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var roleId = Convert.ToInt32(inputNode.Attributes["roleId"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["roleId"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<RolePermission> List = new List<RolePermission>();

                for (int j = 0; j < outputNode.ChildNodes.Count; j++)

                {
                    var tempData = new RolePermission();

                    tempData.Description = Convert.ToString(outputNode.ChildNodes[j].Attributes["desc"].Value.Trim() == "" ? "": Convert.ToString(outputNode.ChildNodes[j].Attributes["desc"].Value.Trim()));
                    List.Add(tempData);
                }
                var forGetAllPermissionsTestCase = new
                {
                   roleId,
                   List,
                   total
                };

                returnValue.Add(forGetAllPermissionsTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
