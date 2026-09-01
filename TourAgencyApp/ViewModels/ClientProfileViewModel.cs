using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourAgencyApp.Data;
using System.Windows;
using TourAgencyApp.Models;

namespace TourAgencyApp.ViewModels
{
    public class ClientProfileViewModel : INotifyPropertyChanged
    {
        private Tourist _client;
        private string _name;
        private string _surname;
        private string _patronimyc;
        private string _phone;
        private string _email;


        public Tourist Client
        {
            get => _client;
            set { _client = value; OnPropertyChanged(); }
        }

        public string Name 
        {
            get => _name;
            set 
            {
                _name = value; OnPropertyChanged();
            }
        }
        public string Surname
        {
            get => _surname;
            set 
            {
                _surname = value; OnPropertyChanged();
            }
        }
        public string Patronymic 
        { 
            get => _patronimyc; 
            set 
            {
                _patronimyc = value; OnPropertyChanged();
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
                _email = value; OnPropertyChanged();
            }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand ClearCommand { get; }

        public ClientProfileViewModel(Tourist c)
        {
            Client = c;

            Name = c.Name;
            Surname = c.Surname;
            Patronymic = c.Patronimyc;
            Phone = c.PhoneNumber;
            Email = c.Email;

            SaveChangesCommand = new RelayCommand(SaveChanges);
            ClearCommand = new RelayCommand(Clear);
        }


        private void Clear()
        {
            Name = Client.Name;
            Surname = Client.Surname;
            Patronymic = Client.Patronimyc;
            Phone = Client.PhoneNumber;
            Email = Client.Email;
        }

        private void SaveChanges()
        {
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    db.Clients.Attach(Client);
                    
                    Client.Name = Name;
                    Client.Surname = Surname;
                    Client.Patronimyc= Patronymic;
                    Client.PhoneNumber = Phone;
                    Client.Email = Email;

                    db.SaveChanges();
                }
                MessageBox.Show("Изменения внесены успешно");
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
