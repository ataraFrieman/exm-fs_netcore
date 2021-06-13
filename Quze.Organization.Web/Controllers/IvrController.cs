using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Organization.Web.Utilites;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.Organization.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Logic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class IvrController : Controller
    {
        [AllowAnonymous]
        [HttpPost("[action]")]
        public bool AVRMessage([FromBody]string messageContent) {
            TextToSpeech tts = new TextToSpeech();
            tts.getVoiceFile(messageContent);
            return true;
        }

      

    }
}
