﻿using minigame_center.Components;
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
        public MenuItemViewModel SecondMenuItemViewModel { get; }
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
            SecondMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "",
                BackgroundImageSource = "../Assets/inProgress.jpg",
                
            };
            //TestUIMenuItemViewModel.ButtonClicked += HandleMenuItemClicked;
            ThirdMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "",
                BackgroundImageSource = "../Assets/inProgress.jpg",
            };
            //ThirdMenuItemViewModel.ButtonClicked += HandleMenuItemClicked;
            FourthMenuItemViewModel = new MenuItemViewModel
            {
                ButtonContent = "",
                BackgroundImageSource = "../Assets/inProgress.jpg"
            };
            //FourthMenuItemViewModel.ButtonClicked += HandleMenuItemClicked;
        }


        private void HandleMenuItemClicked(object sender, EventArgs e)
        {
            var menuItemViewModel = sender as MenuItemViewModel;

            App.MainViewModel.NavigateToPage(menuItemViewModel.NavDestination, menuItemViewModel.NavDestinationHeadline);

            //Initialisiert den Handshake zum anderen Spieler
            Task.Run(() => VierGewinntViewModel.Setup());
            

        }
    }
}