using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourAgencyApp.Data;
using TourAgencyApp.Models;

namespace TourAgencyApp.ViewModels
{
    public class EmployeeHotelsViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Hotel> _hotels;

        //поля для добавления/редактирования
        private string _filename;
        private string _name;
        private string _information;
        private string _location;
        private byte[] _photo;

        //поля для удаления
        private string _selected_hotel;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Information
        {
            get => _information;
            set { _information = value; OnPropertyChanged(); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        public byte[] Photo
        {
            get => _photo;
            set { _photo = value; OnPropertyChanged(); }
        }

        
        public ObservableCollection<Hotel> Hotels
        {
            get => _hotels;
            set { _hotels = value; OnPropertyChanged(); }
        }

        public IEnumerable<string> HotelsToSelect
        {
            get => Hotels.Select(h => h.Name_);
        }

        public string SelectedHotel
        {
            get => _selected_hotel;
            set { _selected_hotel = value; OnPropertyChanged(); }
        }


        public ICommand AddHotelCommand { get; set; }
        public ICommand ClearFormCommand { get; set; }
        public ICommand LoadPhotoCommand { get; set; }
        public ICommand DeleteHotelCommand { get; set; }
        public ICommand SaveHotelCommand { get; set; }

        public EmployeeHotelsViewModel(DataService dataService)
        {
            _dataService = dataService;

            AddHotelCommand = new RelayCommand(AddHotel);
            ClearFormCommand = new RelayCommand(ClearForm);
            LoadPhotoCommand = new RelayCommand(LoadPhoto);
            SaveHotelCommand = new RelayCommand(EditHotel);
            DeleteHotelCommand = new RelayCommand(DeleteHotel);
        }
        
        public void ClearForm()
        {
            Name = string.Empty;
            Information = string.Empty;
            Location = string.Empty;
            Photo = null;
        }

        private bool CanExecute()
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Information) || string.IsNullOrEmpty(Location))
            {
                return false;
            }
            return true;
        }

        private void AddHotel()
        {
            if(!CanExecute())
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            Hotel to_add = new Hotel() { Name_ = Name, Location_ = Location, Information_ = Information, ImageHotel = Photo };
            try
            {
                _dataService.AddNewHotel(to_add);
                MessageBox.Show("Отель успешно добавлен");
            }
            catch
            {
                MessageBox.Show("Не удалось добавить отель");
            }
        }

        private void EditHotel()
        {
            if (!CanExecute())
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            Hotel edited = new Hotel() { Name_ = Name, Location_ = Location, Information_ = Information, ImageHotel = Photo };
            try
            {
                _dataService.EditHotel(SelectedHotel, edited);
                MessageBox.Show("Изменения сохранены");
            }
            catch
            {
                MessageBox.Show("Не удалось внести изменения");
            }
        }

        private void DeleteHotel()
        {
            try
            {
                _dataService.DeleteHotel(SelectedHotel);
                MessageBox.Show("Отель был удален");
            }
            catch
            {
                MessageBox.Show("Не удалось удалить отель");
            }
        }

        //очистка данных при SelectionChanged
        public void UpdateData()
        {
            var selected = Hotels.First(h => h.Name_ == SelectedHotel);
            Name = selected.Name_;
            Information = selected.Information_;
            Location = selected.Location_;
            Photo = selected.ImageHotel;
        }

        private void LoadPhoto()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Графические файлы|*.jpeg; *.jpg; *.png; *.bmp; *.gif";
            ofd.FileName = "";
            if (ofd.ShowDialog() == true)
            {
                _filename = ofd.FileName;
                byte[] bytes = CreateCopy();
                Photo = bytes;
            }
        }

        private byte[] CreateCopy()
        {
            try
            {
                System.Drawing.Image img = Bitmap.FromFile(_filename);
                int maxWidth = 300, maxHeight = 300;
                double ratioX = (double)maxWidth / img.Width;
                double ratioY = (double)maxHeight / img.Height;
                double ratio = Math.Min(ratioX, ratioY);
                int newWidth = (int)(img.Width * ratio);
                int newHeight = (int)(img.Height * ratio);

                Image im = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(im);
                g.DrawImage(img, 0, 0, newWidth, newHeight);
                MemoryStream ms = new MemoryStream();
                im.Save(ms, ImageFormat.Jpeg);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(ms);
                byte[] buf = br.ReadBytes((int)ms.Length);
                return buf;
            }
            catch (Exception)
            {
                MessageBox.Show("Error CreateCopy");
                return null;
            }
        }

        public void LoadHotels()
        {
            var hotelsFromDB = _dataService.GetHotels();
            Hotels = new ObservableCollection<Hotel>(hotelsFromDB);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
