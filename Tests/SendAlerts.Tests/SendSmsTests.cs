using System;
using System.Linq;
using Quze.Infrastruture.Utilities;
using Xunit;
using Quze.Models.Models.Alerts;
using Quze.SendAlerts;

namespace SendAlerts.Tests
{
    public class SendSmsTests
    {
        private readonly SendSMS sendSmsTest;

        /// <summary>
        ///  default ctor
        /// </summary>
        public SendSmsTests()
        {
            var alertsToSend = DemoAlertsList.GetTestAlrtesList();
            sendSmsTest = new SendSMS
            {
                SmsHandler = new SlngSMS(),
                AlertsToSend = alertsToSend
            };
        }

     

        private int GetNumberOfUnsent()
        {
            var numberOfSMSToSend = sendSmsTest.AlertsToSend.Count(
                                                  x => x.AlertType == AlertType.SMS
                                                  && x.TimeToSend.Day == DateTime.Now.Day
                                                  && x.ActualSentTime == null
              );

            return numberOfSMSToSend;
        }

        [Fact]
        public void SendAll_Success()
        {
            var alertsToSendBefore = GetNumberOfUnsent();
            sendSmsTest.SendAll();
            var alertsToSendAfter = GetNumberOfUnsent();

            Assert.True(alertsToSendAfter == 0);
        }

    }
}
