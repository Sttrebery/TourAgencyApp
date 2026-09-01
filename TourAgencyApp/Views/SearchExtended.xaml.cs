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
    /// Логика взаимодействия для SearchExtended.xaml
    /// </summary>
    public partial class SearchExtended : Window
    {
        public SearchExtended()
        {
            InitializeComponent();
            Title = "Туристическое агенство \"Вокруг света\"";
            
        }

        public SearchExtended(SearchViewModel s) : this()
        {
            s.ExtSearchEvent += OnSearch;
            this.DataContext = s;
        }

        private void OnSearch()
        {
            this.Close();
        }
    }
}
