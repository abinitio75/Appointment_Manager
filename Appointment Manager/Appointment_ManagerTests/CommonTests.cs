using Microsoft.VisualStudio.TestTools.UnitTesting;
using Appointment_Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_ManagerTests
{
    [TestClass()]
    public class CommonTests
    {
        [TestMethod()]
        public void GetHashTest()
        {
            string password = "test";
            string hashedPassword = User.GetHash(password);
            Assert.AreEqual(hashedPassword, "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08");
        }
    }
}
