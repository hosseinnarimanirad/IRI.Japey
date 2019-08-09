using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IRI.Sta.Security.Jwt
{
    public sealed class JwtToken
    {
        private JwtSecurityToken _token;

        internal JwtToken(JwtSecurityToken token)
        {
            this._token = token;
        }

        public DateTime ValidTo => _token.ValidTo;

        public string Value => new JwtSecurityTokenHandler().WriteToken(this._token);

        public static JwtToken Create(string secretKey, Dictionary<string, string> claims, int expiryInMinutes, string subject, string issuer, string audience)
        {
            var result = new JwtTokenBuilder()
                            .AddSecurityKey(JwtSecurityKey.Create(secretKey))
                            .AddSubject(subject)
                            .AddIssuer(issuer)
                            .AddClaims(claims)
                            .AddAudience(audience)
                            .AddExpiry(expiryInMinutes);

            return result.Build(); 
        }

    }
}
