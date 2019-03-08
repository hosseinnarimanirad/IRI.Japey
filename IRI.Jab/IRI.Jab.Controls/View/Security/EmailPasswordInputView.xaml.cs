using IRI.Jab.Common.Model.Security;
using IRI.Ket.Common.Helpers;
using IRI.Msh.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IRI.Jab.Controls.View.Security
{
    /// <summary>
    /// Interaction logic for EmailPasswordInputView.xaml
    /// </summary>
    public partial class EmailPasswordInputView : SecurityInputUserControl, IUserEmailPassword
    {
        public EmailPasswordInputView()
        {
            InitializeComponent();
        }

        public SecureString Password => this.key.SecurePassword;

        public string UserNameOrEmail { get => EmailAddress; set => EmailAddress = value; }

        public bool IsValidEmail()
        {
            return NetHelper.IsValidEmail(UserNameOrEmail);
        }

        public void ClearInputValues()
        {
            this.key.Clear();

            this.EmailAddress = string.Empty;
        }

        public string GetPasswordText()
        {
            return SecureStringHelper.GetString(Password);
        }

        public string EmailAddress
        {
            get { return (string)GetValue(EmailAddressProperty); }
            set { SetValue(EmailAddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UsreName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmailAddressProperty =
            DependencyProperty.Register(nameof(EmailAddress), typeof(string), typeof(EmailPasswordInputView), new PropertyMetadata(string.Empty));

    }
}
