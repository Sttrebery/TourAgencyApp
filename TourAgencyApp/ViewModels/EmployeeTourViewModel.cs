using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourAgencyApp.Models;
using TourAgencyApp.Services;
using TourAgencyApp.Views;
using Xceed.Wpf.Toolkit;

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
        private ObservableCollection<Photo> _images = new();

        private int? _selectedEmployeeID;
        private ObservableCollection<Employee> _all_employees;

        private int? _countryId;
        private ObservableCollection<Country> _countries;

        //private Hotel _hotel;
        private int? _hotelId;
        private ObservableCollection<Hotel> _allHotels;

        private int? _transportId;
        private ObservableCollection<Transport> _transports;

        //редактирование тура
        private ICollectionView _toursView; //todo:

        //поля для удаления тура (помещения в архив)
        private int? tourToDeleteId;

        #region props

        #region Property to View Data
        public ObservableCollection<Photo> Images
        {
            get { return _images; }
            set { _images = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tour> ActualTours
        {
            get => _actualTours;
            set { _actualTours = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Tour> ArchiveTours
        {
            get => _archiveTours;
            set { _archiveTours = value; OnPropertyChanged(); }
        }
        #endregion

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
        public ObservableCollection<Employee> AllEmployees
        {
            get => _all_employees;
            set { _all_employees = value; OnPropertyChanged(); }
        }

        public int? SelectedEmployeeID
        {
            get => _selectedEmployeeID;
            set { _selectedEmployeeID = value; OnPropertyChanged(); }
        }

        public int? HotelID
        {
            get => _hotelId;
            set { _hotelId = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Hotel> AllHotels
        {
            get => _allHotels;
            set { _allHotels = value; OnPropertyChanged(); }
        }

        public int? TransportID
        {
            get => _transportId;
            set { _transportId = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Transport> Transports
        {
            get => _transports;
            set { _transports = value; OnPropertyChanged(); }
        }
        public int? CountryID
        {
            get => _countryId;
            set { _countryId = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set { _countries = value; OnPropertyChanged(); }
        }
        #endregion

        #region Delete Tour

        public int? TourToDelete
        {
            get => tourToDeleteId;
            set { tourToDeleteId = value; OnPropertyChanged(); }
        }

        #endregion

        #endregion

        public ICommand AddTourCommand { get; set; }
        public ICommand AddPhotoCommand { get; set; }
        public ICommand ClearTourCommand { get; set; } //очистка формы в окне добавления
        public ICommand DeleteTourCommand { get; set; }
        public ICommand ShowPhotoCommand { get; set; }

        public EmployeeTourViewModel(DataService d)
        {
            _dataService = d;

            //команды
            //todo: вот тут мб убрать load data - работать с локальными изменениями, а потом отправлять изменения в бд
            AddTourCommand = new AsyncRelayCommand(AddTour);
            ClearTourCommand = new RelayCommand(ClearTour);
            DeleteTourCommand = new RelayCommand(DeleteTour);
            AddPhotoCommand = new AsyncRelayCommand(AddPhoto);
            ShowPhotoCommand = new RelayCommand<object>(parameter => ShowPhoto(parameter));
        }

        private void ShowPhoto(object? param)
        {
            var tour = param as Tour;
            if (tour != null && tour.Photos != null && tour.Photos.Any())
            {
                var view = new PhotoViewer() { ImagesCollection = new ObservableCollection<Photo>(tour.Photos) };
                view.Show();
            }
        }

        private async Task AddPhoto()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                // Выполняем загрузку асинхронно
                var photos = await ImageService.LoadPhotosAsync(fileDialog.FileNames) ?? new();
                //Images = new ObservableCollection<Photo>(photos); надо чтобы Images не был null
                foreach (var photo in photos)
                {
                    Images.Add(photo);
                }
            }
        }

        //загрузка данных из бд
        public async Task LoadDataFromDB()
        {
            //todo: переделать потом мб
            ActualTours = new ObservableCollection<Tour>(_dataService.GetTours()) ?? new();
            ArchiveTours = new ObservableCollection<Tour>( ActualTours.Where(t=> t.IsConducted == true) ) ?? new();

            AllEmployees = new ObservableCollection<Employee>(_dataService.GetAllEmployees()) ?? new();
            AllHotels = new ObservableCollection<Hotel>(await _dataService.GetHotelsAsync()) ?? new();
            Countries = new ObservableCollection<Country>(await _dataService.GetAllCountriesAsync()) ?? new();
            Transports = new ObservableCollection<Transport>(await _dataService.GetAllTransportsAsync()) ?? new();
            //OnPropertyChanged();
        }

        private bool CanExecute()
        {
            if( (TourName == string.Empty || string.IsNullOrWhiteSpace(TourName) ) || TourCost <= 0 ||
                !StartDate.HasValue || !EndDate.HasValue || TouristMaxCount <= 0 || StartDate > EndDate ||
                (Description == string.Empty || string.IsNullOrWhiteSpace(Description)) ||
                !SelectedEmployeeID.HasValue || !HotelID.HasValue || !CountryID.HasValue || !TransportID.HasValue)
            {
                return false;
            }    
            return true;
        }

        private async Task AddTour()
        {
            if(!CanExecute())
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return;
            }

            Tour tours = new Tour()
            {
                Name = TourName,
                Cost = TourCost,
                StartDate = StartDate!.Value, 
                EndDate = EndDate!.Value,
                Description = Description,
                MaxTouristCount = TouristMaxCount,
                ResponsibleEmployeeID = SelectedEmployeeID!.Value,
                CountyID = CountryID!.Value,
                TransoprtTypeID = TransportID!.Value,
                HotelID = HotelID!.Value,
                Photos = Images.ToList()
            };

            try
            {
                ActualTours.Add(await _dataService.AddTourAsync(tours));
                MessageBox.Show("Тур успешно добавлен");
                ClearTour();
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

            SelectedEmployeeID = AllEmployees.Select(e => e.ID).FirstOrDefault();
            CountryID = Countries.Select(c => c.ID).FirstOrDefault();
            TransportID = Transports.Select(t => t.ID).FirstOrDefault();
            HotelID = AllHotels.Select(h => h.ID).FirstOrDefault();
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
