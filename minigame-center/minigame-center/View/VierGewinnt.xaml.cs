using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace minigame_center.View
{
    public partial class VierGewinnt : Page
    {
        private Ellipse[,] circlesArray;
        private const double circleSize = 20;
        private Grid gameGrid;

        public VierGewinnt()
        {
            InitializeComponent();
            GenerateGrid(6, 7);
        }

        private void GenerateGrid(int rows, int columns)
        {
            gameGrid = new Grid();

            for (int i = 0; i < rows; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < columns; j++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            GeneratedGrid.Children.Add(gameGrid);

            for (int col = 0; col < columns; col++)
            {
                Button dropButton = new Button();
                dropButton.Content = "*";
                dropButton.Width = circleSize;
                dropButton.Height = 40;
                dropButton.Margin = new Thickness(5);
                dropButton.Tag = col; // Speichere den Index der Spalte im Tag
                dropButton.Click += DropButton_Click; // Handle button click
                gameGrid.Children.Add(dropButton);
                Grid.SetRow(dropButton, 0);
                Grid.SetColumn(dropButton, col);
            }

            circlesArray = new Ellipse[rows, columns];

            for (int row = 1; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Ellipse blackCircle = new Ellipse();
                    blackCircle.Fill = Brushes.DarkBlue;
                    blackCircle.Width = circleSize;
                    blackCircle.Height = circleSize;
                    blackCircle.Margin = new Thickness(5);
                    gameGrid.Children.Add(blackCircle);
                    Grid.SetRow(blackCircle, row);
                    Grid.SetColumn(blackCircle, col);

                    circlesArray[row, col] = blackCircle;
                }
            }
        }

        private void DropButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int column = (int)button.Tag; //@michi hier die Spalte

            int rowIndex = FindLowestEmptyRow(column);
            if (rowIndex >= 0)
            {
                ColorCircle(rowIndex, column, Brushes.Yellow);
                CheckForWin(rowIndex, column);
            }
        }

        private int FindLowestEmptyRow(int column)
        {
            for (int row = circlesArray.GetLength(0) - 1; row >= 0; row--)
            {
                if (circlesArray[row, column] == null)
                {
                    return row;
                }
            }
            return -1;
        }

        private void ColorCircle(int row, int col, Brush color)
        {
            if (row >= 1 && row < circlesArray.GetLength(0) && col >= 0 && col < circlesArray.GetLength(1))
            {
                if (circlesArray[row, col] != null)
                {
                    circlesArray[row, col].Fill = color;
                }
            }
        }

        private void CheckForWin(int row, int col)
        {
            // Beispielsweise für die Logik anker @michi
        }
    }
}
