using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace minigame_center
{
    public partial class Menue : Page
    {
        public Menue()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TestUI());
        }
    }
}
