using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace minigame_center.View
{
    public partial class VierGewinnt : Page
    {
        public VierGewinnt()
        {
            InitializeComponent();
            GenerateGrid(6, 7);
        }

        private void GenerateGrid(int rows, int columns)
        {
            Grid gameGrid = new Grid();

            for (int i = 0; i < rows; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < columns; j++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            GeneratedGrid.Children.Add(gameGrid);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Ellipse blackCircle = new Ellipse();
                    blackCircle.Fill = Brushes.DarkBlue;
                    blackCircle.Width = 80;
                    blackCircle.Height = 80;
                    blackCircle.Margin = new System.Windows.Thickness(5); // Hier wurde der Margin-Wert angepasst
                    gameGrid.Children.Add(blackCircle);
                    Grid.SetRow(blackCircle, row);
                    Grid.SetColumn(blackCircle, col);
                }
            }
        }
    }
}
