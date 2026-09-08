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
    public class MyToursViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private readonly int _userId;
        private ObservableCollection<Tour> _tours;

        public ObservableCollection<Tour> MyTours
        {
            get => _tours;
            set { _tours = value; OnPropertyChanged(); }
        }

        public ICommand LoadToursCommand { get; }
        public ICommand ShowDetailCommand { get; }

        public MyToursViewModel(DataService dataService, int userId)
        {
            _dataService = dataService;
            _userId = userId;
            MyTours = new ObservableCollection<Tour>(_dataService.GetClientTours(_userId));
            LoadToursCommand = new AsyncRelayCommand(LoadTours);
            ShowDetailCommand = new AsyncRelayCommand<object>(ShowDetail);
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

        public async Task LoadTours()
        {
            try
            {
                var result = await _dataService.GetClientToursAsync(_userId);
                MyTours = new ObservableCollection<Tour>(result);
            }
            catch
            {
                if(MyTours == null)
                    MyTours = new();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
