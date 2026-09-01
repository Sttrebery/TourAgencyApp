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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public bool LoginSuccesful { get; set; } = false;
        public Login()
        {
            InitializeComponent();
            Title = "Туристическое агенство \"Вокруг света\"";
            LoginViewModel model = new LoginViewModel();
            model.SignEvent += OnLogin;
            DataContext = model;
        }

        private void OnLogin()
        {
            LoginSuccesful = ((LoginViewModel)DataContext).IsValidated;
            if(LoginSuccesful) this.Close();
        }

        void CorrectView()
        {
            if (Width > 1500 || this.WindowState == WindowState.Maximized)
            {
                panel.Margin = new Thickness(100);
            }
            else panel.Margin = new Thickness(0);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CorrectView();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            CorrectView();
        }

        private void psw_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                (DataContext as LoginViewModel).MyPassword = psw.Password;
        }

    }
}
