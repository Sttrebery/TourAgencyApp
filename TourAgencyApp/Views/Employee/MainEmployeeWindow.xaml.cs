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

namespace TourAgencyApp.Views.Employee
{
    /// <summary>
    /// Логика взаимодействия для MainEmployeeWindow.xaml
    /// </summary>
    public partial class MainEmployeeWindow : Window
    {
        public MainEmployeeWindow()
        {
            InitializeComponent();
            Title = "Туристическое агенство \"Вокруг света\"";
        }
    }
}
