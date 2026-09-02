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
    public class RegisterViewModel : INotifyPropertyChanged
    {
        #region vars
        private DataService _dataService;
        private Tourist Client { get; set; }
        private Employee Employee { get; set; }

        private string _validation = string.Empty;
        private bool _isClient = true;
        private bool _isEmployee = false;
        public bool _isValidated = false;

        //для всех
        private string _username = string.Empty;
        private string _myPassword = string.Empty;
        private string _repeatedPassword = string.Empty;

        private string _surname = string.Empty;
        private string _firstName = string.Empty;
        private string _patronimyc = string.Empty;
        private string _number = string.Empty;
        private string _email = string.Empty;

        //для сотрудника
        private string _position = string.Empty;
        #endregion

        #region props
        public string Validation
        {
            get => _validation;
            set {  _validation = value; OnPropertyChanged(); }
        }

        public bool IsClient
        {
            get => _isClient;
            set { _isClient = value; OnPropertyChanged(); }
        }

        public bool IsEmployee
        {
            get => _isEmployee;
            set { _isEmployee = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string MyPassword
        {
            get => _myPassword;
            set { _myPassword = value; OnPropertyChanged(); }
        }

        public string RepeatedPassword
        {
            get => _repeatedPassword;
            set { _repeatedPassword = value; OnPropertyChanged(); }
        }

        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(); }
        }

        public string Firstname
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string Patronimyc
        {
            get => _patronimyc;
            set { _patronimyc = value; OnPropertyChanged(); }
        }

        public string Number
        {
            get => _number;
            set { _number = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(); }
        }
        #endregion

        public event Action RegisterEvent;
        public ICommand RegisterCommand { get; set; } 

        public RegisterViewModel()
        {
            _dataService = new DataService();

            RegisterEvent = Register;
            RegisterCommand = new RelayCommand(() => RegisterEvent());
        }

        //todo:
        private bool IsFormFilled()
        {
            bool isFilled = true;

            if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(MyPassword) || string.IsNullOrEmpty(RepeatedPassword)
                || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Patronimyc) 
                || string.IsNullOrEmpty(Number) || string.IsNullOrEmpty(Email))
            {
                isFilled = false;
            }

            if (_isEmployee && (string.IsNullOrEmpty(Position)) )
            {
                isFilled = false;
            }
            return isFilled;
        }

        private bool CheckPassword()
        {
            return RepeatedPassword == MyPassword;
        }

        private bool CanRegister()
        {
            //if (!IsFormFilled())
            //{
            //    Validation = "Форма не заполнена полностью";
            //    return false;
            //}

            //if(!CheckPassword())
            //{
            //    Validation = "Пароли не совпадают";
            //    return false;
            //}

            //var founded = _dataService.GetUserByUsername(Username);
            //    if (founded != null)
            //    {
            //        Validation = "Пользователь уже зарегистрирован в базе";
            //        return false;
            //    }

            return true;
        }

        private void Register()
        {
            if (!CanRegister())
            {
                MessageBox.Show(Validation);
                return;
            }

            Validation = "Ошибка! Не удалось зарегистрировать пользователя";

            User user = new User()
            {
                Username = Username,
                Password = MyPassword,
                Email = Email,
                //Role = 
            };

            if(IsClient)
            {
                user.Client = new Tourist()
                {
                    UserID = user.ID,
                    Name = Firstname,
                    Surname = Surname,
                    Patronimyc = Patronimyc,
                    PhoneNumber = Number
                };
            }
            else if (IsEmployee)
            {
                user.Employee = new Employee()
                {
                    UserID = user.ID,
                    Name = Firstname,
                    Surname = Surname,
                    Patronimyc = Patronimyc,
                    PhoneNumber = Number,
                    //Position = PositionEnum
                };
            }

            //if(_dataService.AddUser(user))
            //{
            //            Validation = "Регистрация прошла успешно";
            //            _isValidated = true;
            //}

            //    MessageBox.Show(Validation);

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
