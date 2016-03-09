using System;
using NUnit.Framework;
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
    public class CredentialRepositoryTest
    {
        #region UpdateUserLastLoginDateTest
       // [TestCaseSource("UpdateUserLastLoginDate")]
        public void UpdateUserLastLoginDateTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();

            u.BeginTransaction();
            bool result = u.Credentials.UpdateUserLastLoginDate(x.userId, x.loginId);
            u.SaveChanges();
            u.Commit();


            Assert.That(result, Is.EqualTo(x.outObj));

        }

        public static IEnumerable UpdateUserLastLoginDate()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\CredentialRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("UpdateUserLastLoginDateTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var userId = Convert.ToInt64(inputNode.Attributes["userId"].Value.Trim() == "" ? 0 : Convert.ToInt64(inputNode.Attributes["userId"].Value.Trim()));
                var loginId = Convert.ToString(inputNode.Attributes["loginId"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["loginId"].Value.Trim()));
                var outObj = Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim() == "" ? false : Convert.ToBoolean(outputNode.Attributes["outObj"].Value.Trim()));



                var forUpdateUserLastLoginDateTestCase = new
                {
                    userId,
                    loginId,
                    outObj


                };

                returnValue.Add(forUpdateUserLastLoginDateTestCase);
            }
            return returnValue;

        }
        #endregion


        #region GetUserIdFromCredentialsTest
        // [TestCaseSource("GetUserIdFromCredentials")]
        public void GetUserIdFromCredentialsTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();

            bool isLocked = x.isLocked;
            long result = u.Credentials.GetUserIdFromCredentials(x.email, x.password, x.lockCount, x.timeSpanLimit, out isLocked);

            Assert.That(result, Is.EqualTo(x.userId));
            Assert.That(isLocked, Is.EqualTo(x.isLocked));
        }

        public static IEnumerable GetUserIdFromCredentials()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\CredentialRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("GetUserIdFromCredentialsTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var lockCount = Convert.ToInt32(inputNode.Attributes["lockCount"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["lockCount"].Value.Trim()));
                var email = Convert.ToString(inputNode.Attributes["email"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["email"].Value.Trim()));
                var password = Convert.ToString(inputNode.Attributes["password"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["password"].Value.Trim()));
                var timeSpanLimit = Convert.ToInt32(inputNode.Attributes["timeSpanLimit"].Value.Trim() == "" ? 0 : Convert.ToInt32(inputNode.Attributes["timeSpanLimit"].Value.Trim()));
                var isLocked = Convert.ToBoolean(inputNode.Attributes["isLocked"].Value.Trim() == "" ? false : Convert.ToBoolean(inputNode.Attributes["isLocked"].Value.Trim()));
                var userId = Convert.ToInt64(outputNode.Attributes["userId"].Value.Trim() == "" ? 0L : Convert.ToInt64(outputNode.Attributes["userId"].Value.Trim()));



                var forGetUserIdFromCredentialsTestCase = new
                {
                    lockCount,
                    email,
                    password,
                    timeSpanLimit,
                    isLocked,
                    userId


                };

                returnValue.Add(forGetUserIdFromCredentialsTestCase);
            }
            return returnValue;

        }
        #endregion

        #region UpdatePasswordTest
       // [TestCaseSource("UpdatePassword")]
        public void UpdatePasswordTest(object o)
        {

            dynamic x = o;
            IUserUow u = null;
            u = new UserUow();
            u.BeginTransaction();
            u.Credentials.UpdatePassword(x.loginId, x.encryptedPassword, x.userId);
            u.SaveChanges();
            var data = u.Credentials.GetAll().Where(c => c.RefUser == x.userId).ToList();
            u.Commit();
            Assert.That(data[0].LoginId, Is.EqualTo(x.loginId));
            Assert.That(data[0].Password, Is.EqualTo(x.encryptedPassword));
        }

        public static IEnumerable UpdatePassword()
        {
            var xmlFilePath = Path.GetFullPath("..\\..\\TestData\\CredentialRepositoryTestData.xml");
            var xDoc = new System.Xml.XmlDocument();
            xDoc.Load(xmlFilePath);

            var xTestCases = xDoc.GetElementsByTagName("UpdatePasswordTestData")[0].ChildNodes;
            var returnValue = new List<object>();

            for (int i = 0; i < xTestCases.Count; i++)
            {
                var inputNode = xTestCases[i].ChildNodes[0];
                var outputNode = xTestCases[i].ChildNodes[1];
                var loginId = Convert.ToString(inputNode.Attributes["loginId"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["loginId"].Value.Trim()));
                var encryptedPassword = Convert.ToString(inputNode.Attributes["encryptedPassword"].Value.Trim() == "" ? "" : Convert.ToString(inputNode.Attributes["encryptedPassword"].Value.Trim()));
                var userId = Convert.ToInt64(inputNode.Attributes["userId"].Value.Trim() == "" ? 0L : Convert.ToInt64(inputNode.Attributes["userId"].Value.Trim()));



                var forUpdatePasswordTestCase = new
                {

                    loginId,
                    encryptedPassword,
                    userId


                };

                returnValue.Add(forUpdatePasswordTestCase);
            }
            return returnValue;

        }
        #endregion

    }
}
