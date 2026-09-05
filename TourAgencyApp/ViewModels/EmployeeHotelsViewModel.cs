using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TourAgencyApp.Models;
using TourAgencyApp.Views;
using TourAgencyApp.Services;

namespace TourAgencyApp.ViewModels
{
    public class EmployeeHotelsViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Hotel> _hotels;

        private bool _isChanged = false;
        //поля для добавления
        private string _name;
        private string _desc;
        private string _address;
        private ObservableCollection<Photo> _images;

        //поля для редактирования
        private string _editName;
        private string _editDesc;
        private string _editAddress;
        private int _selectedHotelId;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _desc;
            set { _desc = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Photo> Images
        {
            get => _images;
            set { _images = value; OnPropertyChanged(); }
        }

        public string EditName
        {
            get => _editName;
            set { _editName = value; OnPropertyChanged(); }
        }

        public string EditDescription
        {
            get => _editDesc;
            set { _editDesc = value; OnPropertyChanged(); }
        }

        public string EditAddress
        {
            get => _editAddress;
            set { _editAddress = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Hotel> Hotels
        {
            get => _hotels;
            set { _hotels = value; OnPropertyChanged(); }
        }

        public int SelectedHotelID
        {
            get => _selectedHotelId;
            set { _selectedHotelId = value; OnPropertyChanged(); }
        }

        public ICommand AddHotelCommand { get; set; }
        public ICommand ClearFormCommand { get; set; }
        public ICommand LoadPhotosCommand { get; set; }
        public ICommand DeletePhotoCommand { get; set; }
        public ICommand SaveHotelCommand { get; set; }
        public ICommand ShowPhotoCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public EmployeeHotelsViewModel(DataService dataService)
        {
            _dataService = dataService;
            Hotels = new ObservableCollection<Hotel>(_dataService.GetHotels());

            AddHotelCommand = new AsyncRelayCommand(AddHotel);
            ClearFormCommand = new RelayCommand(ClearForm);
            LoadPhotosCommand = new AsyncRelayCommand(LoadPhoto);
            SaveHotelCommand = new AsyncRelayCommand(EditHotel);
            DeletePhotoCommand = new RelayCommand<object>(parameter => DeletePhoto(parameter));
            ShowPhotoCommand = new RelayCommand<object>(parameter => ShowPhoto(parameter));
            CancelCommand = new RelayCommand(UpdateData);
        }
        
        private void ShowPhoto(object? param)
        {
            var hotel = param as Hotel;
            if (hotel != null && hotel.Photos != null && hotel.Photos.Any())
            {
                var view = new PhotoViewer() { ImagesCollection = new ObservableCollection<Photo>(hotel.Photos)};
                view.Show();
            }
        }

        public void ClearForm()
        {
            Name = string.Empty;
            Description = string.Empty;
            Address = string.Empty;
            Images = null;
        }

        private bool CanExecute()
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(Address))
            {
                return false;
            }
            return true;
        }

        private async Task AddHotel()
        {
            if (!CanExecute())
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            Hotel to_add = new Hotel() { Name = Name, Description = Description, Address = Address, Photos = Images.ToList() };
            try
            {
                await _dataService.AddHotel(to_add);
                _isChanged = true;
                MessageBox.Show("Отель успешно добавлен");
                ClearForm();
            }
            catch
            {
                MessageBox.Show("Не удалось добавить отель");
            }
        }

        private async Task EditHotel()
        {
            if (!CanExecute())
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            Hotel edited = new Hotel() { Name = EditName, Description = EditDescription, Address = EditAddress, Photos = Images.ToList()};
            try
            {
                await _dataService.EditHotel(SelectedHotelID, edited);
                _isChanged = true;
                MessageBox.Show("Изменения сохранены");
            }
            catch
            {
                MessageBox.Show("Не удалось внести изменения");
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

        //изменение данных при SelectionChanged
        public void UpdateData()
        {
            var selected = Hotels.FirstOrDefault(h => h.ID == SelectedHotelID);
            if(selected != null)
            {
                EditName = selected.Name;
                EditDescription = selected.Description;
                EditAddress = selected.Address;
                Images = new(selected.Photos);
            }
            else
            {
                EditName = EditAddress = EditDescription = string.Empty;
                Images = null;
            }
        }

        private async Task LoadPhoto()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                // Выполняем загрузку асинхронно
                var photos = await ImageService.LoadPhotosAsync(fileDialog.FileNames) ?? new();
                Images = new ObservableCollection<Photo>(photos);
            }
        }

        public async Task LoadHotels()
        {
            if(_isChanged)
            {
                var hotelsFromDB = await _dataService.GetHotelsAsync();
                Hotels = new ObservableCollection<Hotel>(hotelsFromDB);
                _isChanged = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
