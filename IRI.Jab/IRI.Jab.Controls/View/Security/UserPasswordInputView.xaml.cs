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
    /// Interaction logic for UserPasswordInputView.xaml
    /// </summary>
    public partial class UserPasswordInputView : UserControl, IHavePassword
    {
        public UserPasswordInputView()
        {
            InitializeComponent();
        }

        public SecureString Password => this.key.SecurePassword;

        public void ClearInputValues()
        {
            this.key.Clear(); 
        }



        public string UserNameWatermark
        {
            get { return (string)GetValue(UserNameWatermarkProperty); }
            set { SetValue(UserNameWatermarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserNameWatermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserNameWatermarkProperty =
            DependencyProperty.Register(nameof(UserNameWatermark), typeof(string), typeof(UserPasswordInputView), new PropertyMetadata("نام کاربری"));


    }
}
