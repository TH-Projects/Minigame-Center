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
        public  string PlayerLabel { get; set; }
        public  string MoveLabel { get; set; }

        private static bool  waitBool = false;

        
        public static Ellipse[,] circlesArray;
        private Grid gameGrid;

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

        private static GameResult gameResult = GameResult.Running;

        public static MQTTGameClient mq;

        private void PayloadHandler(BasePayload payload)
        {
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
                UpdateGUI();
        }

        public VierGewinntViewModel()
        {              
            mq = new MQTTGameClient("4gewinnt", PayloadHandler);
            Connect_Four.initialiazeField();
            Connect_Four.CurrentPlayer = MQTTGameClient.player_number;
            Connect_Four.Field_X = 7;
            Connect_Four.Field_Y = 5;
            
            PlayerLabel = "Suche Spieler...";
            MoveLabel = "";
            OnNavigatedTo();

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

        }

        static void OnProcessExit(object sender, EventArgs e)  // This function is called automatically when the application is terminated.
        {
            if (!(MQTTGameClient.currentMessage.gamestatus == GameStatus.RUNNING && MQTTGameClient.player_number == 0))
            {
                mq.Publish(null, "4gewinnt");
            }
        }


        public static async void Setup()
        {
            await mq.Setup();
            Thread.Sleep(400);
            waitBool = true;
        }

        public void OnNavigatedTo()
        {
            GenerateGrid(6, 7);
        }

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

            for (int col = 0; col < columns; col++)
            {
                Button dropButton = new Button();
                dropButton.Content = "*";
                dropButton.VerticalAlignment = VerticalAlignment.Stretch;
                dropButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                dropButton.Height = 40;
                dropButton.Margin = new Thickness(5);
                dropButton.Tag = col;
                dropButton.Click += (sender, e) => DropButton_Click(dropButton.Tag);
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
                    blackCircle.VerticalAlignment = VerticalAlignment.Stretch;
                    blackCircle.HorizontalAlignment = HorizontalAlignment.Stretch;
                    blackCircle.Margin = new Thickness(15, 10, 15, 10);
                    gameGrid.Children.Add(blackCircle);
                    Grid.SetRow(blackCircle, row);
                    Grid.SetColumn(blackCircle, col);

                    circlesArray[row, col] = blackCircle;
                }
            }
        }


        public static void UpdateGUI()
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
          
              
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
                            if(
                                (MQTTGameClient.currentMessage.winner != MQTTGameClient.clientID) && 
                                (MQTTGameClient.currentMessage.winner != Guid.Empty)
                            ){
                                App.MainViewModel.NavigateToPage(new LoseMessageViewModel(), "Ende des Spiels");
                            }
                            else if(
                                (MQTTGameClient.currentMessage.gamestatus == GameStatus.FINISHED) && 
                                (MQTTGameClient.currentMessage.winner == Guid.Empty)
                            ){
                                App.MainViewModel.NavigateToPage(new DrawMessageViewModel(), "Ende des Spiels");
                            }
                            else { 
                                Connect_Four.setGamefieldFromArray(MQTTGameClient.currentMessage.gamefield);

                                int[][] GameField = Connect_Four.getGameFieldAsArray();
                                for (int i = 0; i < 5; i++)
                                {
                                    for (int j = 0; j < 7; j++)
                                    {
                                        if (GameField[i][j] == 0) circlesArray[i + 1, j].Fill = Brushes.DarkBlue;
                                        else if (GameField[i][j] == 1) circlesArray[i + 1, j].Fill = Brushes.Red;
                                        else if (GameField[i][j] == 2) circlesArray[i + 1, j].Fill = Brushes.Green; // Änderung von 1 auf 2
                                    }
                                }
                            }
                        }
                        break;
                }
                
                
            });
        }



        private void DropButton_Click(object circle)
        {
            if (waitBool == true)
            {
                int column = (int)circle;
                //This if statement checks if the game is still in RUNNING state and if the current message isn't from yourself
                if (MQTTGameClient.game_state == GameStatus.RUNNING && MQTTGameClient.currentMessage.sender != MQTTGameClient.clientID)
                {
                    Connect_Four.CurrentPlayer = MQTTGameClient.player_number;
                    Connect_Four.setGamefieldFromArray(MQTTGameClient.currentMessage.gamefield);

                    if (Connect_Four.SetStonePossible(column))
                    {
                        gameResult = Connect_Four.SetStone(column);
                        //publish a new message
                        BasePayload newMessage = MQTTGameClient.currentMessage;
                        switch (gameResult)
                        {
                            case GameResult.Won:
                                newMessage.buildGameFinishedMsg(MQTTGameClient.clientID, Connect_Four.getGameFieldAsArray());
                                break;
                            case GameResult.Draw:
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

