using System;
using System.Collections.Generic;
using System.Linq; 
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class SecurityHelper
    {
        public static GenericPrincipal Create(string userName)
        {
            return new GenericPrincipal(new GenericIdentity(userName), new string[0]);
        }

       
    }
}
