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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourAgencyApp;
using TourAgencyApp.ViewModels;

namespace TourAgencyApp.Views.Employee.Pages
{
    /// <summary>
    /// Логика взаимодействия для HotelsPage.xaml
    /// </summary>
    public partial class HotelsPage : Page
    {
        public HotelsPage()
        {
            InitializeComponent();
        }

        private void cmb_hotel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DataContext != null)
            {
                (DataContext as EmployeeHotelsViewModel).UpdateData();
            }
        }

        private void tab_AddHotel_Selected(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                (DataContext as EmployeeHotelsViewModel).ClearForm();
            }
        }

        private void tab_hotels_list_Selected(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                (DataContext as EmployeeHotelsViewModel).LoadHotels();
            }
        }
    }
}
