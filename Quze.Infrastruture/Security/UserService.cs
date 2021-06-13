using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Quze.Infrastruture.Security
{

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private readonly string _currentUserGuid;
        //private readonly string _currentUserName;
        //private readonly User _currentUser;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentUserId()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Convert.ToInt32(id);
        }

        public string GetCurrentUserName()
        {
            var name = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
            return name;
        }


        public IUser GetCurrentUser()
        {
            //TODO: לא הושלם יש לבדוק
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var user = new JwtUser();

            var id = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            user.Id= Convert.ToInt32(id);
            user.UserName = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
            var organizationId = claimsPrincipal.FindFirstValue(QuzeJwtRegisteredClaimNames.OrganizationId);
            user.OrganizationId = Convert.ToInt32(organizationId);
            var type = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Typ);
            user.UserTypeId = Convert.ToInt32(type);
            return user;
        }



    }
}
