using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Security.Jwt
{
    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secretKey)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }

    }
}
