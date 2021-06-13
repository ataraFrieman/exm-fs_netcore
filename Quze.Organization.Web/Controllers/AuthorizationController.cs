using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quze.DAL;
using Quze.Models.Entities;

namespace Quze.Organization.Web.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class AuthorizationController : BaseController
    {
        JwtTokenGenerator jwtTokenGenerator;

        public AuthorizationController(QuzeContext ctx, JwtTokenGenerator jwtTokenGenerator, IMapper mapper) : base(ctx, mapper)
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
                var token = await jwtTokenGenerator.GenerateJwtTokenAsync(id, UserType.OrganizationUser);
                return token;
            }
            catch (UserNotFoundException ex)
            {
                return ex.Message;
            }
           
        }


    }
}