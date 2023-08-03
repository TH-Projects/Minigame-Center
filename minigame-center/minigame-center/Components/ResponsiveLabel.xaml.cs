using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace minigame_center.Components
{
    /// <summary>
    /// Interaction logic for ResponsiveLabel.xaml
    /// </summary>
    public partial class ResponsiveLabel : UserControl
    {
        private int _maxTextHeight = 9999;

        public string Text { set { txt.Text = value; } }
        public int MaxTextHeight { set { _maxTextHeight = value; } }
        public ResponsiveLabel()
        {
            this.InitializeComponent();
            txt.Text = "No text loaded";
        }
    }
}
