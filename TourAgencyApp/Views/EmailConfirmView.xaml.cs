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

namespace TourAgencyApp.Views
{
    /// <summary>
    /// Логика взаимодействия для EmailConfirmView.xaml
    /// </summary>
    public partial class EmailConfirmView : Window
    {
        public EmailConfirmView()
        {
            InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
