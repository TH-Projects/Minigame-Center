using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace minigame_center.HelperClasses
{
    public class DelegateCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public DelegateCommand(Predicate<Object> canExecute, Action<object> execute) =>
                (_canExecute, _execute) = (canExecute, execute); 

        public DelegateCommand(Action<object> execute) : this (null, execute) { }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
        

        public void Execute(object parameter) => _execute?.Invoke(parameter);
        
    }
}
