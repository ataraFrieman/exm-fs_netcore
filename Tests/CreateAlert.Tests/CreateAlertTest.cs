using CreateAlerts.Test;
using NUnit.Framework;
using Quze.Models.Entities;
using System.Collections.Generic;
using Quze.CreateAlerts;
using System;
using System.Linq;

namespace Tests
{
    public class CreateAlertTest
    {
        [SetUp]
        public void Setup()
        {
        }

        

        [Test]
        public void TestWithoutExistingAlerts()
        {
            var initAppointments = new InitAppointmentList();
            var appointments = new List<Appointment>() { initAppointments.initAppointmentWithoutAlerts() };
            var createAlert = new CreateAppointmentsAlert(appointments, new DateTime(2019, 1, 1, 10, 0, 0));
            var alerts = createAlert.GetAllAppointmentsAlerts();
            Assert.True(alerts.Count() == 2);
        }

        [Test]
        public void TestStrings()
        {
            var s1 = @"Line 1\""Line 2";
           
        }
    }
}