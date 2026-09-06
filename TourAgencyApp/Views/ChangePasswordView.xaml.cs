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
    /// Логика взаимодействия для ChangePasswordView.xaml
    /// </summary>
    public partial class ChangePasswordView : Window
    {
        public ChangePasswordView()
        {
            InitializeComponent();
        }

        private void psw_1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                (DataContext as ChangePasswordViewModel).NewPassword = psw_1.Password;
        }

        private void psw_2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                (DataContext as ChangePasswordViewModel).RepeatedPassword = psw_2.Password;
        }

        private void btn_cofirm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
