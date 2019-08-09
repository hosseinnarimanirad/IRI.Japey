using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace IRI.Sta.Security.Jwt
{
    public sealed class JwtTokenBuilder
    {


        private SecurityKey _securityKey = null;
        private string _subject = "";
        private string _issuer = "";
        private string _audience = "";
        private List<Claim> _claims;
        private int _expiryInMinutes = 5;

        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this._securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            this._subject = subject;
            return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this._issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            this._audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            if (this._claims == null)
            {
                this._claims = new List<Claim>();
            }

            this._claims.Add(new Claim(type, value));

            return this;
        }

        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            if (this._claims == null)
            {
                this._claims = new List<Claim>();
            }

            //this.claims = this.claims.Union(claims).ToDictionary(k => k.Key, k => k.Value);
            if (claims?.Count > 0)
            {
                foreach (var item in claims)
                {
                    this._claims.Add(new Claim(item.Key, item.Value));
                }
            }

            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInMinutes)
        {
            this._expiryInMinutes = expiryInMinutes;

            return this;
        }

        public JwtTokenBuilder()
        {
            this._claims = new List<Claim>();
        }

        public JwtToken Build()
        {
            EnsureArguments();

            var claims = this._claims.Union(new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, this._subject), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) });

            var token = new JwtSecurityToken(
                              issuer: this._issuer,
                              audience: this._audience,
                              claims: _claims,
                              expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                              signingCredentials: new SigningCredentials(this._securityKey, SecurityAlgorithms.HmacSha256));

            return new JwtToken(token);
        }

        #region " private "

        private void EnsureArguments()
        {
            if (this._securityKey == null)
                throw new ArgumentNullException("Security Key");

            if (string.IsNullOrEmpty(this._subject))
                throw new ArgumentNullException("Subject");

            if (string.IsNullOrEmpty(this._issuer))
                throw new ArgumentNullException("Issuer");

            if (string.IsNullOrEmpty(this._audience))
                throw new ArgumentNullException("Audience");
        }

        #endregion
    }

}
