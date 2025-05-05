
using IRI.Sta.Common.Helpers;
using IRI.Sta.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Security
{
    public class SimpleUserEmailPasswordModel : IUserEmailPassword
    {
        //public string UserName { get; set; }

        //public string Password { get; set; }

        public SimpleUserEmailPasswordModel(SecureString password)
        {
            this.Password = password;
        }

        public string UserNameOrEmail { get; set; }

        public SecureString Password { get; private set; }

        public void ClearInputValues()
        {
            Password.Clear();

            UserNameOrEmail = string.Empty;
        }

        public string GetPasswordText()
        {
            return SecureStringHelper.GetString(Password);
        }

        public bool IsValidEmail()
        {
            return NetHelper.IsValidEmail(UserNameOrEmail);
        }
    }
}
