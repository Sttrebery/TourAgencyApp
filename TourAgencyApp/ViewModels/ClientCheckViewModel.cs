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
using TourAgencyApp.Data;
using TourAgencyApp.Models;

namespace TourAgencyApp.ViewModels
{
    public class ClientCheckViewModel : INotifyPropertyChanged
    {
        public class FIO : IEquatable<FIO>
        {
            public string Name { get; set; }
            public string SurName { get; set; }
            public string Patronimyc { get; set; }

            public override string ToString()
            {
                return $"{SurName} {Name} {Patronimyc}";
            }

            // Реализация IEquatable<T>
            public bool Equals(FIO other)
            {
                if (other == null) return false;
                return SurName == other.SurName &&
                       Name == other.Name &&
                       Patronimyc == other.Patronimyc;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as FIO);
            }

            public override int GetHashCode()
            {
                return (SurName?.GetHashCode() ?? 0) ^
                       (Name?.GetHashCode() ?? 0) ^
                       (Patronimyc?.GetHashCode() ?? 0);
            }
        }

        private readonly DataService _dataService;
        private FIO _selectedClient;
        private IEnumerable<FIO> _allClients;

        public FIO SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<FIO> AllClients
        {
            get { return _allClients; }
            set {  _allClients = value; OnPropertyChanged(); }
        }

        public ICommand CheckCommand { get; set; }

        public ClientCheckViewModel(DataService dataService) 
        {
            _dataService = dataService;
            CheckCommand = new RelayCommand(Check);

            LoadData();
            SelectedClient = _allClients.FirstOrDefault();
        }

        public void LoadData()
        {
            //_allClients = _dataService.GetAllClients().Select(c => new FIO { Name = c.Name_, SurName = c.Surname, Patronimyc = c.Patronymic }).Distinct();
        }

        private void Check()
        {
            //MessageBox.Show(_dataService.CheckClient(SelectedClient.SurName, SelectedClient.Name, SelectedClient.Patronimyc));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
