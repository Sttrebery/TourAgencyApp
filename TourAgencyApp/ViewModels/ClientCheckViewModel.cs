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
    public class ClientCheckViewModel : INotifyPropertyChanged
    {
        //public class FIO : IEquatable<FIO>
        //{
        //    public string Name { get; set; }
        //    public string SurName { get; set; }
        //    public string Patronimyc { get; set; }

        //    public override string ToString()
        //    {
        //        return $"{SurName} {Name} {Patronimyc}";
        //    }

        //    // Реализация IEquatable<T>
        //    public bool Equals(FIO other)
        //    {
        //        if (other == null) return false;
        //        return SurName == other.SurName &&
        //               Name == other.Name &&
        //               Patronimyc == other.Patronimyc;
        //    }

        //    public override bool Equals(object obj)
        //    {
        //        return Equals(obj as FIO);
        //    }

        //    public override int GetHashCode()
        //    {
        //        return (SurName?.GetHashCode() ?? 0) ^
        //               (Name?.GetHashCode() ?? 0) ^
        //               (Patronimyc?.GetHashCode() ?? 0);
        //    }
        //}

        private readonly DataService _dataService;
        private int? _selectedClient;
        private IEnumerable<int> _allClientsIds;
        private IEnumerable<Tourist> _allClients;

        public int? SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<int> AllClientsIds
        {
            get { return _allClients.Select(a=> a.ID).ToList(); }
        }

        public IEnumerable<Tourist> AllClients
        {
            get => _allClients;
            set
            {
                _allClients = value;
                OnPropertyChanged();
            }
        }

        public ICommand CheckCommand { get; set; }

        public ClientCheckViewModel(DataService dataService) 
        {
            _dataService = dataService;
            CheckCommand = new RelayCommand(Check);

            LoadData();
            SelectedClient = _allClientsIds?.First();
        }

        public void LoadData()
        {
            _allClients = _dataService.GetAllClients().ToList();
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
