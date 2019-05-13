using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace IRI.Ket.Security.Jwt
{
    public sealed class JwtToken
    {
        private JwtSecurityToken token;

        internal JwtToken(JwtSecurityToken token)
        {
            this.token = token;
        }

        public DateTime ValidTo => token.ValidTo;

        public string Value => new JwtSecurityTokenHandler().WriteToken(this.token);

        public static JwtToken Create(string secret, int expiryInMinute, string subject = "", string issuer = "", string audience = "", Dictionary<string, string> claims = null)
        {
            var result = new JwtTokenBuilder()
                               .AddSecurityKey(JwtSecurityKey.Create(secret))
                               .AddSubject(subject)
                               .AddIssuer(issuer)
                               .AddAudience(audience)
                               .AddClaim("MembershipId", "111")
                               .AddExpiry(expiryInMinute);

            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    result.AddClaim(claim.Key, claim.Value);
                }
            }

            return result.Build();
        }

    }
}
