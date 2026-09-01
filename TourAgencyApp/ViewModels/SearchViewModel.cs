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
using TourAgencyApp.Data;

namespace TourAgencyApp.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Tour> _allTours;
        private readonly DataService _dataService;
        private string _searchText;
        private ObservableCollection<Tour> _searchResults;
        private IEnumerable<string> _countries;
        private IEnumerable<string> _transports;
        DateTime? _date1;
        DateTime? _date2;
        string _country;
        string _transport;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value; OnPropertyChanged();
            }
        }

        public IEnumerable<string> Countries
        {
            get => _countries;
            set { _countries = value; OnPropertyChanged(); }
        }

        public IEnumerable<string> Transports
        {
            get => _transports;
            set { _transports = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tour> SearchResults
        {
            get => _searchResults;
            set { _searchResults = value; OnPropertyChanged(); }
        }
        public DateTime? Date1
        {
            get => _date1;
            set { _date1 = value; OnPropertyChanged(); }
        }
        public DateTime? Date2
        {
            get => _date2;
            set { _date2 = value; OnPropertyChanged(); }
        }

        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }

        public string Transport
        {
            get => _transport;
            set { _transport = value; OnPropertyChanged(); }
        }

        public event Action ExtSearchEvent;
        public ICommand MiniSearchCommand { get; } //команда для поиска по названию в основном окне
        public ICommand ExtSearchCommand { get; }
        public ICommand ClearTextCommand { get; }
        public ICommand ClearExtendedCommand { get; }

        public SearchViewModel(DataService dataService)
        {
            _dataService = dataService;
            _allTours = _dataService.GetTours();
            Countries = _dataService.GetAllCountries().Select(c => c.Name_).Distinct().ToList();
            Transports = _dataService.GetAllTransports().Select(t => t.Name_).Distinct().ToList();
            SearchResults = new ObservableCollection<Tour>();

            MiniSearchCommand = new RelayCommand(ExecuteTextSearch, CanExecuteSearch);
            ClearTextCommand = new RelayCommand(() => SearchText = string.Empty);
            ClearExtendedCommand = new RelayCommand(ClearExtended);
             
            ExtSearchEvent += ExtSearch;
            ExtSearchCommand = new RelayCommand(() => ExtSearchEvent());
        }


        private bool CanExecuteSearch()
        {
            return !string.IsNullOrWhiteSpace(SearchText) && SearchText.Length >= 1;
        }

        public void ExecuteTextSearch()
        {
            if (!CanExecuteSearch())
            {
                MessageBox.Show("Введите хотя бы 1 символ!");
                return;
            }

            var results = _allTours.Where(t => t.NameTour.StartsWith(SearchText, StringComparison.InvariantCultureIgnoreCase));
            SearchResults = new ObservableCollection<Tour>(results);
        }

        private void ExtSearch()
        {
            IEnumerable<Tour> result = null;
            ToursEqualityComparer comparer = new ToursEqualityComparer();

            if (Date1.HasValue)
            {
                result = _dataService.GetToursByDate(Date1.Value, Date2 ?? DateTime.MaxValue);
            }

            if (!string.IsNullOrEmpty(Country))
            {
                var temp = _dataService.GetToursByCountry(Country);
                result = result == null ? temp : result.Intersect(temp, comparer);
            }

            if (!string.IsNullOrEmpty(Transport))
            {
                var temp = _dataService.GetToursByTransport(Transport);
                result = result == null ? temp : result.Intersect(temp, comparer);
            }

            if (result == null) SearchResults = new ObservableCollection<Tour>();
            else SearchResults = new ObservableCollection<Tour>(result);
        }

        private void ClearExtended()
        {
            Date1 = null;
            Date2 = null;
            Transport = string.Empty;
            Country = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
