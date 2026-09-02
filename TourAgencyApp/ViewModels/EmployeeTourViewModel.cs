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
using Xceed.Wpf.Toolkit;

namespace TourAgencyApp.ViewModels
{
    public class EmployeeTourViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        
        //поля для просмотра
        private ObservableCollection<Tour> _actualTours;   //Актуальных туров
        private ObservableCollection<Tour> _archiveTours; //Архивных туров

        //поля при добавлении тура
        private string _tourName;
        private decimal _tourCost;
        private DateTime? _startdate;
        private DateTime? _enddate;
        private int _touristMaxCount;
        private IEnumerable<Employee> _all_employees;
        private Employee _selected_employee;
        private string _country;
        private IEnumerable<Hotel> _allHotels;
        private Hotel _hotel;
        private string _hotelName;

        //поля для удаления тура (помещения в архив)
        private Tour _del_tour;

        #region props
        public ObservableCollection<Tour> ActualTours
        {
            get => _actualTours;
            set { _actualTours = value; OnPropertyChanged(); }
        }

        public IEnumerable<string> ActualToursNames
        {
            get => _actualTours.Select(a => a.Name);
        }
        public ObservableCollection<Tour> ArchiveTours
        {
            get => _archiveTours;
            set { _archiveTours = value; OnPropertyChanged(); }
        }

        #region Add Tour
        public string TourName
        {
            get => _tourName;
            set { _tourName = value; OnPropertyChanged(); }
        }
        public decimal TourCost
        {
            get => _tourCost;
            set { _tourCost = value; OnPropertyChanged(); }
        }
        public DateTime? StartDate
        {
            get => _startdate;
            set { _startdate = value; OnPropertyChanged(); }
        }
        public DateTime? EndDate
        {
            get => _enddate;
            set { _enddate = value; OnPropertyChanged(); }
        }
        public int TouristMaxCount
        {
            get => _touristMaxCount;
            set { _touristMaxCount = value; OnPropertyChanged(); }
        }
        public IEnumerable<int> AllEmployees
        {
            get { return _all_employees.Select(a => a.ID); }
        }

        public string SelectedEmployeeFullName
        {
            get => _all_employees.Where(e => e.ID == SelectedEmployee).Select(a => $"{a.Surname} {a.Name} {a.Patronimyc}").FirstOrDefault();
        }

        public int SelectedEmployee
        {
            get => _selected_employee?.ID ?? -1;
            set
            {
                _selected_employee = _all_employees.First(e => e.ID == value);
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedEmployeeFullName));
            }
        }
        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }
        public IEnumerable<string> AllHotels
        {
            get => _allHotels.Select(a => a.Name).Distinct();
        }
        public string Hotel
        {
            get => _hotelName;
            set { _hotelName = value; OnPropertyChanged(); }
        }
        #endregion

        #region Delete Tour

        public string TourToDelete
        {
            get => _del_tour.Name;
            set { _del_tour = _actualTours.First(a => a.Name == value); OnPropertyChanged(); }
        }

        #endregion

        #endregion

        public ICommand AddTourCommand { get; set; }
        public ICommand ClearTourCommand { get; set; } //очистка формы в окне добавления
        public ICommand DeleteTourCommand { get; set; }

        public EmployeeTourViewModel(DataService d)
        {
            _dataService = d;
            LoadData();
            ClearTour();
            _del_tour = _actualTours.FirstOrDefault();

            //команды
            AddTourCommand = new RelayCommand(() => { AddTour(); LoadData(); } );
            ClearTourCommand = new RelayCommand(ClearTour);
            DeleteTourCommand = new RelayCommand( () => { DeleteTour(); LoadData(); });

        }

        //загрузка данных из бд
        public void LoadData()
        {
            ActualTours = new ObservableCollection<Tour>( _dataService.GetTours());
            ArchiveTours = new ObservableCollection<Tour>( _dataService.GetArchiveTours());

            _all_employees = _dataService.GetAllEmployees();
            _allHotels = _dataService.GetHotels();

            OnPropertyChanged();
        }

        //
        private bool CanExecuteAdd()
        {
            //todo: пределать ЧТОБЫ ВСЕ БЫЛО ЗАПОЛНЕНО
            if(TourName == string.Empty || TourCost <= 0 ||
                !StartDate.HasValue || !EndDate.HasValue|| TouristMaxCount <= 0 || StartDate >= EndDate)
            {
                return false;
            }    
            return true;
        }

        private void AddTour()
        {
            if(!CanExecuteAdd())
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return;
            }

            Tour tours = new Tour();
            tours.Name = TourName;
            tours.Cost = TourCost;
            tours.StartDate = StartDate.Value;
            tours.EndDate = EndDate.Value;
            tours.MaxTouristCount = TouristMaxCount;
            tours.ResponsibleEmployeeID = SelectedEmployee;
            //tours.HotelID = //добалвение отеля
                //добавление страны
                //добавление транспорта


            try
            {
                tours = _dataService.AddTour(tours);
                MessageBox.Show("Тур успешно добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось добавить тур\n{ex.Message}");
            }
        }

        private void ClearTour()
        {
            TourName = string.Empty;
            TourCost = 0;
            StartDate = null;
            EndDate = null;
            TouristMaxCount = 0;
            SelectedEmployee = _all_employees.Select(e => e.ID).First();
            Country = string.Empty;
            Hotel = string.Empty;
        }

        private void DeleteTour()
        {
            try
            {
                _dataService.DeleteActualTour(_del_tour);
                MessageBox.Show("Тур был помещен в архив");
            }
            catch
            {
                MessageBox.Show("Не удалось удалить тур.");
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
