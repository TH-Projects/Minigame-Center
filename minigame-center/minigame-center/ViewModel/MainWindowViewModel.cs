using minigame_center.HelperClasses;
using minigame_center.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minigame_center.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        public string MainHeadline { get; set; }

        public MainWindowViewModel(INavigationService navigationService)
        {
            MainHeadline = "No Headline Loaded";
            _navigationService = navigationService;
            NavigateToInitialView();
        }

        private void NavigateToInitialView()
        {
            var viewModel = new MenueViewModel();
            MainHeadline = "Hauptmenü";
            _navigationService.NavigateToViewModel(viewModel);
        }

        public void NavigateToPage(BaseViewModel viewModel)
        {
            var view = ViewLocator.GetViewForViewModel(viewModel);
            _navigationService.NavigateToViewModel(viewModel);    
        }
        

       
    }
}
