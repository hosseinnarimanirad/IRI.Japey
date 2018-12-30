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
            this.Password.Clear(); 
        }
    }
}
