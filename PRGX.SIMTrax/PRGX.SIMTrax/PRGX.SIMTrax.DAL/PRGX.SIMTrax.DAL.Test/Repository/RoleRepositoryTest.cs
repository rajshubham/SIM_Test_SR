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

namespace PRGX.SIMTrax.DAL.Test.Repository
{
    [TestFixture]
   public class RoleRepositoryTest
    {
        #region GetRoleDetailsTest
    //  [TestCaseSource("GetRoleDetails")]
        public void GetRoleDetailsTest(object o)
        {

            dynamic x = o;
            IRoleUow ru = null;
            ru = new RoleUow();

           Role result = ru.Roles.GetRoleDetails(x.roleId);
           if (result != null)
           {
               Assert.That(result.Description, Is.EqualTo(x.desc));
           }
           else
           {

               Assert.That(result, Is.EqualTo(null));
           }
           
        }

        public static IEnumerable GetRoleDetails()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\RoleRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetRoleDetailsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var roleId = Convert.ToInt64(inputNode.Attributes["roleId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["roleId"].Value.Trim()));
                                                  
                var desc = Convert.ToString(outputNode.Attributes["desc"].Value.Trim() == "" ? "": Convert.ToString(outputNode.Attributes["desc"].Value.Trim()));

                var forGetRoleDetailsTestCase = new
                {
                    roleId,
                  desc
                };

                returnValue.Add(forGetRoleDetailsTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetUserPermissionBasedOnUserIdTest
        // [TestCaseSource("GetUserPermissionBasedOnUserId")]
        public void GetUserPermissionBasedOnUserIdTest(object o)
        {

            dynamic x = o;
            IRoleUow ru = null;
            ru = new RoleUow();

            List<ItemList> result = ru.Roles.GetUserPermissionBasedOnUserId(x.userId);
            List<ItemList> outResult = x.permissionList;
            List<ItemList> roleList = result.Take(5).ToList();
                Assert.That(outResult.Select(i=>i.Text), Is.EqualTo(roleList.Select(i=>i.Text)));
            Assert.That(outResult.Select(i => i.Value), Is.EqualTo(roleList.Select(i => i.Value)));

        }

        public static IEnumerable GetUserPermissionBasedOnUserId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\RoleRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetUserPermissionBasedOnUserIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var userId = Convert.ToInt64(inputNode.Attributes["userId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["userId"].Value.Trim()));
                List<ItemList> permissionList = new List<ItemList>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new ItemList();
                    tempData.Text = Convert.ToString(outputNode.ChildNodes[j].Attributes["text"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["text"].Value.Trim()));
                    tempData.Value = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["value"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["value"].Value.Trim()));
                    permissionList.Add(tempData);
            }
                var forGetUserPermissionBasedOnUserIdTestCase = new
                {
                    userId,
                    permissionList
                };

                returnValue.Add(forGetUserPermissionBasedOnUserIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetUserListByPermissionTest
      //[TestCaseSource("GetUserListByPermission")]
        public void GetUserListByPermissionTest(object o)
        {

            dynamic x = o;
            IRoleUow ru = null;
            ru = new RoleUow();

            List<ItemList> result = ru.Roles.GetUserListByPermission(x.permission);
            List<ItemList> outResult = x.permissionList;
            Assert.That(result.Select(i => i.Text), Is.EqualTo(outResult.Select(i => i.Text)));
            Assert.That(result.Select(i => i.Value), Is.EqualTo(outResult.Select(i => i.Value)));

        }

        public static IEnumerable GetUserListByPermission()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\RoleRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetUserListByPermissionTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var permission = Convert.ToInt64(inputNode.Attributes["permission"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["permission"].Value.Trim()));
                List<ItemList> permissionList = new List<ItemList>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new ItemList();
                    tempData.Text = Convert.ToString(outputNode.ChildNodes[j].Attributes["text"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["text"].Value.Trim()));
                    tempData.Value = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["value"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["value"].Value.Trim()));
                    permissionList.Add(tempData);
                }
                var forGetUserListByPermissionTestCase = new
                {
                    permission,
                    permissionList
                };

                returnValue.Add(forGetUserListByPermissionTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
