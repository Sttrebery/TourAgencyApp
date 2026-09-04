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
            LoadPhotosCommand = new RelayCommand(LoadPhoto);
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
            //OnPropertyChanged();
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

        private void LoadPhoto()
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

                        if (image_data != null)
                        {
                            photosToAdd.Add(new Photo() { PhotoValue = image_data });
                        }
                    }
                    catch
                    {
                        MessageBox.Show($"Не удалось загрузить файл: {filename}", "Ошибка");
                    }
                }
                Images = new ObservableCollection<Photo>(photosToAdd);
            }
        }

        //private byte[] CreateCopy()
        //{
        //    try
        //    {
        //        System.Drawing.Image img = Bitmap.FromFile(_filename);
        //        int maxWidth = 300, maxHeight = 300;
        //        double ratioX = (double)maxWidth / img.Width;
        //        double ratioY = (double)maxHeight / img.Height;
        //        double ratio = Math.Min(ratioX, ratioY);
        //        int newWidth = (int)(img.Width * ratio);
        //        int newHeight = (int)(img.Height * ratio);

        //        Image im = new Bitmap(newWidth, newHeight);
        //        Graphics g = Graphics.FromImage(im);
        //        g.DrawImage(img, 0, 0, newWidth, newHeight);
        //        MemoryStream ms = new MemoryStream();
        //        im.Save(ms, ImageFormat.Jpeg);
        //        ms.Flush();
        //        ms.Seek(0, SeekOrigin.Begin);
        //        BinaryReader br = new BinaryReader(ms);
        //        byte[] buf = br.ReadBytes((int)ms.Length);
        //        return buf;
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Error CreateCopy");
        //        return null;
        //    }
        //}

        public void LoadHotels()
        {
            if(_isChanged)
            {
                var hotelsFromDB = _dataService.GetHotels(); //todo: async
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
