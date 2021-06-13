using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using static Quze.API.IVR.YemotHamashiach;
using Quze.API.IVR;
using Quze.Infrastruture.Extensions;
using Quze.DAL;

namespace Quze.API.Controllers
{
    [Route("[controller]")]

    public class IVRController : ControllerBase
    {
        QuzeContext ctx;
        public IVRController(QuzeContext ctx)
        {
            this.ctx = ctx;
        }

        [Route("send")]
        [HttpPost]
        public async Task<string> sendIVRMessageAsync([FromBody] Message message)
        {
            var yemot = new YemotHamashiach();
            var response = await yemot.sendIVRMessageAsync(message);
            if (response.responseStatus == "OK")
            {
                return "success";
            }
            else
            {
                return "faild";
            }
        }

        [Route("qflow")]
        [HttpPost]
        public async Task<string> sendQflowIVRMessageAsync([FromBody] QFlowIVR message)
        {
            if(! message.AppointmentTime.IsTomorrow())
            {
                return "you can ask ivr message only for tomorrow's appointments";
            }
            try
            {
                var yemot = new YemotHamashiach();
                var response = await yemot.sendIVRMessageAsync(new Message(message));
                if (response.responseStatus == "OK")
                {
                    return "success";
                }
                else
                {
                    return "faild";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



    }

    

}


