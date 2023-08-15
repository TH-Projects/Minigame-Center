using minigame_center.HelperClasses;
using minigame_center.ViewModel;
using System;

public class MenuItemViewModel : BaseViewModel
{
    public event EventHandler ButtonClicked;

    public string BackgroundImageSource { get; set; }
    public string ButtonContent { get; set; }
    public BaseViewModel NavDestination { get; set; }
    public string NavDestinationHeadline { get; set; }

    public DelegateCommand ButtonCommand { get; }

    public MenuItemViewModel()
    {
        ButtonCommand = new DelegateCommand((o) => ButtonClicked?.Invoke(this, EventArgs.Empty));
    }
}