using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAgencyApp.Data;
using TourAgencyApp.Models;

namespace TourAgencyApp.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Tour> _topTour;
        private ObservableCollection<Tour> _antiTopTour;
        private string _topTourist;

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
        public string TopTourist
        {
            get { return _topTourist; }
            set { _topTourist = value; OnPropertyChanged(); }
        }

        public StatsViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        public void LoadData()
        {
            TopTour = new ObservableCollection<Tour>(_dataService.GetTopActualTour());
            AntiTopTour = new ObservableCollection<Tour>(_dataService.GetUnpopularTour());
            TopTourist = _dataService.GetActiveTourist();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
