using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quze.DAL;
using Quze.Models.Entities;

namespace Quze.API.HelpControllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthorizationController : ControllerBase
    {
        JwtTokenGenerator jwtTokenGenerator;

        public AuthorizationController(JwtTokenGenerator jwtTokenGenerator, IMapper mapper) 
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        [AllowAnonymous]
        [HttpGet("{id}/{pass}")]
        public async Task<string> Get(string id,string pass)
        {
            if (pass != "baruchHashem") return "Al Taavod Alay!!";
            try
            {
                var token = await jwtTokenGenerator.GenerateJwtTokenAsync(id, UserType.SuperUser);
                return token;
            }
            catch (UserNotFoundException ex)
            {
                return ex.Message;
            }
           
        }


    }
}