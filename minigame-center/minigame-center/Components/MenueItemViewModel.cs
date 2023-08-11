using minigame_center.HelperClasses;
using minigame_center.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace minigame_center.Components
{
    public class MenuItemViewModel : BaseViewModel
    {
        public event EventHandler ButtonClicked;

        public string BackgroundImageSource { get; set; }
        public string ButtonContent { get; set; }

        public DelegateCommand ButtonCommand { get; }

        public MenuItemViewModel()
        {
            ButtonCommand = new DelegateCommand((o) => ButtonClicked?.Invoke(this, EventArgs.Empty));
        }
    }
}