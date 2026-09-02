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

        public ActualToursViewModel(DataService dataService)
        {
            _dataService = dataService;
            LoadToursCommand = new RelayCommand(LoadTours);
            LoadTours();
        }

        public void LoadTours()
        {
            var toursFromDb = _dataService.GetTours();
            Tours = new ObservableCollection<Tour>(toursFromDb);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
