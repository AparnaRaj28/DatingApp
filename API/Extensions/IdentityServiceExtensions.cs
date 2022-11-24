using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>{
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //here we can specify all of the rules on how the token should be validated
                    ValidateIssuerSigningKey = true, //server is gonna check for token signing key and make sure is valid
                    //based on the issuer signing key 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding
                    .UTF8.GetBytes(config["TokenKey"])),
                    // IssuerSigningKeys = new List<SymmetricSecurityKey> { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])) },
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }
        
    }

   
}