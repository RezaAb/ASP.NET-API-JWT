using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.IdentityModel.Tokens;

namespace WebApi2.Models
{
    public class AuthenticationModule
    {
        private const string SecretKey = "kjhjhsiuLKJSLWLl5522asdasdJKHJHNN";
        private SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        public string GenerateTokenForUser(string userName, int userId)
        {
            var signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature,
                SecurityAlgorithms.Sha256Digest);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "Custom");

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = "http://www.example.com",
                Audience = "self",
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                Expires = DateTime.UtcNow.AddYears(1)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(plainToken);
            return encodedToken;
        }

        public JwtSecurityToken GenerateUserClaimFromJWt(string authToken)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuers = new[]
                {
                    "http://www.example.com"
                },
                ValidAudiences = new[]
                {
                    "self",
                    "Any"
                },
                IssuerSigningKey = _signingKey
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(authToken, tokenValidationParameters, out validatedToken);
            }
            catch (Exception e)
            {
                return null;
            }
            return validatedToken as JwtSecurityToken;
        }

        public JWTIdentity PopulateUserIdentity(JwtSecurityToken jwtSecurityToken)
        {
            string name = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "unique_name").Value;
            string userId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            return new JWTIdentity(name) {UserId = Convert.ToInt32(userId), UserName = name};
        }
    }
}