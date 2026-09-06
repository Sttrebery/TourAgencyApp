using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourAgencyApp.Models;
using TourAgencyApp.Services;
using TourAgencyApp.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TourAgencyApp.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private Profile _profile;

        private int _userID;
        private string _name;
        private string _surname;
        private string _patronimyc;
        private string _phone;
        private string _email;
        private byte[]? _photo; 

        public Profile Profile
        {
            get => _profile;
            set { _profile = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value.Trim(); OnPropertyChanged();
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value.Trim(); OnPropertyChanged();
            }
        }
        public string Patronymic
        {
            get => _patronimyc;
            set
            {
                _patronimyc = value.Trim(); OnPropertyChanged();
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value; OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value.Trim(); OnPropertyChanged();
            }
        }
        public byte[]? Photo
        {
            get => _photo;
            set { _photo = value; OnPropertyChanged(); }
        }

        public Visibility UserPhoto
        {
            get => Photo == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public Visibility DefaultPhoto
        {
            get => Photo != null? Visibility.Collapsed : Visibility.Visible;
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand LoadPhotoCommand { get; }
        public ICommand ChangePasswordCommand { get; }


        //конструктор
        public ProfileViewModel(int user_id)
        {
            _dataService = new DataService();
            _userID = user_id;
            Profile = new Profile(user_id);
            Clear();

            SaveChangesCommand = new AsyncRelayCommand(SaveChangesAsync);
            LoadPhotoCommand = new AsyncRelayCommand(LoadPhoto);
            ChangePasswordCommand = new RelayCommand(ChangePassword);
            ClearCommand = new RelayCommand(Clear);
        }

        private async Task LoadPhoto()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                // Выполняем загрузку асинхронно
                Photo res = await ImageService.LoadPhotoAsync(fileDialog.FileName) ;
                
                if(res != null)
                {
                    Photo = res.PhotoValue;
                    OnPropertyChanged(nameof(UserPhoto));
                    OnPropertyChanged(nameof(DefaultPhoto));
                }
            }
        }

        private bool IsFormFilled()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Patronymic)
                || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Email))
            {
                return false;
            }
            else return true;
        }

        private void ChangePassword()
        {
            Window change_pass_window = new ChangePasswordView() { DataContext = new ChangePasswordViewModel(_dataService, Email, _userID) };
            change_pass_window.ShowDialog();
        }

        private void Clear()
        {
            Name = Profile.Name;
            Surname = Profile.Surname;
            Patronymic = Profile.Patronimyc;
            Phone = Profile.PhoneNumber;
            Email = Profile.Email;
            Photo = Profile.Photo;

            OnPropertyChanged(nameof(UserPhoto));
            OnPropertyChanged(nameof(DefaultPhoto));
        }

        private async Task SaveChangesAsync()
        {
            if (!IsFormFilled())
            {
                MessageBox.Show("Форма не заполнена полностью", "Ошибка");
                return;
            }

            if (!EmailConfirmService.ValidateEmail(Email))
            {
                MessageBox.Show("Некорректный адрес электронной почты", "Ошибка");
                return;
            }

            Profile NewProfile = new()
            {
                Name = Name,
                Surname = Surname,
                Patronimyc = Patronymic,
                PhoneNumber = Phone,
                Email = Email,
                Photo = Photo
            };

            try
            {
                await _dataService.EditProfileAsync(_userID, NewProfile);
                MessageBox.Show("Изменения внесены успешно");

                Profile = NewProfile;
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить изменения");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
