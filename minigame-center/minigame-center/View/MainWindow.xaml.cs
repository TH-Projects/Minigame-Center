using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace minigame_center
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            content_frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            content_frame.Navigate(new Menue());
        }

        private void lbl_headline_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
