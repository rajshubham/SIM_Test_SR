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
   public class UserRepositoryTest
    {
        #region IsEmailExistsTest
         // [TestCaseSource("IsEmailExists")]
        public void IsEmailExistsTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();

            bool result = u.Users.IsEmailExists(x.email);


            Assert.That(result, Is.EqualTo(x.outObj));

        }

        public static IEnumerable IsEmailExists()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\UserRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("IsEmailExistsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var email = Convert.ToString(inputNode.Attributes["email"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["email"].Value.Trim()));
                var outObj = Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim()));

                var forIsEmailExistsTestCase = new
                {
                    email,
                    outObj
                };

                returnValue.Add(forIsEmailExistsTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetUserDetailsByOrganisationPartyIdTest
       //[TestCaseSource("GetUserDetailsByOrganisationPartyId")]
        public void GetUserDetailsByOrganisationPartyIdTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();

            UserDetails result = u.Users.GetUserDetailsByOrganisationPartyId(x.organizationPartyId);
            if (result != null)
            {
                Assert.That(result.LoginId, Is.EqualTo(x.userDetails.LoginId));
            }
           else
            {
                Assert.That(result, Is.EqualTo(null));
            }

        }

        public static IEnumerable GetUserDetailsByOrganisationPartyId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\UserRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetUserDetailsByOrganisationPartyIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organizationPartyId = Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim()));

                var userDetails = new UserDetails();

                userDetails.LoginId = Convert.ToString(outputNode.Attributes["loginId"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["loginId"].Value.Trim()));
               
                var forGetUserDetailsByOrganisationPartyIdTestCase = new
                {
                    organizationPartyId,
                    userDetails
                };

                returnValue.Add(forGetUserDetailsByOrganisationPartyIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetUserDetailByUserIdTest
        //  [TestCaseSource("GetUserDetailByUserId")]
        public void GetUserDetailByUserIdTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();

            UserDetails result = u.Users.GetUserDetailByUserId(x.userId);
            if (result != null)
            {
                Assert.That(result.LoginId, Is.EqualTo(x.userDetails.LoginId));
            }
            else
            {
                Assert.That(result, Is.EqualTo(null));
            }


        }

        public static IEnumerable GetUserDetailByUserId()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\UserRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetUserDetailByUserIdTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var userId = Convert.ToInt64(inputNode.Attributes["userId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["userId"].Value.Trim()));

                var userDetails = new UserDetails();

                userDetails.LoginId = Convert.ToString(outputNode.Attributes["loginId"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.Attributes["loginId"].Value.Trim()));

                var forGetUserDetailByUserIdTestCase = new
                {
                    userId,
                    userDetails
                };

                returnValue.Add(forGetUserDetailByUserIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetPartyUserTest
        // [TestCaseSource("GetPartyUser")]
        public void GetPartyUserTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();

            List<User> result = u.Users.GetPartyUser(x.organizationPartyId);
            List<User> outResult = x.user;
            Assert.That(result.Select(i=>i.Id), Is.EqualTo(outResult.Select(i=>i.Id)));
            Assert.That(result.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable GetPartyUser()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\UserRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetPartyUserTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var organizationPartyId = Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["orgPartyId"].Value.Trim()));
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<User> user = new List<User>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new User();

                    tempData.Id = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["Id"].Value.Trim()));
                    user.Add(tempData);
                }
                var forGetUserDetailByUserIdTestCase = new
                {
                    organizationPartyId,
                    user,
                    total
                };

                returnValue.Add(forGetUserDetailByUserIdTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetAllUsersTest
        // [TestCaseSource("GetAllUsers")]
        public void GetAllUsersTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();
            int total = x.totalRecords;
            List<UserAccount> result = u.Users.GetAllUsers(x.loginId,x.userName,x.userType,x.status,x.source,out total,x.pageIndex,x.pageSize,x.sortDirection);
            List<UserAccount> outResult = x.user;
            Assert.That(result.Select(i => i.UserId), Is.EqualTo(outResult.Select(i => i.UserId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetAllUsers()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\UserRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetAllUsersTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var loginId = Convert.ToString(inputNode.Attributes["loginId"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["loginId"].Value.Trim()));
                var userName = Convert.ToString(inputNode.Attributes["userName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["userName"].Value.Trim()));
                var userType = Convert.ToInt32(inputNode.Attributes["userType"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["userType"].Value.Trim()));
                var status = Convert.ToString(inputNode.Attributes["status"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["status"].Value.Trim()));
                var source = Convert.ToInt16(inputNode.Attributes["source"].Value.Trim() == "" ? 0 : Convert.ToInt16(inputNode.Attributes["source"].Value.Trim()));
                var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var pageIndex = Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim() == "" ? 0: Convert.ToInt32(inputNode.Attributes["pageIndex"].Value.Trim()));
                var pageSize = Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageSize"].Value.Trim()));
                var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0: Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));

                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<UserAccount> user = new List<UserAccount>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new UserAccount();

                    tempData.UserId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["userId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["userId"].Value.Trim()));
                    user.Add(tempData);
                }
                var forGetAllUsersTestCase = new
                {
                   loginId,
                   userName,
                   userType,
                   status,
                   source,
                   totalRecords,
                   pageIndex,
                   pageSize,
                   sortDirection,
                    user,
                    total
                };

                returnValue.Add(forGetAllUsersTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetUserNameTest
       //  [TestCaseSource("GetUserName")]
        public void GetUserNameTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();
            List<string> result = u.Users.GetUserName(x.userName);
            List<string> outResult = x.user;
            Assert.That(result, Is.EqualTo(outResult));
            Assert.That(result.Count, Is.EqualTo(x.total));

        }

        public static IEnumerable GetUserName()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\UserRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetUserNameTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                 var userName = Convert.ToString(inputNode.Attributes["userName"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["userName"].Value.Trim()));
               
                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<string> user = new List<string>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData =  Convert.ToString(outputNode.ChildNodes[j].Attributes["userName"].Value.Trim() == "" ? "" : Convert.ToString(outputNode.ChildNodes[j].Attributes["userName"].Value.Trim()));
                    user.Add(tempData);
                }
                var forGetUserNameTestCase = new
                {
                 
                    userName,                  
                    user,
                    total
                };

                returnValue.Add(forGetUserNameTestCase);
            }
            return returnValue;

        }
        #endregion

        #region GetUserDetailsForDashboardTest
         //[TestCaseSource("GetUserDetailsForDashboard")]
        public void GetUserDetailsForDashboardTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();
            int total = x.totalRecords;
            List<UserAccount> result = u.Users.GetUserDetailsForDashboard(x.buyerPartyId,out total,x.pageNumber,x.sortDirection);
            List<UserAccount> outResult = x.user;
            Assert.That(result.Select(i => i.UserId), Is.EqualTo(outResult.Select(i => i.UserId)));
            Assert.That(result.Count, Is.EqualTo(x.total));
            Assert.That(total, Is.EqualTo(x.totalRecords));
        }

        public static IEnumerable GetUserDetailsForDashboard()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\UserRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetUserDetailsForDashboardTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];

                var buyerPartyId = Convert.ToInt64(inputNode.Attributes["buyerPartyId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["buyerPartyId"].Value.Trim()));
               var totalRecords = Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["totalRecords"].Value.Trim()));
                var pageNumber = Convert.ToInt32(inputNode.Attributes["pageNumber"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["pageNumber"].Value.Trim()));
                 var sortDirection = Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["sortDirection"].Value.Trim()));

                var total = Convert.ToInt32(outputNode.Attributes["count"].Value.Trim() == "" ? 0 : Convert.ToInt32(outputNode.Attributes["count"].Value.Trim()));

                List<UserAccount> user = new List<UserAccount>();
                for (int j = 0; j < outputNode.ChildNodes.Count; j++)
                {
                    var tempData = new UserAccount();

                    tempData.UserId = Convert.ToInt64(outputNode.ChildNodes[j].Attributes["userId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.ChildNodes[j].Attributes["userId"].Value.Trim()));
                    user.Add(tempData);
                }
                var forGetUserDetailsForDashboardTestCase = new
                {
                  buyerPartyId,
                    totalRecords,
                 pageNumber,
                    sortDirection,
                    user,
                    total
                };

                returnValue.Add(forGetUserDetailsForDashboardTestCase);
            }
            return returnValue;

        }
        #endregion
    }
}
