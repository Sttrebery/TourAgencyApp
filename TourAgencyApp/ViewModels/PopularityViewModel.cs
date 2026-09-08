using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourAgencyApp.Models;
using TourAgencyApp.Services;
using TourAgencyApp.Views;

namespace TourAgencyApp.ViewModels
{
    public class PopularityViewModel : INotifyPropertyChanged
    {
        #region поля
        private readonly DataService _dataService;

        private string _topCountryName;
        private int? _topCountryCount;

        private ObservableCollection<Tour> _topTours;
        #endregion

        #region Properties


        public string TopCountryName
        {
            get { return _topCountryName; }
            set { _topCountryName = value; OnPropertyChanged(); }
        }
        public int? TopCountryCount
        {
            get { return _topCountryCount; }
            set { _topCountryCount = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tour> TopTours
        {
            get { return _topTours; }
            set { _topTours = value; OnPropertyChanged(); }
        }
        #endregion

        public ICommand LoadDataCommand { get; }
        public ICommand ShowDetailCommand { get; }

        public PopularityViewModel(DataService dataService)
        {
            _dataService = dataService;
            (TopCountryName, TopCountryCount) = _dataService.GetTopCountry();
            TopTours = new ObservableCollection<Tour>(_dataService.GetTopActualTour().Take(10));

            ShowDetailCommand = new AsyncRelayCommand<object>(ShowDetail);
            LoadDataCommand = new AsyncRelayCommand(LoadData);
        }

        private async Task ShowDetail(object? param)
        {
            var tour = param as Tour;

            try
            {
                if (tour != null)
                {
                    var full_info = await _dataService.GetFullTourInfoByIDAsync(tour.ID);
                    var view = new TourDetailView() { DataContext = new TourDetailViewModel(full_info) };
                    view.Show();
                }
            }
            catch
            {
                MessageBox.Show("Неизвестная ошибка!");
            }

        }

        public async Task LoadData()
        {
            (TopCountryName, TopCountryCount) = await _dataService.GetTopCountryAsync();

            TopTours = new ObservableCollection<Tour>(await _dataService.GetTopActualTourAsync());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
