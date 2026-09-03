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
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;
using System.Drawing;
using System.IO;

namespace TourAgencyApp.ViewModels
{
    public class EmployeeTourViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        
        //поля для просмотра
        private ObservableCollection<Tour> _actualTours;   //Актуальных туров
        private ObservableCollection<Tour> _archiveTours; //Архивных туров //todo: мб сделать вычисляемым

        //поля при добавлении тура
        private string _tourName;
        private decimal _tourCost;
        private DateTime? _startdate;
        private DateTime? _enddate;
        private string _description;
        private int _touristMaxCount;
        private IEnumerable<Employee> _all_employees;
        private Employee _selected_employee;
        private int _countryId;
        private IEnumerable<Country> _countries;
        private IEnumerable<Hotel> _allHotels;
        private Hotel _hotel;
        private string _hotelName;
        private int _transportId;
        private IEnumerable<Transport> _transports;
        private List<Photo> _images = new();

        //поля для удаления тура (помещения в архив)
        private int? _del_tour;

        #region props
        public List<Photo> Images
        {
            get { return _images; }
            set { _images = value; OnPropertyChanged(); }
        }

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
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
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

        public IEnumerable<string> AllHotels
        {
            get => _allHotels.Select(a => a.Name).Distinct();
        }
        public string Hotel
        {
            get => _hotelName;
            set { _hotelName = value; OnPropertyChanged(); }
        }
        public int TransportID
        {
            get => _transportId;
            set { _transportId = value; OnPropertyChanged(); }
        }
        public IEnumerable<Transport> Transports
        {
            get => _transports;
            set { _transports = value; OnPropertyChanged(); }
        }
        public int CountryID
        {
            get => _countryId;
            set { _countryId = value; OnPropertyChanged(); }
        }
        public IEnumerable<Country> Countries
        {
            get => _countries;
            set { _countries = value; OnPropertyChanged(); }
        }
        #endregion

        #region Delete Tour

        public int? TourToDelete
        {
            get => _del_tour;
            set { _del_tour = value; OnPropertyChanged(); }
        }

        #endregion

        #endregion

        public ICommand AddTourCommand { get; set; }
        public ICommand AddPhotoCommand { get; set; }
        public ICommand ClearTourCommand { get; set; } //очистка формы в окне добавления
        public ICommand DeleteTourCommand { get; set; }
        
        public EmployeeTourViewModel(DataService d)
        {
            _dataService = d;
            LoadData();
            ClearTour();

            //команды
            //todo: вот тут мб убрать load data - работать с локальными изменениями, а потом отправлять изменения в бд
            AddTourCommand = new RelayCommand(() => { AddTour(); LoadData(); } );
            ClearTourCommand = new RelayCommand(ClearTour);
            DeleteTourCommand = new RelayCommand( () => { DeleteTour(); LoadData(); });
            AddPhotoCommand = new RelayCommand(AddPhoto);
        }

        private void AddPhoto()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            List<Photo> photosToAdd = new List<Photo>();

            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                //todo: вынести загрузку всех фото в асинхронный метод
                foreach (string filename in fileDialog.FileNames)
                {
                    byte[] image_data = null;
                    try
                    {
                        using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            image_data = new byte[fs.Length];
                            fs.Read(image_data, 0, (int)fs.Length);
                        }

                        if(image_data != null)
                        {
                            photosToAdd.Add(new Photo() { PhotoValue = image_data });
                        }
                    }
                    catch
                    {
                        MessageBox.Show($"Не удалось загрузить файл: {filename}", "Ошибка");
                    }
                }
                Images = photosToAdd;
            }

        }

        //загрузка данных из бд
        public void LoadData()
        {
            //todo: переделать потом мб
            ActualTours = new ObservableCollection<Tour>(_dataService.GetTours()) ?? new ObservableCollection<Tour>();
            ArchiveTours = new ObservableCollection<Tour>( _dataService.GetArchiveTours()) ?? new ObservableCollection<Tour>();

            _all_employees = _dataService.GetAllEmployees() ?? new List<Employee>();
            _allHotels = _dataService.GetHotels() ?? new List<Hotel>();

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
            //Country = string.Empty;
            Hotel = string.Empty;
        }

        private void DeleteTour()
        {
            try
            {
                //todo:
                //_dataService.DeleteActualTour(_del_tour.Value);
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
