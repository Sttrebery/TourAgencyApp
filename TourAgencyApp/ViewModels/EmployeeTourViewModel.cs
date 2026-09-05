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
        private ObservableCollection<Tour> _archiveTours; //Архивных туров 
        private ICollectionView _toursView;
        private ICollectionView _archiveToursView;

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
        private int? tourToEditId;

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

        public ICollectionView ToursView
        {
            get => _toursView;
            set { _toursView = value; OnPropertyChanged(); }
        }
        public ICollectionView ArchiveToursView
        {
            get => _archiveToursView;
            set { _archiveToursView = value; OnPropertyChanged(); }
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

        public int? TourToDeleteID
        {
            get => tourToDeleteId;
            set { tourToDeleteId = value; OnPropertyChanged(); }
        }

        public int? TourToEditID
        {
            get => tourToEditId;
            set { tourToEditId = value; OnPropertyChanged(); }
        }

        #endregion

        public ICommand AddTourCommand { get; set; }
        public ICommand AddPhotoCommand { get; set; }
        public ICommand ClearTourCommand { get; set; } //очистка формы в окне добавления
        public ICommand DeleteTourCommand { get; set; }
        public ICommand ShowPhotoCommand { get; set; }
        public ICommand DeletePhotoCommand { get; set; }
        public ICommand EditTourCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public EmployeeTourViewModel(DataService d)
        {
            _dataService = d;

            AddTourCommand = new AsyncRelayCommand(AddTour);
            ClearTourCommand = new RelayCommand(ClearTour);
            DeleteTourCommand = new AsyncRelayCommand(DeleteTour);
            AddPhotoCommand = new AsyncRelayCommand(AddPhoto);
            ShowPhotoCommand = new RelayCommand<object>(parameter => ShowPhoto(parameter));
            DeletePhotoCommand = new RelayCommand<object>(parameter => DeletePhoto(parameter));
            EditTourCommand = new AsyncRelayCommand(EditTour);
            CancelCommand = new RelayCommand(UpdateUI);
        }

        private void ShowPhoto(object? param)
        {
            var tour = param as Tour;
            if (tour != null && tour.Photos != null && tour.Photos.Any())
            {
                var view = new PhotoViewer() { ImagesCollection = new ObservableCollection<Photo>(tour.Photos) };
                view.Show();
            }
            else
            {
                MessageBox.Show("Не найдено ни одной фотографии.");
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
                foreach (var photo in photos)
                {
                    Images.Add(photo);
                }
            }
        }

        private void DeletePhoto(object? param)
        {
            var p = param as Photo;
            if (p != null)
            {
                Images.Remove(p);
            }
        }

        //загрузка данных из бд
        public async Task LoadDataFromDB()
        {
            var tours = await _dataService.GetToursAsync();
            ActualTours = new ObservableCollection<Tour>(tours.Where(t=>t.IsConducted == false));
            ArchiveTours = new ObservableCollection<Tour>(tours.Where(t=> t.IsConducted == true));

            AllEmployees = new ObservableCollection<Employee>(await _dataService.GetAllEmployeesAsync());
            AllHotels = new ObservableCollection<Hotel>(await _dataService.GetHotelsAsync());
            Countries = new ObservableCollection<Country>(await _dataService.GetAllCountriesAsync());
            Transports = new ObservableCollection<Transport>(await _dataService.GetAllTransportsAsync());
        }

        private bool CanExecute()
        {
            if( (TourName == string.Empty || string.IsNullOrWhiteSpace(TourName) ) || TourCost <= 0 ||
                !StartDate.HasValue || !EndDate.HasValue || TouristMaxCount <= 0 ||
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
            else if (StartDate > EndDate || StartDate < DateTime.Today)
            {
                MessageBox.Show("Дата начала не может быть позднее даты окончания или текущего дня!");
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
                MessageBox.Show("Тур успешно добавлен.");
                ClearTour();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось добавить тур:\n{ex.Message}");
            }
        }

        private async Task EditTour()
        {
            if (!CanExecute())
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return;
            }
            else if (StartDate > EndDate || StartDate < DateTime.Today)
            {
                MessageBox.Show("Дата начала не может быть позднее даты окончания или текущего дня!");
                return;
            }

            Tour edited = new Tour()
            {
                ID = TourToEditID!.Value,
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
                await _dataService.EditTourAsync(TourToEditID!.Value, edited);

                var selected = ActualTours.FirstOrDefault(h => h.ID == TourToEditID);
                ActualTours.Remove(selected);
                ActualTours.Add(edited);

                MessageBox.Show("Изменения сохранены");
                ClearTour();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось внести изменения: {ex.Message}");
            }

        }

        public void UpdateUI()
        {
            var selected = ActualTours.FirstOrDefault(h => h.ID == TourToEditID);
            if (selected != null)
            {
                TourName = selected.Name;
                Description = selected.Description;
                TourCost = selected.Cost;
                StartDate = selected.StartDate;
                EndDate = selected.EndDate;
                TouristMaxCount = selected.MaxTouristCount;
                Images = new(selected.Photos);

                SelectedEmployeeID = selected.ResponsibleEmployeeID;
                CountryID = selected.CountyID;
                TransportID = selected.TransoprtTypeID;
                HotelID = selected.HotelID;
            }
            else
            {
                ClearTour();
            }
        }

        public void ClearTour()
        {
            TourName = string.Empty;
            Description = string.Empty;
            TourCost = 0;
            StartDate = null;
            EndDate = null;
            TouristMaxCount = 0;
            Images.Clear();

            SelectedEmployeeID = null;
            CountryID = null;
            TransportID = null;
            HotelID = null;

            TourToDeleteID = null;
            TourToEditID = null;
        }

        private async Task DeleteTour()
        {
            var tour_to_delete = ActualTours.FirstOrDefault(t=>t.ID == TourToDeleteID!.Value);
            if (tour_to_delete == null)
                return;

            try
            {
                await _dataService.DeleteTourAsync(TourToDeleteID!.Value);

                ActualTours.Remove(tour_to_delete);
                tour_to_delete.IsConducted = true;
                ArchiveTours.Add(tour_to_delete);

                MessageBox.Show("Тур был помещен в архив");
                TourToDeleteID = null;
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
