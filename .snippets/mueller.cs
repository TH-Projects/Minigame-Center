public void OnNavigatedTo()
{
    GenerateGrid(6, 7);
}

public void GenerateGrid(int rows, int columns)
{
    // Erstelle ein neues Grid-Element, um das Spielbrett zu erstellen.
    gameGrid = new Grid();  // private Grid gameGrid;

    // Erstelle Zeilendefinitionen für das Grid basierend auf der Anzahl der Zeilen.
    for (int i = 0; i < rows; i++)
    {
        gameGrid.RowDefinitions.Add(new RowDefinition());
    }

    // Erstelle Spaltendefinitionen für das Grid basierend auf der Anzahl der Spalten.
    for (int j = 0; j < columns; j++)
    {
        gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
    }

    // Weise das erstellte Grid der ViewGeneratedGrid-Eigenschaft zu, um es in der GUI anzuzeigen.
    ViewGeneratedGrid = gameGrid; // Public mit Get und Set

    // Erzeuge Schaltflächen (Buttons) für das Einwerfen von Spielsteinen in jede Spalte.
    for (int col = 0; col < columns; col++)
    {
        Button dropButton = new Button();
        dropButton.Content = "*"; // Setze den Anzeigetext der Schaltfläche auf ein Sternsymbol.

        // Setze den Stil (Style) der Schaltfläche basierend auf Anwendungsressourcen.
        dropButton.Style = (Style)Application.Current.Resources["DarkButtonStyle"];

        // Konfiguriere die Ausrichtung, den Abstand und den Tag der Schaltfläche.
        dropButton.VerticalAlignment = VerticalAlignment.Stretch;
        dropButton.HorizontalAlignment = HorizontalAlignment.Stretch;
        dropButton.Margin = new Thickness(20);
        dropButton.Tag = col; // Verwende den Tag, um die ausgewählte Spalte zu identifizieren.
        dropButton.FontSize = 24;

        // Weise eine Click-Handler-Funktion zu, um auf das Klicken der Schaltfläche zu reagieren.
        dropButton.Click += (sender, e) => DropButton_Click(dropButton.Tag);

        // Füge die Schaltfläche dem Grid hinzu und platziere sie in der ersten Zeile und der aktuellen Spalte.
        gameGrid.Children.Add(dropButton);
        Grid.SetRow(dropButton, 0);
        Grid.SetColumn(dropButton, col);
    }

    // Erzeuge ein Array von Ellipsen, um das Spielfeld zu repräsentieren.
    circlesArray = new Ellipse[rows, columns];

    // Erzeuge Ellipsen für jede Zelle des Spielfelds.
    for (int row = 1; row < rows; row++)
    {
        for (int col = 0; col < columns; col++)
        {
            Ellipse blackCircle = new Ellipse();
            blackCircle.Fill = Brushes.DarkBlue; // Fülle die Ellipse mit dunkelblauer Farbe.
            blackCircle.VerticalAlignment = VerticalAlignment.Stretch;
            blackCircle.HorizontalAlignment = HorizontalAlignment.Stretch;
            blackCircle.Margin = new Thickness(17, 15, 17, 15); // Setze den Abstand der Ellipse.
            
            // Füge die Ellipse dem Grid hinzu und platziere sie in der aktuellen Zeile und Spalte.
            gameGrid.Children.Add(blackCircle);
            Grid.SetRow(blackCircle, row);
            Grid.SetColumn(blackCircle, col);

            // Weise die Ellipse dem circlesArray zu, um sie später zu aktualisieren.
            circlesArray[row, col] = blackCircle;
        }
    }
}

    circlesArray = new Ellipse[rows, columns];

    for (int row = 1; row < rows; row++)
    {
        for (int col = 0; col < columns; col++)
        {
            Ellipse blackCircle = new Ellipse();
            blackCircle.Fill = Brushes.DarkBlue;
            blackCircle.VerticalAlignment = VerticalAlignment.Stretch;
            blackCircle.HorizontalAlignment = HorizontalAlignment.Stretch;
            blackCircle.Margin = new Thickness(17,15,17,15);
            gameGrid.Children.Add(blackCircle);
            Grid.SetRow(blackCircle, row);
            Grid.SetColumn(blackCircle, col);

            circlesArray[row, col] = blackCircle;
        }
    }
}