using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Security
{
    public interface INewPassword : ISecurityBase
    {
        SecureString NewPassword { get; }

        SecureString ConfirmPassword { get; }

        bool IsNewPasswordValid();
    }
}
