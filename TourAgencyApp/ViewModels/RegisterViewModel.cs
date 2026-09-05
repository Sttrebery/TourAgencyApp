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
using TourAgencyApp.Views;

namespace TourAgencyApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        #region vars
        private DataService _dataService;

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
        private PositionEnum? _position = null;
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

        public PositionEnum? Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(); }
        }

        public IEnumerable<PositionEnum> PositionEnumValues
        {
            get
            {
                return Enum.GetValues(typeof(PositionEnum)).Cast<PositionEnum>();
            }
        }
        #endregion

        public event Action RegisterEvent;
        public ICommand RegisterCommand { get; set; } 

        public RegisterViewModel()
        {
            _dataService = new DataService();

            RegisterEvent += async () => { await Register(); };
            RegisterCommand = new RelayCommand(() => RegisterEvent());
        }

        //проверка на заполнение полей формы
        private bool IsFormFilled()
        {
            bool isFilled = true;

            if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(MyPassword) || string.IsNullOrEmpty(RepeatedPassword)
                || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Patronimyc) 
                || string.IsNullOrEmpty(Number) || string.IsNullOrEmpty(Email))
            {
                isFilled = false;
            }

            if (_isEmployee && !Position.HasValue)
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
            if (!IsFormFilled())
            {
                Validation = "Форма не заполнена полностью";
                return false;
            }

            //todo: ChechEmail with Regex

            if (!CheckPassword())
            {
                Validation = "Пароли не совпадают";
                return false;
            }

            var founded = _dataService.GetUserByUsername(Username);
            if (founded != null)
            {
                Validation = "Пользователь уже зарегистрирован в базе";
                return false;
            }

            return true;
        }

        private async Task<bool> ConfirmEmail()
        {
            string code = GenerateCode();
            Window email_confirm = new EmailConfirmView() { DataContext = new EmailConfirmViewModel(code) };
            
            await MailService.SendConfirmationEmail(code, Email);
            
            email_confirm.ShowDialog();
            if((email_confirm.DataContext as EmailConfirmViewModel)!.Success)
            {
                return true;
            }
            else return false;
        }

        private string GenerateCode()
        {
            Random rand = new Random();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                builder.Append(rand.Next(0, 10).ToString());
            }
            return builder.ToString();
        }

        private async Task Register()
        {
            if (!CanRegister())
            {
                MessageBox.Show(Validation);
                return;
            }

            //код подтверждения с почты
            bool _isConfirmed = await ConfirmEmail();
            if (!_isConfirmed)
            {
                MessageBox.Show("Не удалось подтвердить почту", "Ошибка");
                return;
            }

            //создание нового пользователя - можно вынести в отдельный метод
            User user = new User()
            {
                Username = Username,
                Password = HashService.HashPassword(MyPassword), //шифрование пароля
                Email = Email,
                Role = IsClient ? RoleEnum.Client : RoleEnum.Employee
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
                    Position = Position.Value
                };
            }

            if (_dataService.AddUser(user))
            {
                Validation = "Регистрация прошла успешно";
                ClearForm();
                _isValidated = true;
            }
            else Validation = "Неизвестная ошибка! Не удалось зарегистрировать пользователя";

            MessageBox.Show(Validation);
        }

        private void ClearForm()
        {
            Username = string.Empty;
            MyPassword = string.Empty;
            RepeatedPassword = string.Empty;
            Surname = string.Empty;
            Firstname = string.Empty;
            Patronimyc = string.Empty;
            Number = string.Empty;
            Email = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
