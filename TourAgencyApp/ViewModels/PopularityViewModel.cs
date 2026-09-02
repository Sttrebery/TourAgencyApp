using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourAgencyApp.Models;
using TourAgencyApp.Services;

namespace TourAgencyApp.ViewModels
{
    public class PopularityViewModel : INotifyPropertyChanged
    {
        #region поля
        private readonly DataService _dataService;
        private string _topCountryName;
        private int _topCountryCount;

        private ObservableCollection<Tour> _topTour;
        private ObservableCollection<Hotel> _topHotel;
        #endregion

        #region Properties
        public string TopCountryName
        {
            get { return _topCountryName; }
            set { _topCountryName = value; OnPropertyChanged(); }
        }
        public int TopCountryCount
        {
            get { return _topCountryCount; }
            set { _topCountryCount = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tour> TopTour
        {
            get { return _topTour; }
            set { _topTour = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Hotel> TopHotel
        {
            get { return _topHotel; }
            set { _topHotel = value; OnPropertyChanged(); }
        }
        #endregion

        public ICommand LoadDataCommand { get; }

        public PopularityViewModel(DataService dataService)
        {
            _dataService = dataService;
            LoadDataCommand = new RelayCommand(LoadData);
        }

        public void LoadData()
        {
            //_dataService.GetTopCountry(out _topCountryName,out _topCountryCount);
            OnPropertyChanged(nameof(TopCountryName));
            OnPropertyChanged(nameof(TopCountryCount));

            //TopTour = new ObservableCollection<Tour>(_dataService.GetTopActualTour());
            //TopHotel = new ObservableCollection<Hotel>(_dataService.GetTopHotel());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
