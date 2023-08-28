using minigame_center.Components;
using minigame_center.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace minigame_center.ViewModel
{
    public class MenueViewModel : BaseViewModel
    {
        public MenuItemViewModel VierGewinntMenuItemViewModel { get; }
        public MenuItemViewModel TestUIMenuItemViewModel { get; }
        public MenuItemViewModel ThirdMenuItemViewModel { get; }
        public MenuItemViewModel FourthMenuItemViewModel { get; }

        public MenueViewModel()
        {
            VierGewinntMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "Vier Gewinnt",
                BackgroundImageSource = "../Assets/vierGewinnt.jpg",
                NavDestination = new VierGewinntViewModel(),
                NavDestinationHeadline = "Vier Gewinnt"
            };
            VierGewinntMenuItemViewModel.ButtonClicked += HandleMenuItemClicked;
            TestUIMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "TestUI",
                BackgroundImageSource = "../Assets/placeholder.png",
                NavDestination = new TestUIViewModel(),
                NavDestinationHeadline = "TestUI"
            };
            TestUIMenuItemViewModel.ButtonClicked += HandleMenuItemClicked;
            ThirdMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "",
                BackgroundImageSource = "../Assets/inProgress.jpg"
            };
            ThirdMenuItemViewModel.ButtonClicked += HandleMenuItemClicked;
            FourthMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "",
                BackgroundImageSource = "../Assets/inProgress.jpg"
            };
            FourthMenuItemViewModel.ButtonClicked += HandleMenuItemClicked;
        }

        private void HandleMenuItemClicked(object sender, EventArgs e)
        {
            var menuItemViewModel = sender as MenuItemViewModel;
            if (menuItemViewModel != null && menuItemViewModel.NavDestination != null)
            {

                Task.Run(() => VierGewinntViewModel._mq.Setup());

                App.MainViewModel.NavigateToPage(menuItemViewModel.NavDestination, menuItemViewModel.NavDestinationHeadline);
            }
            
        }
    }
}