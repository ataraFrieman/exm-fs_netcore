using Quze.Infrastruture.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.API.IVR
{
    public class Message
    {
        public string TextToSpeech { get; set; }
        public string Phone { get; set; }
        public Message()
        {

        }
        public Message(QFlowIVR qFlowIVR)
        {
            Phone = qFlowIVR.PhoneNumber;
            if (qFlowIVR.AppointmentTime.IsTomorrow())
                TextToSpeech = "Hello " + qFlowIVR.FellowName +
                    " , this is Quze speaking, we wanted to remind you that do you have an appointment for "
                    + qFlowIVR.ServiceType
                    + " tommorow at " + qFlowIVR.AppointmentTime.ToString("h:mm tt") +
                    " at Givat Ram";
        }
    }
    
}
