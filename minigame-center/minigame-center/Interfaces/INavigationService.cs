using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using minigame_center.ViewModel;

namespace minigame_center.Interfaces
{
    public interface INavigationService
    {
        void NavigateToViewModel(BaseViewModel viewModel);
    }
}
