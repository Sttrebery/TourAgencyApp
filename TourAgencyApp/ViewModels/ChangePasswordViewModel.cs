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
using TourAgencyApp.Models;
using TourAgencyApp.Services;

namespace TourAgencyApp.ViewModels
{
    class ChangePasswordViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private readonly string _email;
        private readonly int _user_id;
        private string _newPassword;
        private string _repeatedNewPassword;

        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        public string RepeatedPassword
        {
            get => _repeatedNewPassword;
            set { _repeatedNewPassword = value; OnPropertyChanged(); }
        }

        public ICommand ConfirmCommand { get; set; }

        public ChangePasswordViewModel(DataService dataService, string email, int user_id)
        {
            _dataService = dataService;
            _email = email;
            _user_id = user_id;

            ConfirmCommand = new AsyncRelayCommand(Confirm);
        }

        private bool Check()
        {
            return NewPassword == RepeatedPassword;
        }

        private async Task Confirm()
        {
            if(!Check())
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка");
                return;
            }

            bool _isConfirmed = await EmailConfirmService.EmailConfirmAsync(_email);

            if(_isConfirmed)
            {
                try
                {
                    await _dataService.ChangePasswordAsync(_user_id, HashService.HashPassword(_newPassword));
                    MessageBox.Show("Пароль изменен.");
                }
                catch
                {
                    MessageBox.Show("Не удалось изменить пароль.", "Ошибка");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
