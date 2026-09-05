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

namespace TourAgencyApp.ViewModels
{
    public class ClientMainViewModel : INotifyPropertyChanged
    {
        private Tourist _client;
        private string _name;
        private string _surname;
        private string _patronimyc;
        private Tour _selectedTour;
        private IEnumerable<Tour> _allTours;

        #region Properties
        public string SelectedTour
        {
            get => _selectedTour?.Name;
            set
            {
                _selectedTour = _allTours.First(t => t.Name == value);
                OnPropertyChanged();
            }
        }
        public IEnumerable<string> AllTours
        {
            get { return _allTours.Select(a => a.Name); }
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
        public ClientProfileViewModel ProfileVM { get; set; }
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
            var dataService = new DataService();
            _allTours = dataService.GetTours();

            // viewModels
            ActualToursVM = new ActualToursViewModel(dataService);
            ProfileVM = new ClientProfileViewModel(Client);
            PopularityVM = new PopularityViewModel(dataService);
            SearchVM = new SearchViewModel(dataService);
            MyToursVM = new MyToursViewModel(dataService, Client);

            // команды 
            SearchEvent += SearchVM.ExecuteTextSearch;
            SearchEvent += () => CurrentPage = SearchVM;

            SearchVM.ExtSearchEvent += () => CurrentPage = SearchVM;

            NavigateToActualCommand = new RelayCommand(() => { ActualToursVM.LoadTours(); CurrentPage = ActualToursVM; });
            NavigateToPopularityCommand = new RelayCommand(() => { PopularityVM.LoadData(); CurrentPage = PopularityVM; });
            NavigateToMyToursCommand = new RelayCommand(() => { MyToursVM.LoadTours(); CurrentPage = MyToursVM; });
            NavigateToProfileCommand = new RelayCommand(() => CurrentPage = ProfileVM);
            SearchCommand = new RelayCommand(() => SearchEvent());
            ExtendedSearchCommand = new RelayCommand(ExtendedSearch);
            SignUpForTourCommand = new RelayCommand(SignUpForTour);

            //стартовая страница
            CurrentPage = ActualToursVM;
        }

        public async void SignUpForTour()
        {
            var data = new DataService();
            if(await data.SignUpFoTour(Client, _selectedTour))
            {
                MessageBox.Show($"Вы были записаны на тур {SelectedTour}");
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
