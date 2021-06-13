using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Quze.Infrastruture.Security;
using System.IdentityModel.Tokens.Jwt;

namespace Quze.Infrastruture.Extensions
{
    public static class IServicesExtensions
    {
        public static AuthenticationBuilder AddQuzeJwtAuthentication(this IServiceCollection me)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            // Build the intermediate service provider, sp retrives services from the services collection
            
            //var configuration = sp.GetService<IConfiguration>();
            
            //var quzeTokenValidationParameters = new QuzeTokenValidationParameters(configuration);
            me.AddSingleton<QuzeTokenValidationParameters, QuzeTokenValidationParameters>();

            var sp = me.BuildServiceProvider();
            var quzeTokenValidationParameters = sp.GetService<QuzeTokenValidationParameters>();
            var quzeTokenValidationHeandler = new QuzeTokenValidationHeandler(quzeTokenValidationParameters);

            
            
            return me.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cg => quzeTokenValidationHeandler.GetJwtBearerOptions(cg));
        }
    }
}
