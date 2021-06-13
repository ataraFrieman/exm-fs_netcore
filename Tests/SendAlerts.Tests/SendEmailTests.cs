using System;
using System.Linq;
using Xunit;
using Quze.Models.Models.Alerts;
using Quze.SendAlerts;
using Quze.Infrastruture.Utilities.Email;

namespace SendAlerts.Tests
{
    public class SendEmailTests
    {
        private readonly SendEmail sendEmailTest;

        /// <summary>
        ///  default ctor
        /// </summary>
        public SendEmailTests()
        {
           sendEmailTest = new SendEmail(new GmailMail())
            {
                AlertsToSend = DemoAlertsList.GetTestAlrtesList()
            };
        }

        private int GetNumberOfUnsent()
        {
            var numberOfSMSToSend = sendEmailTest.AlertsToSend.Count(
                                                  x => x.AlertType == AlertType.Email
                                                  && x.TimeToSend.Day == DateTime.Now.Day
                                                  && x.ActualSentTime == null
              );

            return numberOfSMSToSend;
        }

        [Fact]
        public void SendAll_Success()
        {
            var alertsToSendBefore = GetNumberOfUnsent();
            sendEmailTest.SendAll();
            var alertsToSendAfter = GetNumberOfUnsent();

            Assert.True(alertsToSendAfter == 0);
        }
    }
}
