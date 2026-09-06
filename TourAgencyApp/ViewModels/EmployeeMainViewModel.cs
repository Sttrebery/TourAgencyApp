using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourAgencyApp.Services;
using TourAgencyApp.Models;
using System.Windows;

namespace TourAgencyApp.ViewModels
{
    public class EmployeeMainViewModel : INotifyPropertyChanged
    {
        private Employee _employee;

        public Employee Employee
        {
            get { return _employee; }
            set { _employee = value; OnPropertyChanged(); }
        }
        public string UserName
        {
            get => $"{Employee.Surname} {Employee.Name} {Employee.Patronimyc}";
        }

        // текущая выбранная страница
        private object _currentPage;
        public object CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }

        public Visibility IsRoot { get => Employee.Position != PositionEnum.Manager ? Visibility.Visible : Visibility.Collapsed; }

        #region Viewmodels
        public EmployeeTourViewModel ToursVM { get; set; }
        public EmployeeHotelsViewModel HotelsVM { get; set; }
        public StatsViewModel StatsVM { get; set; }
        public EmployeeCountriesViewModel CountriesVM { get; set; }
        public EmployeeTransportsViewModel TransportsVM { get; set; }
        #endregion

        //add: navigateToProfile
        public ICommand NavigateToToursCommand { get; }
        public ICommand NavigateToHotelsCommand { get; }
        public ICommand NavigateToStatsCommand { get; }
        public ICommand NavigateToCountriesCommand { get; }
        public ICommand NavigateToTransportsCommand { get; }

        public EmployeeMainViewModel(Employee e)
        {
            Employee = e;
            var dataService = new DataService();

            //viewmodels
            StatsVM = new StatsViewModel(dataService);
            ToursVM = new EmployeeTourViewModel(dataService);
            HotelsVM = new EmployeeHotelsViewModel(dataService);
            CountriesVM = new EmployeeCountriesViewModel(dataService);
            TransportsVM = new EmployeeTransportsViewModel(dataService);

            NavigateToStatsCommand = new RelayCommand(() => { StatsVM.LoadData(); CurrentPage = StatsVM; });
            NavigateToToursCommand = new RelayCommand(async () => {  await ToursVM.LoadDataFromDB(); CurrentPage =  ToursVM; });
            NavigateToHotelsCommand = new RelayCommand(() => {  CurrentPage =  HotelsVM; });
            NavigateToCountriesCommand = new RelayCommand(() =>  CurrentPage = CountriesVM);
            NavigateToTransportsCommand = new RelayCommand(() =>  CurrentPage = TransportsVM);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
