using CreateAlerts.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quze.CreateAlerts;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CreateAlertsService.Tests
{
    [TestClass]
    public class CreateAlertsTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var initAppointments = new InitAppointmentList();
            var appointments = new List<Appointment>() { initAppointments.initAppointmentWithoutAlerts() };
            var createAlert = new CreateAppointmentsAlert(appointments, new DateTime(2019, 1, 1, 10, 0, 0));
            var alerts = createAlert.GetAllAppointmentsAlerts();
            
            Assert.IsTrue(alerts.Count() == 2);
        }
    }
}
