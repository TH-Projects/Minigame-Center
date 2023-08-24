using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlTypes;
using System.Threading;
using MQTTnet.Client;
using MQTT_Event_Driven.MQTTClient;
using Game_Logic;

namespace MQTT_Event_Driven
{
    internal class Program
    {
        static MQTTGameClient mq = new MQTTGameClient("4gewinnt", GamePayloadHandlingPrototype);
        static void Main(string[] args)
        {
            Init();

            while (true) { }
        }

        static public async void GamePayloadHandlingPrototype(BasePayload inputMessage)
        {
            Console.Out.WriteLineAsync("game payload handling function was called!");
            //at this point it is verified that gamestatus == running
            //and that the message comes from the opponent
            GameResult gameResult = GameResult.Running;

            //this function supports multiple games
            if (MQTTGameClient.game_topic == "4gewinnt")
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
                        newMessage.buildGameFinishedMsg(mq.ClientID);
                        break;
                    case GameResult.Draw:
                        newMessage.buildGameFinishedMsg(mq.ClientID);
                        break;
                    case GameResult.Running:
                        newMessage.buildGameRunningMsg(mq.ClientID, connect_Four.getGameFieldAsArray());
                        break;
                }
                //need to send new payload
                mq.SendPayload(newMessage);


            }
        }

        // Event handler for received messages
        static void HandleMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Received message on topic on Main {e.ApplicationMessage.Topic} from ClientID {e.ClientId} : {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            // Add your custom logic to handle the received message
        }
        
        static async void Init()
        {
            ConfigManager.BuildConfig();
            await mq.Setup();
            int[][] field = {           // Initial Matrix for test cases
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 1, 0, 0},
            };
        }
    }
}
