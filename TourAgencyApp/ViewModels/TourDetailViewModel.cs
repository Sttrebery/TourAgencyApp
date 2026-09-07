using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourAgencyApp.Models;

namespace TourAgencyApp.ViewModels
{
    public class TourDetailViewModel : INotifyPropertyChanged
    {
        private Tour _tour;
        private ObservableCollection<Photo> _photos = new();
        private Photo _selectedPhoto;

        public Tour Tour
        {
            get { return _tour; }
            set { _tour = value; OnPropertyChanged(); }
        }
        
        public string TourName
        {
            get => Tour.Name;
        }

        public DateTime StartDate
        {  get => Tour.StartDate; }

        public DateTime EndDate
        { get => Tour.EndDate; }

        public string Description
        { get => Tour.Description; }

        public decimal Cost
        { get => Tour.Cost; }

        public int MaxTouristCount
        { get => Tour.MaxTouristCount; }

        public string HotelName
        { get => Tour.Hotel!.Name; }

        public string TransportName
        { get => Tour.TransoprtType!.Name; }

        public string CountryName
        { get => Tour.Country!.Name; }

        public ObservableCollection<Photo> Photos
        {
            get => _photos;
            set { _photos = value; OnPropertyChanged(); }
        }

        public Photo SelectedPhoto
        {
            get => _selectedPhoto;
            set {_selectedPhoto = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentPhotoChanged)); }
        }

        // Событие для триггера анимации
        public event EventHandler CurrentPhotoChanged;

        public ICommand NextPhotoCommand { get; }
        public ICommand PreviousPhotoCommand { get; }

        public TourDetailViewModel(Tour tour) 
        {
            Tour = tour;
            if (Tour.Photos != null)
            {
                Photos = new ObservableCollection<Photo>(Tour.Photos);
                SelectedPhoto = Photos.First();
            }
            NextPhotoCommand = new RelayCommand(NextPhoto);
            PreviousPhotoCommand = new RelayCommand(PreviousPhoto);
        }

        private void NextPhoto()
        {
            if (Photos.Count == 0) return;
            var currentIndex = Photos.IndexOf(SelectedPhoto);
            var nextIndex = (currentIndex + 1) % Photos.Count;
            SelectedPhoto = Photos[nextIndex];
        }

        private void PreviousPhoto()
        {
            if (Photos.Count == 0) return;
            var currentIndex = Photos.IndexOf(SelectedPhoto);
            var prevIndex = (currentIndex - 1 + Photos.Count) % Photos.Count;
            SelectedPhoto = Photos[prevIndex];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
