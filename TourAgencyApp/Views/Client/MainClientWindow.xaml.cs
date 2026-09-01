using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace TourAgencyApp.Views.Client
{
    /// <summary>
    /// Логика взаимодействия для MainClientWindow.xaml
    /// </summary>
    public partial class MainClientWindow : Window
    {
        public MainClientWindow()
        {
            InitializeComponent();
            Title = "Туристическое агенство \"Вокруг света\"";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (cmb_tour.SelectedIndex == -1)
                btn_tourSign.IsEnabled = false;
           else
                btn_tourSign.IsEnabled = true;
        }
    }
}
