using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistration.Models
{
    public class TokenManager: ITokenManager
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenManager(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GenerateToken(UserViewModel userViewModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[StaticKeys.JwtKey]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(_config[StaticKeys.JwtIssuer],
              _config[StaticKeys.JwtIssuer],
              new Claim[] {
                    new Claim(ClaimTypes.Name, userViewModel.Username)       // This username can be verified when the token is verified using a JWT debugger at https://jwt.io/ 
                 },
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        // Save token in cookies
        public void SaveToken(string token)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(5);
            options.HttpOnly = true;
            _httpContextAccessor.HttpContext.Response.Cookies.Append(StaticKeys.JwtTokenCookie, token, options);
        }
    }
}
