﻿using IRI.Jab.Common.Model.Security;
using IRI.Jab.Common.Presenters.Security;
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
    /// Interaction logic for ChangeUserPasswordView.xaml
    /// </summary>
    public partial class ChangeUserPasswordView : SecurityInputUserControl, IChangePassword
    {
        public ChangeUserPasswordView()
        {
            InitializeComponent();
        }


        public SecureString Password => this.key.SecurePassword;

        public SecureString NewPassword => this.newPassword.SecurePassword;

        public SecureString ConfirmPassword => this.confirmNewPassword.SecurePassword;

        //same code exist in EmailSignUpView & UserNameSignUpView
        public bool IsNewPasswordValid()
        {
            return NewPassword != null && NewPassword.Length > 0 && !SecureStringHelper.SecureStringEqual(this.NewPassword, this.ConfirmPassword);
        }

        public void ClearInputValues()
        {
            this.key.Clear();

            this.newPassword.Clear();

            this.confirmNewPassword.Clear();
        }

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


        //public Brush InputBorderBrush
        //{
        //    get { return (Brush)GetValue(InputBorderBrushProperty); }
        //    set { SetValue(InputBorderBrushProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for InputBorderBrush.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty InputBorderBrushProperty =
        //    DependencyProperty.Register(nameof(InputBorderBrush), typeof(Brush), typeof(ChangeUserPasswordView), new PropertyMetadata(null));


        //public Thickness InputBorderThickness
        //{
        //    get { return (Thickness)GetValue(InputBorderThicknessProperty); }
        //    set { SetValue(InputBorderThicknessProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for InputBorderThickness.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty InputBorderThicknessProperty =
        //    DependencyProperty.Register(nameof(InputBorderThickness), typeof(Thickness), typeof(ChangeUserPasswordView), new PropertyMetadata(new Thickness()));

    }
}
