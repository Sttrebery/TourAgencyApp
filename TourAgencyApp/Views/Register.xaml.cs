using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TourAgencyApp.ViewModels;

namespace TourAgencyApp.Views
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            Title = "Туристическое агенство \"Вокруг света\"";
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rb_employee_Checked(object sender, RoutedEventArgs e)
        {
            panel_employee_info.Visibility = Visibility.Visible;
        }

        private void rb_employee_Unchecked(object sender, RoutedEventArgs e)
        {
            panel_employee_info.Visibility = Visibility.Collapsed;
        }

        private void psw_1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                (DataContext as RegisterViewModel).MyPassword = psw_1.Password;
        }

        private void psw_2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                (DataContext as RegisterViewModel).RepeatedPassword = psw_2.Password;
        }

        private void registerWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                RegisterViewModel r = (RegisterViewModel)this.DataContext;
                r.RegisterEvent += () => { if (r._isValidated) this.Close(); };
            }      
        }

    }
}
