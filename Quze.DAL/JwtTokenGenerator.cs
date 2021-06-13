using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Security;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Quze.DAL
{

    public class JwtTokenGenerator
    {
        DAL.Stores.UserStore userStore;
        QuzeTokenValidationParameters quzeTokenValidationParameters;

        public JwtTokenGenerator(IUserStore<User> userStore, QuzeTokenValidationParameters quzeTokenValidationParameters)
        {
            this.userStore = (DAL.Stores.UserStore)userStore;
            this.quzeTokenValidationParameters = quzeTokenValidationParameters;
        }

        /// <summary>
        /// GenerateJwtToken for API/Website/Organization Site
        /// </summary>
        /// <param name="userId">the id of a user that exists in quze DB</param>
        /// <returns></returns>
        public async Task<string> GenerateJwtTokenAsync(string userId, int userType)
        {
            var user = await userStore.FindByIdAsync(userId, userType, CancellationToken.None);
            if (user.IsNull())
            {
                throw new UserNotFoundException(userId);
            }

            var claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Typ, "ApiUser"),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Typ, user.UserTypeId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName.ValueOrEmpty()),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName.ValueOrEmpty()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.ValueOrEmpty()),
                new Claim(QuzeJwtRegisteredClaimNames.OrganizationId, user.OrganizationId.ToString()),
                new Claim(QuzeJwtRegisteredClaimNames.IdentityNumber, user.IdentityNumber.ToString())
            };

            var key = quzeTokenValidationParameters.IssuerSigningKey;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(quzeTokenValidationParameters.JwtExpireDays));

            var token = new JwtSecurityToken(
                quzeTokenValidationParameters.ValidIssuer,
                quzeTokenValidationParameters.ValidAudience,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenstring;
        }



    }

    public class UserNotFoundException : ApplicationException
    {
        public UserNotFoundException(string userId) : base("user id " + userId + " not found")
        { }

    }

}
