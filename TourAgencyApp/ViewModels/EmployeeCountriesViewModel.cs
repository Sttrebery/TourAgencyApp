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
    public class EmployeeCountriesViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Country> _countries;
        private ICollectionView _countriesView;
        private string _countryName; //to edit country name
        private string _countryToAdd;
        private string _countryToRename;
        private bool _isChanged;

        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set { _countries = value; OnPropertyChanged(); }
        }

        public ICollectionView CountriesView
        {
            get => _countriesView;
            set { _countriesView = value; OnPropertyChanged(); }
        }

        public string CountryName
        {
            get => _countryName;
            set { _countryName = value; OnPropertyChanged(); }
        }

        public string CountryToAdd
        {
            get => _countryToAdd;
            set { _countryToAdd = value; OnPropertyChanged(); }
        }

        public string CountryToRename
        {
            get => _countryToRename;
            set { _countryToRename = value; OnPropertyChanged(); }
        }

        public ICommand UpdateListCommmand {  get; set; }
        public ICommand AddCountryCommmand {  get; set; }
        public ICommand RenameCountryCommand { get; set; }

        public EmployeeCountriesViewModel(DataService d) 
        {
            _dataService = d;
            _isChanged = false;
            Countries = new ObservableCollection<Country>(_dataService.GetAllCountries()); //получаем либо список, либо пустой List (но не null!)
            CountriesView = CollectionViewSource.GetDefaultView(Countries);

            AddCountryCommmand = new RelayCommand(AddCountry);
            UpdateListCommmand = new AsyncRelayCommand(UpdateList);
            RenameCountryCommand = new RelayCommand(RenameCountry);
        }

        //async
        private async Task UpdateList()
        {
            //загрузка данных в бд если есть
            if (_isChanged)
            {
                try
                {
                    await _dataService.SaveCountriesAsync(Countries);
                    _isChanged = false;

                    MessageBox.Show("Данные были обновлены.");
                }
                catch
                {
                    MessageBox.Show("Не удалось сохранить данные", "Ошибка");
                }
            }
            Countries = new ObservableCollection<Country>(await _dataService.GetAllCountriesAsync()); //наоборот - загрузка из БД для обновления ID
            CountriesView?.Refresh();
        }

        private void CheckCountryName(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите новое название!", "Ошибка");
                return;
            }
        }

        private void RenameCountry()
        {
            CheckCountryName(CountryName);
            int countryIndex = Countries.IndexOf(Countries.First(f => f.Name == CountryToRename));
            Countries[countryIndex].Name = CountryName.Trim();
            CountryName = string.Empty;
            CountryToRename = string.Empty;
            _isChanged = true;
            MessageBox.Show("Изменения сохранены.");

            CountriesView?.Refresh();
        }

        private void AddCountry()
        {
            CheckCountryName(CountryToAdd);

            var founded = Countries.FirstOrDefault(f => f.Name == CountryToAdd);
            if (founded != null)
            {
                MessageBox.Show("Такая страна уже есть в базе!", "Ошибка");
            }
            else
            {
                //добавление в список (отложенный режим)
                Countries.Add(new Country() { Name = CountryToAdd.Trim() });
                CountryToAdd = string.Empty;
                _isChanged = true;
                MessageBox.Show("Страна добавлена!");

                CountriesView?.Refresh();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
