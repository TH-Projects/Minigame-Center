using minigame_center.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minigame_center.HelperClasses
{
    public static class ViewLocator
    {
        public static object GetViewForViewModel(BaseViewModel viewModel)
        {
            // Conventions: This ViewLocator is based on the used naming conventions
            // Naming comvention: The name of the ViewModel is the name of the View with "ViewModel" at the end.
            // Exapmple: TestUIViewModel -> TestUI
            string viewModelName = viewModel.GetType().Name;
            string viewName = viewModelName.Replace("ViewModel", "");

            // All Views are in the namespace "minigame_center.View".
            string fullViewName = $"minigame_center.View.{viewName}";

            // Loads the View object and returns it.
            try
            {
                Type viewType = Type.GetType(fullViewName);
                if (viewType != null)
                {
                    return Activator.CreateInstance(viewType);
                }
                else
                {
                    throw new ArgumentException($"View not found for ViewModel: {viewModelName}");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error loading view for ViewModel: {viewModelName}. {ex.Message}");
            }
        }
    }

}
