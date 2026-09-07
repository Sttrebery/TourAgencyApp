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
    public class ActualToursViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Tour> _tours;

        public ObservableCollection<Tour> Tours
        {
            get => _tours;
            set { _tours = value; OnPropertyChanged(); }
        }

        public ICommand LoadToursCommand { get; }
        public ICommand ShowDetailCommand { get; }

        public ActualToursViewModel(DataService dataService)
        {
            _dataService = dataService;
            LoadToursCommand = new AsyncRelayCommand(LoadTours);
            ShowDetailCommand = new RelayCommand<object>(ShowDetail);
            Tours = new(_dataService.GetActualTours());
        }

        private void ShowDetail(object? param)
        {
            var tour = param as Tour;
            if(tour != null)
            {
                var view = new TourDetailView() { DataContext = new TourDetailViewModel(tour) };
                view.Show();
            }
            else
            {
                MessageBox.Show("Неизвестная ошибка!");
            }
        }

        public async Task LoadTours()
        {
            var toursFromDb = await _dataService.GetActualToursAsync();
            Tours = new ObservableCollection<Tour>(toursFromDb);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
