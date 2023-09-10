using System;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using minigame_center.HelperClasses;
using minigame_center.Model.MQTTClient;
using minigame_center.Model.Payload;
using minigame_center.View;
using minigame_center.ViewModel;

namespace minigame_center.ViewModel
{
    public class VierGewinntViewModel : BaseViewModel
    {
        // Diese Klasse implementiert die Logik für ein Vier-gewinnt-Spiel in einer WPF-Anwendung.

        // Deklaration von Eigenschaften und Feldern
        public string PlayerLabel { get; set; } // Anzeige des aktuellen Spielers
        public string MoveLabel { get; set; }   // Anzeige des aktuellen Spielzugs

        private static bool waitBool = false; // Eine Flagge, die verwendet wird, um auf Benutzeraktionen zu warten

        public static Ellipse[,] circlesArray; // Ein Array von Ellipsen, die das Spielbrett repräsentieren
        private Grid gameGrid;                 // Das Hauptspiel-Gitter

        private Grid _viewGeneratedGrid;
        public Grid ViewGeneratedGrid
        {
            get { return _viewGeneratedGrid; }
            set
            {
                _viewGeneratedGrid = value;
                OnPropertyChanged(nameof(ViewGeneratedGrid));
            }
        }

        private static GameResult gameResult = GameResult.Running; // Der aktuelle Spielstatus

        public static MQTTGameClient mq; // Eine MQTT-Verbindung zur Kommunikation mit dem Spielserver

        // Ein Delegat, der verwendet wird, um auf eingehende Spiel-Payloads zu reagieren
        private void PayloadHandler(BasePayload payload)
        {
            // Aktualisiere die GUI entsprechend den empfangenen Spielinformationen
            UpdateGUI();

            if (MQTTGameClient.oponnent == Guid.Empty && payload.sender != MQTTGameClient.clientID)
            {
                // Wenn der Gegner noch nicht festgelegt ist, setze ihn basierend auf dem Absender der Payload
                MQTTGameClient.oponnent = payload.sender;
            }

            if (MQTTGameClient.player_number == 1)
            {
                PlayerLabel = "Spieler 1 (Rot)";
            }
            else
            {
                PlayerLabel = "Spieler 2 (Grün)";
            }
            OnPropertyChanged(nameof(PlayerLabel));

            if (payload.sender != MQTTGameClient.clientID)
            {
                MoveLabel = "Du bist am Zug!";
            }
            else
            {
                MoveLabel = "Gegner am Zug!";
            }
            OnPropertyChanged(nameof(MoveLabel));
        }

        // Konstruktor der Klasse
        public VierGewinntViewModel()
        {
            // Initialisierung der MQTT-Verbindung und des Spielfelds
            mq = new MQTTGameClient("4gewinnt", PayloadHandler);
            Connect_Four.initialiazeField();
            Connect_Four.CurrentPlayer = MQTTGameClient.player_number;
            Connect_Four.Field_X = 7;
            Connect_Four.Field_Y = 5;

            PlayerLabel = "Suche Spieler..."; // Anfangsanzeige
            MoveLabel = "";
            OnNavigatedTo();

            // Event-Handler für das Beenden der Anwendung
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        // Handler für das Beenden der Anwendung
        static void OnProcessExit(object sender, EventArgs e)
        {
            // Dieser Code wird automatisch ausgeführt, wenn die Anwendung beendet wird.

            // Überprüfe, ob das Spiel noch läuft und ein Spieler während des Spiels beigetreten ist
            if (!(MQTTGameClient.currentMessage.gamestatus == GameStatus.RUNNING && MQTTGameClient.player_number == 0)
                && MQTTGameClient.game_state != GameStatus.FINISHED)
            {
                // Wenn der Gegner festgelegt wurde und das Spiel noch nicht vorbei ist,
                // sende eine Nachricht, in der der Gegner als Gewinner definiert wird.
                if (MQTTGameClient.oponnent != Guid.Empty && MQTTGameClient.currentMessage.gamestatus == GameStatus.RUNNING)
                {
                    BasePayload payload = new BasePayload();
                    payload.buildGameFinishedMsg(MQTTGameClient.clientID, MQTTGameClient.oponnent, Connect_Four.getGameFieldAsArray());
                    mq.SendPayload(payload);
                }
                else
                {
                    // Andernfalls, wenn der Gegner nicht festgelegt ist, veröffentliche einfach eine leere Nachricht.
                    mq.Publish(null, "4gewinnt");
                }
            }
        }

        // Methode zum Einrichten der MQTT-Verbindung
        public static async void Setup()
        {
            await mq.Setup();
            Thread.Sleep(400); // Warte kurz auf die Einrichtung
            waitBool = true;   // Setze die Warteflagge auf "true", um Benutzeraktionen zuzulassen
        }

        // Methode, die aufgerufen wird, wenn die Ansicht navigiert wird
        public void OnNavigatedTo()
        {
            GenerateGrid(6, 7); // Generiere das Spielbrett
        }

        // Methode zum Generieren des Spielbretts (Rasters)
        public void GenerateGrid(int rows, int columns)
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

            ViewGeneratedGrid = gameGrid;

            // Erstelle Schaltflächen (Buttons) für das Einwerfen von Spielsteinen
            for (int col = 0; col < columns; col++)
            {
                Button dropButton = new Button();
                dropButton.Content = "*";

                // Setze den Stil des Buttons (aus den Anwendungsressourcen)
                dropButton.Style = (Style)Application.Current.Resources["DarkButtonStyle"];

                dropButton.VerticalAlignment = VerticalAlignment.Stretch;
                dropButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                dropButton.Margin = new Thickness(20);
                dropButton.Tag = col;
                dropButton.FontSize = 24;
                dropButton.Click += (sender, e) => DropButton_Click(dropButton.Tag);
                gameGrid.Children.Add(dropButton);
                Grid.SetRow(dropButton, 0);
                Grid.SetColumn(dropButton, col);
            }

            circlesArray = new Ellipse[rows, columns];

            // Erstelle Ellipsen für das Spielfeld
            for (int row = 1; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Ellipse blackCircle = new Ellipse();
                    blackCircle.Fill = Brushes.DarkBlue;
                    blackCircle.VerticalAlignment = VerticalAlignment.Stretch;
                    blackCircle.HorizontalAlignment = HorizontalAlignment.Stretch;
                    blackCircle.Margin = new Thickness(17, 15, 17, 15);
                    gameGrid.Children.Add(blackCircle);
                    Grid.SetRow(blackCircle, row);
                    Grid.SetColumn(blackCircle, col);

                    circlesArray[row, col] = blackCircle;
                }
            }
        }

        // Methode zum Aktualisieren der GUI basierend auf dem aktuellen Spielstatus
        public static void UpdateGUI()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Führe je nach Spielstatus entsprechende Aktionen aus
                switch (gameResult)
                {
                    case GameResult.Won:
                        App.MainViewModel.NavigateToPage(new WinMessageViewModel(), "Ende des Spiels");
                        break;
                    case GameResult.Draw:
                        App.MainViewModel.NavigateToPage(new DrawMessageViewModel(), "Ende des Spiels");
                        break;
                    case GameResult.Running:
                        if (MQTTGameClient.currentMessage.gamefield != null)
                        {
                            if (
                                (MQTTGameClient.currentMessage.winner != MQTTGameClient.clientID) &&
                                (MQTTGameClient.currentMessage.winner != Guid.Empty)
                            )
                            {
                                MQTTGameClient.game_state = GameStatus.FINISHED;
                                App.MainViewModel.NavigateToPage(new LoseMessageViewModel(), "Ende des Spiels");
                            }
                            else if (
                                (MQTTGameClient.currentMessage.gamestatus == GameStatus.FINISHED) &&
                                (MQTTGameClient.currentMessage.winner == Guid.Empty)
                            )
                            {
                                MQTTGameClient.game_state = GameStatus.FINISHED;
                                App.MainViewModel.NavigateToPage(new DrawMessageViewModel(), "Ende des Spiels");
                            }
                            else if (
                                (MQTTGameClient.currentMessage.gamestatus == GameStatus.FINISHED) &&
                                (MQTTGameClient.currentMessage.winner == MQTTGameClient.clientID)
                            )
                            {
                                MQTTGameClient.game_state = GameStatus.FINISHED;
                                App.MainViewModel.NavigateToPage(new WinMessageViewModel(), "Der Gegner hat aufgegeben");
                            }
                            else
                            {
                                // Aktualisiere das Spielfeld basierend auf den empfangenen Spielinformationen
                                Connect_Four.setGamefieldFromArray(MQTTGameClient.currentMessage.gamefield);

                                int[][] GameField = Connect_Four.getGameFieldAsArray();
                                for (int i = 0; i < 5; i++)
                                {
                                    for (int j = 0; j < 7; j++)
                                    {
                                        if (GameField[i][j] == 0) circlesArray[i + 1, j].Fill = Brushes.DarkBlue;
                                        else if (GameField[i][j] == 1) circlesArray[i + 1, j].Fill = Brushes.Red;
                                        else if (GameField[i][j] == 2) circlesArray[i + 1, j].Fill = Brushes.Green;
                                    }
                                }
                            }
                        }
                        break;
                }
            });
        }

        // Handler für das Klicken auf einen Drop-Button
        private void DropButton_Click(object circle)
        {
            if (waitBool == true)
            {
                int column = (int)circle;
                // Diese if-Anweisung überprüft, ob das Spiel noch läuft und die aktuelle Nachricht nicht von Ihnen stammt
                if (MQTTGameClient.game_state == GameStatus.RUNNING && MQTTGameClient.currentMessage.sender != MQTTGameClient.clientID)
                {
                    Connect_Four.CurrentPlayer = MQTTGameClient.player_number;
                    Connect_Four.setGamefieldFromArray(MQTTGameClient.currentMessage.gamefield);

                    if (Connect_Four.SetStonePossible(column))
                    {
                        gameResult = Connect_Four.SetStone(column);
                        // Sende eine neue Nachricht, die den aktuellen Spielstatus enthält
                        BasePayload newMessage = MQTTGameClient.currentMessage;
                        switch (gameResult)
                        {
                            case GameResult.Won:
                                MQTTGameClient.game_state = GameStatus.FINISHED;
                                newMessage.buildGameFinishedMsg(MQTTGameClient.clientID, Connect_Four.getGameFieldAsArray());
                                break;
                            case GameResult.Draw:
                                MQTTGameClient.game_state = GameStatus.FINISHED;
                                newMessage.buildGameDrawMsg(MQTTGameClient.clientID, Connect_Four.getGameFieldAsArray());
                                break;
                            case GameResult.Running:
                                newMessage.buildGameRunningMsg(MQTTGameClient.clientID, Connect_Four.getGameFieldAsArray());
                                break;
                        }
                        mq.SendPayload(newMessage);
                    }
                }
            }
        }
    }
}
