using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IRI.Ket.Security.Jwt
{
    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
         
    }
}
