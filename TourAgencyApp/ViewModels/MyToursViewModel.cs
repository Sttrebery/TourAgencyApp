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
using TourAgencyApp.Data;
using TourAgencyApp.Models;

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

        public MyToursViewModel(DataService dataService, Tourist client)
        {
            _dataService = dataService;
            _client = client;
            LoadToursCommand = new RelayCommand(LoadTours);
            LoadTours();
        }

        public void LoadTours()
        {
            var result = _dataService.GetClientTours(_client.Name,_client.Surname,_client.Patronimyc);
            MyTours = new ObservableCollection<Tour>(result);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
