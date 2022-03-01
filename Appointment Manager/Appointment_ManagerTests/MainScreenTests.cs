using Appointment_Manager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Appointment_ManagerTests
{
    [TestClass]
    public class MainScreenTests
    {
        [TestMethod]
        public void SearchAppointmentListShouldReturnMultipleRows()
        {
            string searchTerm = "ee";

            SortedList<DateTime, Appointment> appointmentList = new SortedList<DateTime, Appointment>
            {
                {
                    DateTime.Now, new Appointment(
                        "John Doe", DateTime.Now.AddMinutes(1), DateTime.Now.AddHours(1),
                        "Presentation", "Not needed", "", "", "", 1, "", 1)
                },
                {
                    DateTime.Now.AddMinutes(2), new Appointment
                    ("Alfred Newman", DateTime.Now.AddMinutes(12), DateTime.Now.AddHours(2),
                    "Scrum Meeting", "", "", "", "", 2, "", 2)
                },
                {
                    DateTime.Now.AddMinutes(5), new Appointment
                    ("Ina Prufung", DateTime.Now.AddMinutes(30), DateTime.Now.AddHours(3),
                    "Presentation", "", "", "", "", 3, "", 3)
                }
            };
            List<Appointment> resultList = new MainScreen().SearchAppointments(searchTerm, appointmentList);
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 1);
            Assert.IsTrue(resultList.Count < 3);
        }

        [TestMethod]
        public void SearchCustomerListShouldReturnMultipleRows()
        {
            string searchTerm = "new";
            List<Customer> customerList = new List<Customer>
            {
                new Customer
                    ("John Doe", 1, true, "123 Main", "", 1, "New York", 1, "US",
                    1, "2222222", "22222"),
                new Customer
                    ("Alfred Newman", 1, true, "123 Elm", "", 2, "Chicago", 2, "US",
                    1, "3333333", "33333"),
                new Customer
                    ("Ina Prufung", 1, true, "123 Oak", "", 3, "Los Angeles", 3, "US",
                    1, "4444444", "44444")
            };
            List<Customer> resultList = new MainScreen().SearchCustomers(searchTerm, customerList);
            
            Assert.IsNotNull(resultList);
            Assert.IsTrue(resultList.Count > 1);
            Assert.IsTrue(resultList.Count < 3);
        }
    }
}
