using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourAgencyApp.Services;
using TourAgencyApp.Models;
using TourAgencyApp.Views;
using System.Collections.ObjectModel;

namespace TourAgencyApp.ViewModels
{
    public class ClientMainViewModel : INotifyPropertyChanged
    {
        //todo: перепривязать combobox с турами
        private readonly DataService _dataService;
        private Tourist _client;
        private int _selectedTour;
        private ObservableCollection<Tour> _allTours;


        #region Properties
        public int SelectedTour
        {
            get => _selectedTour;
            set
            {
                _selectedTour = value; OnPropertyChanged();
            }
        }
        public ObservableCollection<Tour> AllTours
        {
            get => _allTours;
            set { _allTours = value; OnPropertyChanged(); }
        }
        public Tourist Client
        {
            get { return _client; }
            set { _client = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get { return Client.Name; }
        }
        public string Surname
        {
            get { return Client.Surname; }
        }
        public string Patronimyc
        {
            get { return Client.Patronimyc; }
        }
        public string UserName
        {
            get => $"{Surname} {Name} {Patronimyc}";
        }

        // текущая выбранная страница
        private object _currentPage;
        public object CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }
        #endregion

        #region viewmodels
        public SearchViewModel SearchVM { get; set; } //для нового окна
        public ActualToursViewModel ActualToursVM { get; set; }
        public PopularityViewModel PopularityVM { get; set; }
        public MyToursViewModel MyToursVM { get; set; }
        public ProfileViewModel ProfileVM { get; set; }
        #endregion

        #region Commands
        public event Action SearchEvent;
        public ICommand NavigateToActualCommand { get; }
        public ICommand NavigateToPopularityCommand { get; }
        public ICommand NavigateToMyToursCommand { get; }
        public ICommand NavigateToProfileCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ExtendedSearchCommand { get; }
        public ICommand SignUpForTourCommand { get; }
        #endregion

        //Конструктор
        public ClientMainViewModel(Tourist client)
        {
            Client = client; 
            _dataService = new DataService();
            _allTours = new(_dataService.GetActualTours());

            // viewModels
            ActualToursVM = new ActualToursViewModel(_dataService);
            ProfileVM = new ProfileViewModel(Client.UserID);
            PopularityVM = new PopularityViewModel(_dataService);
            SearchVM = new SearchViewModel(_dataService);
            MyToursVM = new MyToursViewModel(_dataService, Client.UserID);

            // команды 
            SearchEvent += SearchVM.ExecuteTextSearch;
            SearchEvent += () => CurrentPage = SearchVM;

            SearchVM.ExtSearchEvent += () => CurrentPage = SearchVM;

            NavigateToActualCommand = new RelayCommand(() => { CurrentPage = ActualToursVM; });
            NavigateToPopularityCommand = new RelayCommand(() => { CurrentPage = PopularityVM; });
            NavigateToMyToursCommand = new RelayCommand(() => { CurrentPage = MyToursVM; });
            NavigateToProfileCommand = new RelayCommand(() => CurrentPage = ProfileVM);
            SearchCommand = new RelayCommand(() => SearchEvent());
            ExtendedSearchCommand = new RelayCommand(ExtendedSearch);
            SignUpForTourCommand = new RelayCommand(SignUpForTour);

            //стартовая страница
            CurrentPage = ActualToursVM;
        }

        public async void SignUpForTour()
        {
            if(await _dataService.SignUpFoTour(Client.ID, SelectedTour))
            {
                MessageBox.Show($"Вы были записаны на тур {AllTours.First(f=>f.ID == SelectedTour).Name}");
            }
            else
            {
                MessageBox.Show("Не удалось записаться на тур\nВозможны вы уже записаны, или запись больше не доступна");
            }
        }

        public void ExtendedSearch()
        {
            var searchWindow = new SearchExtended(SearchVM);
            searchWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
