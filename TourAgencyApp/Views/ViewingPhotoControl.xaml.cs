using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourAgencyApp.Models;

namespace TourAgencyApp.Views
{
    /// <summary>
    /// Логика взаимодействия для ViewingPhotoControl.xaml
    /// </summary>
    public partial class ViewingPhotoControl : UserControl
    {
        //public IObservable<Photo> Images { get; set; }

        public ViewingPhotoControl()
        {
            InitializeComponent();
            //DataContext = this;
        }

        // DependencyProperty для привязки коллекции извне
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(IEnumerable),
                typeof(ViewingPhotoControl),
                new PropertyMetadata(null));

        public IEnumerable Images
        {
            get { return (IEnumerable)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }
    }
}
