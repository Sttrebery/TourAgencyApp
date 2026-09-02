using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TourAgencyApp.Views;
using TourAgencyApp.Services;
using TourAgencyApp.Models;
using TourAgencyApp.Views.Client;
using TourAgencyApp.Views.Employee;
using TourAgencyApp.Services;
using Microsoft.EntityFrameworkCore;

namespace TourAgencyApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Fields
        private DataService _dataService;
        private Tourist Client { get; set; } = null;
        private Employee Employee { get; set; } = null;
        private List<User> Users { get; set; }
        private User User { get; set; } = null;
        private string _username = string.Empty;
        private string _myPassword = string.Empty;
        private string _validation = string.Empty;
        private bool _isClient = true;
        private bool _isEmployee = false;
        private bool _isValidated = false;
        #endregion

        #region Properties
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
        public string Validation
        {
            get => _validation;
            set { _validation = value; OnPropertyChanged(); }
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
        public bool IsValidated
        {
            get => _isValidated;
            set { _isValidated = value; OnPropertyChanged(); }
        }
        #endregion

        public event Action SignEvent;
        public event Action RegisterEvent;
        public ICommand SignCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }

        public LoginViewModel()
        {
            _dataService = new DataService();
            //todo: посмотреть можно ли упростить
            SignEvent += async () => { await SignIn(); } ;
            RegisterEvent += Register;
            SignCommand = new RelayCommand(async () =>
            {
                if (SignEvent != null)
                {
                    await LoadUsers();
                    SignEvent();
                }
            });
            RegisterCommand = new RelayCommand(() =>
            {
                if (RegisterEvent != null)
                {
                    RegisterEvent();
                }
            });
        }

        private async Task SignIn()
        {
            Window logged;
            User = Users.FirstOrDefault(u => u.Username == Username)!;
            Validate(User);
            if(IsValidated)
            {

                if(User.Role == RoleEnum.Client)
                {
                    logged = new MainClientWindow();
                    Client = await _dataService.GetClientByID(User.ID);
                    var clientViewModel = new ClientMainViewModel(Client);
                    logged.DataContext = clientViewModel;
                    logged.Show();
                }
                else
                {
                    logged = new MainEmployeeWindow();
                    Employee = await _dataService.GetEmployeeByID(User.ID);
                    var empViewModel = new EmployeeMainViewModel(Employee); 
                    logged.DataContext = empViewModel;
                    logged.Show();
                }
            }
        }

        private async Task LoadUsers()
        {
            try
            {
                using var db = new TourAgencyDbContext();
                Users = await db.Users.ToListAsync();
            }
            catch
            {
                Users = new(); 
            }
        }

        private void Validate(object? User)
        {
            if (User == null)
            {
                Validation = "Такой пользователь не найден";
            }
            else if (User is User u && u.Password != HashService.HashPassword(MyPassword))
            {
                Validation = "Неправильный пароль";
            }
            else
            {
                Validation = string.Empty;
                IsValidated = true;
                return;
            }
            MessageBox.Show(Validation);
            IsValidated = false;
        }

        private void Register()
        {
            var regWindow = new TourAgencyApp.Views.Register() { DataContext = new RegisterViewModel() };
            regWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
