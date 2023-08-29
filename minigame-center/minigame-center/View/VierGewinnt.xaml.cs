using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System;
using minigame_center.ViewModel;

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

            const double circleSize = 20; // Feste Größe der Kreise

            for (int col = 0; col < columns; col++)
            {
                Button dropButton = new Button();
                dropButton.Content = "*";
                dropButton.Width = circleSize;
                dropButton.Height = 40;
                dropButton.Margin = new System.Windows.Thickness(5);
                dropButton.Click += (sender, e) => VierGewinntViewModel.DropButton_Click(sender, e, col); // Handle button click
                gameGrid.Children.Add(dropButton);
                Grid.SetRow(dropButton, 0);
                Grid.SetColumn(dropButton, col);
            }

            for (int row = 1; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Ellipse blackCircle = new Ellipse();
                    blackCircle.Fill = Brushes.DarkBlue;
                    blackCircle.Width = circleSize;
                    blackCircle.Height = circleSize;
                    blackCircle.Margin = new System.Windows.Thickness(5);
                    gameGrid.Children.Add(blackCircle);
                    Grid.SetRow(blackCircle, row);
                    Grid.SetColumn(blackCircle, col);
                }
            }
        }

      
    }
}
