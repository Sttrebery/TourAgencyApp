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
using TourAgencyApp.Views.Employee.Pages;

namespace TourAgencyApp.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Tour> _topTour;
        private ObservableCollection<Tour> _antiTopTour;
        private ObservableCollection<Tour> _archiveTopTour;
        private IEnumerable<Tour> tours;
        private IEnumerable<Tour> arch_tours;

        public ObservableCollection<Tour> TopTour
        {
            get { return _topTour; }
            set { _topTour = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Tour> AntiTopTour
        {
            get { return _antiTopTour; }
            set { _antiTopTour = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Tour> ArchiveTopTour
        {
            get { return _archiveTopTour; }
            set { _archiveTopTour = value; OnPropertyChanged(); }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand ShowDetailCommand { get; }

        public StatsViewModel(DataService dataService)
        {
            _dataService = dataService;
            tours =_dataService.GetTopActualTour();
            arch_tours = _dataService.GetTopArchiveTour();

            UpdateData();
            
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
            tours = await _dataService.GetTopActualTourAsync();
            arch_tours = await _dataService.GetTopArchiveTourAsync();

            UpdateData();
        }

        private void UpdateData()
        {
            if (tours.Count() == 0)
            {
                TopTour = new ObservableCollection<Tour>();
                AntiTopTour = new ObservableCollection<Tour>();
            }
            else
            {
                TopTour = new ObservableCollection<Tour>([tours.Last()]);
                AntiTopTour = new ObservableCollection<Tour>([tours.First()]);
            }

            if (arch_tours.Count() == 0)
            {
                ArchiveTopTour = new ObservableCollection<Tour>();
            }
            else
            {
                ArchiveTopTour = new([arch_tours.Last()]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
