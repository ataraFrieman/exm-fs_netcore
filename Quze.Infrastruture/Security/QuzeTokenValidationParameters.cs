using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Quze.Infrastruture.Security
{
    public class QuzeTokenValidationParameters: TokenValidationParameters
    {
        public int JwtExpireDays { get; set; } = 3000;

        IConfiguration configuration;
        public QuzeTokenValidationParameters(IConfiguration configuration)
        {
            this.configuration = configuration;
            Init();
        }


        void Init()
        {
            //QuzeContext = new QuzeContext( configuration[];
            var secretKey = configuration["Authentication:JwtKey"];
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            ValidIssuer = configuration["Authentication:JwtIssuer"];
            ValidAudience = configuration["Authentication:JwtAudience"];
            JwtExpireDays = int.Parse(configuration["Authentication:JwtExpireDays"]);
            ClockSkew = TimeSpan.Zero; // remove delay of token when expire
        }
    }
}
