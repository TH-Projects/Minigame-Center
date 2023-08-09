using minigame_center.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using minigame_center.HelperClasses;

namespace minigame_center
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            var navigationService = new NavigationService(mainWindow.fr_MainContent);
            var mainWindowViewModel = new MainWindowViewModel(navigationService);
            mainWindow.DataContext = mainWindowViewModel;
     
            this.MainWindow = mainWindow; // Das MainWindow der App zuweisen
            mainWindow.Show();       
        }

    }

}
