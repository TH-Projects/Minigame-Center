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
    internal class MenueViewModel : BaseViewModel
    {
        public DelegateCommand NavigateCommand { get; set; }

        public event EventHandler MenueItemClicked;

        public MenuItemViewModel VierGewinntMenuItemViewModel { get; }
        public MenuItemViewModel SecondMenuItemViewModel { get; }
        public MenuItemViewModel ThirdMenuItemViewModel { get; }
        public MenuItemViewModel FourthMenuItemViewModel { get; }


        public MenueViewModel()
        {    
            this.NavigateCommand = new DelegateCommand(
                        (o) =>  App.MainViewModel.NavigateToPage(new VierGewinntViewModel())
            ) ;

            VierGewinntMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "Vier Gewinnt",
                BackgroundImageSource = "../Assets/vierGewinnt.jpg"
            };
            VierGewinntMenuItemViewModel.ButtonClicked += HandleMenuItemClicked;
            SecondMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "",
                BackgroundImageSource = "../Assets/inProgress.jpg"
            };
            ThirdMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "",
                BackgroundImageSource = "../Assets/inProgress.jpg"
            };
            FourthMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "",
                BackgroundImageSource = "../Assets/inProgress.jpg"
            };
            
        }

        private void HandleMenuItemClicked(object sender, EventArgs e) => NavigateCommand.Execute(new VierGewinntViewModel());
    }
}
