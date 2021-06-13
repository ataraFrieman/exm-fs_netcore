using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quze.DAL;
using Quze.Infrastruture.Security;
using Quze.Infrastruture.Extensions;

namespace Quze.Organization.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        protected readonly QuzeContext context;
        protected readonly IMapper mapper;

        public BaseController(QuzeContext ctx, IMapper mapper)
        {
            context = ctx;
            this.mapper = mapper;
        }

        JwtUser jwtUser;
        public JwtUser JwtUser
        {
            get
            {
                if (jwtUser.IsNull())
                {
                    jwtUser = new JwtUser(this.User);
                }
                return jwtUser;
            }
        }

    }
}