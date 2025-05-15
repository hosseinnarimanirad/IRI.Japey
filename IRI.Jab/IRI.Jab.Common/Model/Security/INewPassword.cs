using System.Security;

namespace IRI.Jab.Common.Model.Security;

public interface INewPassword : ISecurityBase
{
    SecureString NewPassword { get; }

    SecureString ConfirmPassword { get; }

    bool IsNewPasswordValid();

    string GetNewPasswordText();
}
