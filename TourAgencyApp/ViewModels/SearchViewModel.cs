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
    public class SearchViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Tour> _allTours;
        private readonly DataService _dataService;
        private string _searchText;
        private ObservableCollection<Tour> _searchResults;
        private ObservableCollection<Country> _countries;
        private ObservableCollection<Transport> _transports;
        DateTime? _date1;
        DateTime? _date2;
        int? _countryId;
        int? _transportId;

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value.Trim(); OnPropertyChanged(); }
        }

        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set { _countries = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Transport> Transports
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

        public int? CountryId
        {
            get => _countryId;
            set { _countryId = value; OnPropertyChanged(); }
        }

        public int? TransportId
        {
            get => _transportId;
            set { _transportId = value; OnPropertyChanged(); }
        }

        public event Action ExtSearchEvent;
        public ICommand MiniSearchCommand { get; } //команда для поиска по названию в основном окне
        public ICommand ExtSearchCommand { get; }
        public ICommand ClearTextCommand { get; }
        public ICommand ClearExtendedCommand { get; }
        public ICommand ShowDetailCommand { get; }
        public SearchViewModel(DataService dataService)
        {
            _dataService = dataService;
            _allTours = _dataService.GetActualTours();
            Countries = new(_dataService.GetAllCountries());
            Transports = new(_dataService.GetAllTransports());
            SearchResults = new ObservableCollection<Tour>();

            MiniSearchCommand = new RelayCommand(ExecuteTextSearch, CanExecuteSearch);
            ClearTextCommand = new RelayCommand(() => SearchText = string.Empty);
            ClearExtendedCommand = new RelayCommand(ClearExtended);
            ShowDetailCommand = new RelayCommand<object>(ShowDetail);
            ExtSearchEvent += ExtSearch;
            ExtSearchCommand = new RelayCommand(() => ExtSearchEvent());
        }

        private void ShowDetail(object? param)
        {
            var tour = param as Tour;
            if (tour != null)
            {
                var view = new TourDetailView() { DataContext = new TourDetailViewModel(tour) };
                view.Show();
            }
            else
            {
                MessageBox.Show("Неизвестная ошибка!");
            }
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

            var results = _allTours.Where(t => t.Name.StartsWith(SearchText, StringComparison.InvariantCultureIgnoreCase));
            SearchResults = new ObservableCollection<Tour>(results);
        }

        private void ExtSearch()
        {
            IEnumerable<Tour> result = null;
            ToursEqualityComparer comparer = new ToursEqualityComparer();

            if (Date1.HasValue)
            {
                result = GetToursByDate(Date1.Value, Date2 ?? DateTime.MaxValue);
            }

            if (CountryId.HasValue)
            {
                var temp = _allTours.Where(t=> t.Country!.ID == CountryId);
                result = result == null ? temp : result.Intersect(temp, comparer);
            }

            if (TransportId.HasValue)
            {
                var temp = _allTours.Where(t => t.TransoprtType!.ID == TransportId);
                result = result == null ? temp : result.Intersect(temp, comparer);
            }

            if (result == null) SearchResults = new ObservableCollection<Tour>();
            else
            {
                SearchResults = new ObservableCollection<Tour>(result);
            }
        }

        public IEnumerable<Tour> GetToursByDate(DateTime date1, DateTime date2)
        {
            return _allTours.Where(t => t.StartDate >= date1 && t.StartDate <= date2).ToList();
        }

        private void ClearExtended()
        {
            Date1 = null;
            Date2 = null;
            TransportId = null;
            CountryId = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
