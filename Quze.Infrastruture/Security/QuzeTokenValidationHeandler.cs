using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Quze.Infrastruture.Security
{

    public class QuzeTokenValidationHeandler
    {
        QuzeTokenValidationParameters quzeTokenValidationParameters;
        public QuzeTokenValidationHeandler(QuzeTokenValidationParameters quzeTokenValidationParameters)
        {
            this.quzeTokenValidationParameters = quzeTokenValidationParameters;
        }

        public void GetJwtBearerOptions(JwtBearerOptions cfg)
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = quzeTokenValidationParameters;
           
        }


    }
}
