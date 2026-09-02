using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TourAgencyApp.ViewModels
{
    public class EmailConfirmViewModel : INotifyPropertyChanged
    {
        private readonly string _original_code;
        private string _code;
        private bool _success;

        public bool Success 
        {
            get => _success;
            set {_success = value; OnPropertyChanged(); } 
        }

        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(); }
        }

        public ICommand ConfirmCommand { get; set; }


        public EmailConfirmViewModel(string orig_code)
        {
            _original_code = orig_code;
            Success = false;

            ConfirmCommand = new RelayCommand(Confirm);
        }

        private void Confirm()
        {
            if (Code == _original_code)
            {
                Success = true;
            }
            else Success = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
