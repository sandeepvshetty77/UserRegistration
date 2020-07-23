using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserRegistration.Models
{
    public static class StaticKeys
    {
        public const string JwtIssuer = "Jwt:Issuer";
        public const string JwtKey = "Jwt:Key";
        public const string UsersDbConnectionString = "UsersDbConnection";
        public const string JwtTokenCookie = "JWTtoken";
    }
}
