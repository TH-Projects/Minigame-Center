using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using minigame_center.Model.MQTTClient;
using minigame_center.Model.Payload;
using minigame_center.HelperClasses;
using System.Net.Http.Headers;
using System.Windows.Media;
using System.Windows.Media.Animation;
using minigame_center.View;

using minigame_center.Model.MQTTClient;
using minigame_center.Model.Payload;


namespace minigame_center.ViewModel
{
    public class VierGewinntViewModel : BaseViewModel
    {

        static public MQTTGameClient mq = new MQTTGameClient("4gewinnt", PayloadHandler);


        public VierGewinntViewModel()
        {


        }

        private static void PayloadHandler(BasePayload payload)
        {
            throw new NotImplementedException();
        }


        public void GameControls()
        {

        }

        /*
namespace GameController

{

    class GameController


    static public async BasePayload calculateResponse(BasePayload inputMessage)

    {

        Console.Out.WriteLineAsync("game payload handling function was called!");

        //at this point it is verified that gamestatus == running

        //and that the message comes from the opponent

        GameResult gameResult = GameResult.Running;

        lastMessage = inputMessage;

 

 

        //this function supports multiple games

        if (inputMessage.game_topic == "4gewinnt")

        {

            //prompt for player input

            //need to figure out how

            int selectedColumn = 4;

 

            //process that input

            Connect_Four connect_Four = new Connect_Four(7, 6, 1);//how to determine Player 1 or 2                 

            connect_Four.setGamefieldFromArray(inputMessage.gamefield);

 

            if (connect_Four.SetStonePossible(selectedColumn))

            {

                gameResult = connect_Four.SetStone(selectedColumn);

            }

 

            //publish a new message

            BasePayload newMessage = inputMessage;

 

            switch (gameResult)

            {

                case GameResult.Won:

                    newMessage.buildGameFinishedMsg(ClientID);

                    break;

                case GameResult.Draw:

                    newMessage.buildGameFinishedMsg(ClientID);

                    break;

                case GameResult.Running:

                    newMessage.buildGameRunningMsg(ClientID, connect_Four.getGameFieldAsArray());

                    break;

            }

            //return the new message to be sent

            return newMessage;

 

        }

    }
}

}*/



        //public static void DropButton_Click(object sender, RoutedEventArgs e, int column)
        public static void DropButton_Click(object sender, RoutedEventArgs e, int column)
        {
            Console.WriteLine("Click button was called");
            column = 1;
            GameResult gameResult = GameResult.Running;
            //check if last message was from opponent
            if (MQTTGameClient.CurrentMessage != null && MQTTGameClient.game_state == GameStatus.RUNNING && MQTTGameClient.CurrentMessage.sender == MQTTGameClient.oponnent)
            {
                Console.WriteLine("i reached the IF statement");
                //process selected column via game logic
                Connect_Four connect_Four = new Connect_Four(7, 6, MQTTGameClient.player_number);
                connect_Four.setGamefieldFromArray(MQTTGameClient.CurrentMessage.gamefield);

                if (connect_Four.SetStonePossible(column))
                {
                    gameResult = connect_Four.SetStone(column);
                }



                //publish a new message

                BasePayload newMessage = MQTTGameClient.CurrentMessage;
                
                    switch (gameResult)
                    {
                        case GameResult.Won:
                            newMessage.buildGameFinishedMsg(MqttBaseClient.ClientID);
                            break;
                        case GameResult.Draw:
                            newMessage.buildGameFinishedMsg(MqttBaseClient.ClientID);
                            break;
                        case GameResult.Running:
                            newMessage.buildGameRunningMsg(MqttBaseClient.ClientID, connect_Four.getGameFieldAsArray());
                            break;
                    }
           



                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (connect_Four.GameField[i, j] == 0) VierGewinnt.circlesArray[i, j].Fill = Brushes.Gray;
                        else if (connect_Four.GameField[i, j] == 1) VierGewinnt.circlesArray[i, j].Fill = Brushes.Red;
                        else if (connect_Four.GameField[i, j] == 1) VierGewinnt.circlesArray[i, j].Fill = Brushes.Green;
                    }
                }


                //send new payload
                mq.SendPayload(newMessage);

                /*game_logic.SetStone(column);
                Console.WriteLine(column);
                var payload = new BasePayload();
                payload.buildGameRunningMsg(_mq.ClientID, game_logic.getGameFieldAsArray());
                _mq.SendPayload(payload);*/
            }
        }
    }
}