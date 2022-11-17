using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Helper
{
     class JWTHelper
    {
        public static string GetPrincipal(string token, IConfiguration _configuration)
        {
            if (string.IsNullOrEmpty(token))
            {
                return "";
            }
            try
            {
                token = token[7..];
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

                JwtSecurityTokenHandler handler = new();
                TokenValidationParameters validationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],

                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],

                };
                ClaimsPrincipal principal = handler.ValidateToken(token, validationParameters, out SecurityToken tokenValidated);
                if (principal == null)
                    return null;
                ClaimsIdentity identity = null;
                try
                {
                    identity = (ClaimsIdentity)principal.Identity;
                }
                catch (NullReferenceException)
                {
                    return null;
                }
                Claim userIdClaim = identity.Claims.FirstOrDefault();
                string userId = userIdClaim.Value;
                return userId;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string GetToken(string id, IConfiguration _configuration)
        {
            try
            {
                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var tokenHandler = new JwtSecurityTokenHandler();
                var now = DateTime.UtcNow;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                            {
                                    new Claim("Id",id)
                            }),
                    NotBefore = now,
                    Expires = now.AddYears(10),
                    Issuer = _configuration["JWT:ValidIssuer"],
                    Audience = _configuration["JWT:ValidAudience"],
                    IssuedAt = now,
                    SigningCredentials = new SigningCredentials(
                symmetricKey,
                SecurityAlgorithms.HmacSha256Signature),
                };
                var stoken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(stoken);
                return "Bearer " + token;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string GenerateToken(string id, IConfiguration _configuration)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
