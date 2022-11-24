
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key; //symmetric keys stays on the server and not on the client
        public TokenService(IConfiguration config) // store the secret key to sign the toke.It is stored in configuration
        {
            //set the key
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])); //SymmetricSecurityKey takes byte array
        }
        public string CreateToken(AppUser user)
        {
           var claims = new List<Claim>                                //Claim are the information a user claims.We use list so that we can add more than one claim                                                            //
           {
              new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
           };

           //Signing credentials
           var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature );


           //describe the token we are gonna return
           var tokenDescriptor = new SecurityTokenDescriptor
           { 
             Subject = new ClaimsIdentity(claims), //subject includes the claims we want to return
             Expires = DateTime.Now.AddDays(7), //setting the expiry date for the token
             SigningCredentials = creds //setting the signing credentials to creds
           };
            
            //setting a token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            //creating the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //returning the token
            return tokenHandler.WriteToken(token);
 
        }
    }
}