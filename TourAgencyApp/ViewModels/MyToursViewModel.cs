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
    public class MyToursViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private readonly Tourist _client;
        private ObservableCollection<Tour> _tours;

        public ObservableCollection<Tour> MyTours
        {
            get => _tours;
            set { _tours = value; OnPropertyChanged(); }
        }

        public ICommand LoadToursCommand { get; }
        public ICommand ShowDetailCommand { get; }

        public MyToursViewModel(DataService dataService, Tourist client)
        {
            _dataService = dataService;
            _client = client;
            LoadToursCommand = new AsyncRelayCommand(LoadTours);
            LoadTours();
        }

        public async Task LoadTours()
        {
            //var result = _dataService.GetClientTours(_userId);
            //MyTours = new ObservableCollection<Tour>(result);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
