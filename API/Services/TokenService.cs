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
        private readonly SymmetricSecurityKey _key;
        // there are 2 types of keys used in cryptography 
        // 1. symmetric key: the same key is used to encrypt the data as is used to decrypt the data 
        // Since our server is responsible for both signing and decrypting the token key 

        // 2. asymmetric key: thats when your server needs to encrypt sth,
        // & the client also needs to decrypt sth 
        // On that basis, we have a public (used to decrypt data) and a private key (stays on the server)

        // In this case, we can use Symmetric bc it's gonna stay in the server and never going to the client 
        // bc the client does not need to decrypt this key 

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            // A claim is a statement about an entity (typically the user) and additional information relevant to the token.
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
                // 1. JwtRegisteredClaimNames.NameId: This is a predefined claim type provided by the System.IdentityModel.Tokens.Jwt namespace. 
                // It represents the unique identifier for the subject (user) of the token.

                // 2. user.UserName: This is the value associated with the claim. 
                // It typically represents the username or some unique identifier for the user.
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims), // Specifies the subject of the token, which typically includes the claims about the user.
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            // Creates an instance of JwtSecurityTokenHandler, which is responsible for creating and validating JWT tokens.

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);  
            // Converts the JWT token into its string representation, which is the actual token that will be sent to clients.
        }
    }
}