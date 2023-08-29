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
        public static Ellipse[,] circlesArray; // 2D Array, um die Ellipsen zu speichern
        private const double circleSize = 20; // Feste Größe der Kreise
        private Grid gameGrid; // Instanzvariable für das Spielraster

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

            // Buttons erzeugen
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

            circlesArray = new Ellipse[rows, columns]; // Initialisierung des Arrays

            // Kreise erzeugen
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

                    circlesArray[row, col] = blackCircle; // Speichern der Ellipse im Array
                }
            }

            // Beispielhafte Funktionsaufrufe zum Einfärben von Feldern
            // ColorCircle(1, 2, Brushes.Red);
            // ColorCircle(2, 3, Brushes.Green);
            // ColorCircle(3, 4, Brushes.Blue);
        }

        private void DropButton_Click(object sender, RoutedEventArgs e, int column)
        {
            //int rowIndex = FindLowestEmptyRow(column);
            //if (rowIndex >= 0)
            //{
            //    Ellipse yellowCircle = new Ellipse();
            //    yellowCircle.Fill = Brushes.Yellow;
            //    yellowCircle.Width = circleSize;
            //    yellowCircle.Height = circleSize;
            //    yellowCircle.Margin = new System.Windows.Thickness(5);
            //    gameGrid.Children.Add(yellowCircle);
            //    Grid.SetRow(yellowCircle, rowIndex);
            //    Grid.SetColumn(yellowCircle, column);

            //    circlesArray[rowIndex, column] = yellowCircle; // Aktualisieren des Arrays

                // Weitere Aktionen durchführen, z.B. Überprüfung auf Gewinnbedingungen
            }
        }

        //private int FindLowestEmptyRow(int column)
        //{
        //    for (int row = circlesArray.GetLength(0) - 1; row >= 0; row--)
        //    {
        //        if (circlesArray[row, column] == null)
        //        {
        //            return row;
        //        }
        //    }
        //    return -1; // Keine leere Zeile gefunden
        //}

        //private void ColorCircle(int row, int col, Brush color)
        //{
        //    if (row >= 1 && row < circlesArray.GetLength(0) && col >= 0 && col < circlesArray.GetLength(1))
        //    {
        //        if (circlesArray[row, col] != null)
        //        {
        //            circlesArray[row, col].Fill = color;
        //        }
        //    }
        //}
    }

