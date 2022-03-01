using Appointment_Manager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Appointment_ManagerTests
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public void LoginTest()
        {
            string connectionString = "server=localhost;user id=root;password=root;database=scheduledb;persistsecurityinfo=True;";
            string userName = "test";
            string password = "test";
            bool exists = User.Login(userName, password, connectionString);

            Assert.IsTrue(exists);
        }
    }
}
