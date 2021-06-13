using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Quze.Infrastruture.Security
{
    public class JwtUser : IUser
    {
        public JwtUser()
        {

        }



        public JwtUser(ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            Id = Convert.ToInt32(id);
            UserName = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
            var type = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Typ);

            var organizationId = claimsPrincipal.FindFirstValue(QuzeJwtRegisteredClaimNames.OrganizationId);
            OrganizationId = Convert.ToInt32(organizationId == "" ? "0" : organizationId);
            UserTypeId = Convert.ToInt32(type);
            IdentityNumber = claimsPrincipal.FindFirstValue(QuzeJwtRegisteredClaimNames.IdentityNumber);
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public int? OrganizationId { get; set; }
        public int UserTypeId { get; set; }
        public string IdentityNumber { get; set; }
        public string IdentityUser { get; set; }
    }
}
