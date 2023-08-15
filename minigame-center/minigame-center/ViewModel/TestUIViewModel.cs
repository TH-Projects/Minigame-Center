using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using minigame_center.ViewModel;
using minigame_center.HelperClasses;
using System.Security.Cryptography.X509Certificates;

namespace minigame_center.ViewModel
{
    internal class TestUIViewModel : BaseViewModel
    {


        string firstLabelHeadline = "Error";
        public string FirstLabelHeadline
        {
            get => firstLabelHeadline;
            set
            {
                if (FirstLabelHeadline != value)
                {
                    firstLabelHeadline = value;
                    OnPropertyChanged(nameof(FirstLabelHeadline));
                }
            }
        }
        string secondLabelHeadline = "Success";
        public string SecondLabelHeadline
        {
            get => secondLabelHeadline;
            set
            {
                if (SecondLabelHeadline != value)
                {
                    secondLabelHeadline = value;
                    OnPropertyChanged(nameof(SecondLabelHeadline));
                }
            }
        }
        public DelegateCommand OptionalCommand { get; set; }
        string buttonContent = "Click here";
        public string ButtonContent
        {
            get => buttonContent;
            set
            {
                if (ButtonContent != value)
                {
                    buttonContent = value;
                    OnPropertyChanged(nameof(ButtonContent));
                }
            }
        }
        string bigBoxOne;
        public string BigBoxOne
        {
            get => bigBoxOne;
            set
            {
                if(BigBoxOne != value)
                {
                    bigBoxOne = value;
                    OnPropertyChanged(nameof(BigBoxOne));
                }
            }
        }
        string bigBoxTwo;
        public string BigBoxTwo
        {
            get => bigBoxTwo;
            set
            {
                if(BigBoxTwo != value)
                {
                    bigBoxTwo = value;
                    OnPropertyChanged(nameof(BigBoxTwo));
                }
            }
        }
        public TestUIViewModel()
        {
            this.OptionalCommand = new DelegateCommand(
               (o) => { throw new NotImplementedException("No command implemented yet"); }
               );
            firstLabelHeadline = "Success";
            secondLabelHeadline = "Error";
            buttonContent = "Click";
            bigBoxOne = "bla";
            bigBoxTwo = "bli";
        }
    }
}

