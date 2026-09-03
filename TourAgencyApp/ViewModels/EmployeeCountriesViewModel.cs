using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourAgencyApp.Services;
using TourAgencyApp.Models;
using System.Windows;
using System.Collections.ObjectModel;

namespace TourAgencyApp.ViewModels
{
    public class EmployeeCountriesViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private ObservableCollection<Country> _countries;
        private string _countryName;
        private string _countryToAdd;
        private bool _isChanged;

        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set { _countries = value; OnPropertyChanged(); }
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

        public ICommand UpdateListCommmand {  get; set; }
        public ICommand AddCountryCommmand {  get; set; }

        public EmployeeCountriesViewModel(DataService d) 
        {
            _dataService = d;
            _isChanged = false;
            Countries = new ObservableCollection<Country>(_dataService.GetAllCountries()); //получаем либо список, либо пустой List (но не null!)

            AddCountryCommmand = new RelayCommand(AddCountry);
            UpdateListCommmand = new AsyncRelayCommand(UpdateList);
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
        }

        private void AddCountry()
        {
            var founded = Countries.FirstOrDefault(f => f.Name == CountryToAdd);
            if (founded != null)
            {
                MessageBox.Show("Такая страна уже есть в базе!", "Ошибка");
            }
            else
            {
                //добавление в список (отложенный режим)
                Countries.Add(new Country() { Name = CountryToAdd });
                CountryToAdd = string.Empty;
                _isChanged = true;
                MessageBox.Show("Страна добавлена!");

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
