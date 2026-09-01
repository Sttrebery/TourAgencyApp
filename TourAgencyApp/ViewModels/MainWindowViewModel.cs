using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TourAgencyApp.Models;
using System.Text;
using System.Threading.Tasks;

namespace TourAgencyApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
