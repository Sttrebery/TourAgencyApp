using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TourAgencyApp.Models;
using TourAgencyApp.Services;

namespace TourAgencyApp.ViewModels
{
    public class EmployeeTransportsViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Transport> _transports;
        private ICollectionView _transportsView;
        private string _transportName; //to edit country name
        private string _transportToAdd;
        private string _transportToRename;
        private bool _isChanged;

        public ObservableCollection<Transport> Transports
        {
            get => _transports;
            set { _transports = value; OnPropertyChanged(); }
        }

        public ICollectionView TransportsView
        {
            get => _transportsView;
            set { _transportsView = value; OnPropertyChanged(); }
        }

        public string TransportName
        {
            get => _transportName;
            set { _transportName = value; OnPropertyChanged(); }
        }

        public string TransportToAdd
        {
            get => _transportToAdd;
            set { _transportToAdd = value; OnPropertyChanged(); }
        }

        public string TransportToRename
        {
            get => _transportToRename;
            set { _transportToRename = value; OnPropertyChanged(); }
        }

        public ICommand UpdateListCommmand { get; set; }
        public ICommand AddTransportCommmand { get; set; }
        public ICommand RenameTransportCommand { get; set; }

        public EmployeeTransportsViewModel(DataService d)
        {
            _dataService = d;
            _isChanged = false;
            _transports = new ObservableCollection<Transport>(_dataService.GetAllTransports()); //получаем либо список, либо пустой List (но не null!)
            _transportsView = CollectionViewSource.GetDefaultView(Transports);

            AddTransportCommmand = new RelayCommand(AddCountry);
            UpdateListCommmand = new AsyncRelayCommand(UpdateList);
            RenameTransportCommand = new RelayCommand(RenameCountry);
        }

        //async
        private async Task UpdateList()
        {
            //загрузка данных в бд если есть
            if (_isChanged)
            {
                try
                {
                    await _dataService.SaveTransportsAsync(Transports);
                    _isChanged = false;

                    MessageBox.Show("Данные были обновлены.");
                }
                catch
                {
                    MessageBox.Show("Не удалось сохранить данные", "Ошибка");
                }
            }
            Transports = new ObservableCollection<Transport>(await _dataService.GetAllTransportsAsync()); //наоборот - загрузка из БД для обновления ID
            TransportsView?.Refresh();
        }

        private void CheckName(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите новое название!", "Ошибка");
                return;
            }
        }

        private void RenameCountry()
        {
            CheckName(TransportName);
            int countryIndex = Transports.IndexOf(Transports.First(f => f.Name == TransportToRename));
            Transports[countryIndex].Name = TransportName.Trim();
            TransportName = string.Empty;
            TransportToRename = string.Empty;
            _isChanged = true;
            MessageBox.Show("Изменения сохранены.");

            TransportsView?.Refresh();
        }

        private void AddCountry()
        {
            CheckName(TransportToAdd);

            var founded = Transports.FirstOrDefault(f => f.Name == TransportToAdd);
            if (founded != null)
            {
                MessageBox.Show("Такой транспорт уже есть в базе!", "Ошибка");
            }
            else
            {
                //добавление в список (отложенный режим)
                Transports.Add(new Transport() { Name = TransportToAdd.Trim() });
                TransportToAdd = string.Empty;
                _isChanged = true;
                MessageBox.Show("Запись добавлена!");

                TransportsView?.Refresh();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
