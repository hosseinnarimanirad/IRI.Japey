using IRI.Jab.Common.Model.Security;
using IRI.Jab.Common.Presenters.Security;
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
    /// Interaction logic for ChangeUserPasswordView.xaml
    /// </summary>
    public partial class ChangeUserPasswordView : UserControl, IChangePassword
    {
        public ChangeUserPasswordView()
        {
            InitializeComponent();
        }


        public SecureString Password => this.key.SecurePassword;

        public SecureString NewPassword => this.newPassword.SecurePassword;

        public SecureString ConfirmPassword => this.confirmNewPassword.SecurePassword;



        public bool IsUserNameShown
        {
            get { return (bool)GetValue(IsUserNameShownProperty); }
            set { SetValue(IsUserNameShownProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUserNameShown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUserNameShownProperty =
            DependencyProperty.Register(nameof(IsUserNameShown), typeof(bool), typeof(ChangeUserPasswordView), new PropertyMetadata(true));




        public bool CanUserChangeUserName
        {
            get { return (bool)GetValue(CanUserChangeUserNameProperty); }
            set { SetValue(CanUserChangeUserNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanUserChangeUserName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanUserChangeUserNameProperty =
            DependencyProperty.Register(nameof(CanUserChangeUserName), typeof(bool), typeof(ChangeUserPasswordView), new PropertyMetadata(true));
         
    }
}
