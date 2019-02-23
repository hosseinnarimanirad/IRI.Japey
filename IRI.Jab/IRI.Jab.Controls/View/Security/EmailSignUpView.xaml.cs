using IRI.Jab.Common.Model.Security;
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
    /// Interaction logic for EmailSignUpView.xaml
    /// </summary>
    public partial class EmailSignUpView : UserControl, INewSimpleUserPass
    {
        public EmailSignUpView()
        {
            InitializeComponent();
        }

        public SecureString NewPassword => this.key.SecurePassword;

        public SecureString ConfirmPassword => this.confirmPassword.SecurePassword;

        public string EmailAddress
        {
            get { return (string)GetValue(UsreNameProperty); }
            set { SetValue(UsreNameProperty, value); }
        }

        public string UserName { get => EmailAddress; set => EmailAddress = value; }

        // Using a DependencyProperty as the backing store for UsreName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsreNameProperty =
            DependencyProperty.Register(nameof(EmailAddress), typeof(string), typeof(UserNameSignUpView), new PropertyMetadata(string.Empty));


        public void ClearInputValues()
        {
            this.key.Clear();

            this.confirmPassword.Clear();

            this.EmailAddress = string.Empty;
        }
    }
}
