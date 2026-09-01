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
using TourAgencyApp.Views.Employee.Pages;

namespace TourAgencyApp.ViewModels
{
    public class ClientHotelsViewModels : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Hotel> _hotels;

        public ObservableCollection<Hotel> Hotels
        {
            get => _hotels;
            set { _hotels = value; OnPropertyChanged(); }
        }

        public ClientHotelsViewModels(DataService dataService)
        {
            _dataService = dataService;
        }

        public void LoadHotels()
        {
            var hotelsFromDB = _dataService.GetHotels();
            Hotels = new ObservableCollection<Hotel>(hotelsFromDB);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
