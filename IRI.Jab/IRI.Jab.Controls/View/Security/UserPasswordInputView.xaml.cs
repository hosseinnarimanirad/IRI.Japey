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
    /// Interaction logic for UserPasswordInputView.xaml
    /// </summary>
    public partial class UserPasswordInputView : SecurityInputUserControl, IUserEmailPassword
    {
        public UserPasswordInputView()
        {
            InitializeComponent();
        }

        public SecureString Password => this.key.SecurePassword;
         
        public bool IsValidEmail()
        {
            return NetHelper.IsValidEmail(UserNameOrEmail);
        }
 
        public void ClearInputValues()
        {
            this.key.Clear();

            this.UserNameOrEmail = string.Empty;
        }

        public string GetPasswordText()
        {
            return SecureStringHelper.GetString(Password);
        }

        public string UserNameWatermark
        {
            get { return (string)GetValue(UserNameWatermarkProperty); }
            set { SetValue(UserNameWatermarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserNameWatermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserNameWatermarkProperty =
            DependencyProperty.Register(nameof(UserNameWatermark), typeof(string), typeof(UserPasswordInputView), new PropertyMetadata("نام کاربری"));




        public string UserNameOrEmail
        {
            get { return (string)GetValue(UsreNameProperty); }
            set { SetValue(UsreNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UsreName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsreNameProperty =
            DependencyProperty.Register(nameof(UserNameOrEmail), typeof(string), typeof(UserPasswordInputView), new PropertyMetadata(string.Empty));



        //public Brush InputBorderBrush
        //{
        //    get { return (Brush)GetValue(InputBorderBrushProperty); }
        //    set { SetValue(InputBorderBrushProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for InputBorderBrush.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty InputBorderBrushProperty =
        //    DependencyProperty.Register(nameof(InputBorderBrush), typeof(Brush), typeof(UserPasswordInputView), new PropertyMetadata(null));


        //public Thickness InputBorderThickness
        //{
        //    get { return (Thickness)GetValue(InputBorderThicknessProperty); }
        //    set { SetValue(InputBorderThicknessProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for InputBorderThickness.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty InputBorderThicknessProperty =
        //    DependencyProperty.Register(nameof(InputBorderThickness), typeof(Thickness), typeof(UserPasswordInputView), new PropertyMetadata(new Thickness()));

    }
}
