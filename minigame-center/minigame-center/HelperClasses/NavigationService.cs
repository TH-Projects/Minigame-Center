using minigame_center.Interfaces;
using minigame_center.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace minigame_center.HelperClasses
{
    public class NavigationService : INavigationService
    {
        private Frame _mainFrame;
        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }
        public void NavigateToViewModel(BaseViewModel viewModel)
        {
            //searches for the matching view of the viewModel param and navigates the frame to it
            var view = ViewLocator.GetViewForViewModel(viewModel);
            _mainFrame.Navigate(view);
        }
    }
}
