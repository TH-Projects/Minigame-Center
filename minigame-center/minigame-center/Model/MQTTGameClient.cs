using MQTTnet.Client;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using minigame_center.HelperClasses;
using minigame_center.Model.Payload;
using MQTTnet.Server;
using minigame_center.ViewModel;

namespace minigame_center.Model.MQTTClient
{
    public delegate void OnGamePayloadRecieved(BasePayload payload);

    public class MQTTGameClient : MqttBaseClient
    {
        public static GameStatus game_state = GameStatus.NO_RESPONSE;

        public static string game_topic;

        public static BasePayload currentMessage { get; set; }

        static public Guid oponnent { get; set; }

        static Guid senderID;
        
        public static int player_number { get; set; }

        private static OnGamePayloadRecieved GamePayloadHandler;


     
        public static void HandleMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            string received_payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            if (IsValidJson(received_payload))
            {
                currentMessage = JsonSerializer.Deserialize<BasePayload>(received_payload);

                senderID = currentMessage.sender;

                if(game_state == GameStatus.NO_OPPONENT && currentMessage.sender != clientID) //Player number 1 gets the response from number 2 and sets himself to Status RUNNING
                {
                    game_state = GameStatus.RUNNING;
                    
                }

                if (game_state == GameStatus.RUNNING)
                {                 
                    VierGewinntViewModel.UpdateGUI();
                    
                    if (oponnent == senderID)
                    {
                        GamePayloadHandler(currentMessage);
                    }
                }


            }
            else currentMessage = null;
        }

        public static bool IsValidJson(string input)
        {
            // Simple regular expression to check for curly braces indicative of a JSON object
            // This might not catch all edge cases, but it's a basic and fast check
            string pattern = @"^\s*\{.*\}\s*$";
            return Regex.IsMatch(input, pattern);
        }

        public MQTTGameClient(string game, OnGamePayloadRecieved handler) : base()
        {
            MessageReceived += HandleMessageReceived;
            GamePayloadHandler += handler;
            MQTTGameClient.game_topic = game;
        }

        public async Task Setup()
        {
            try
            {
                Console.WriteLine("Setting up game connection");
                Console.WriteLine($"ClientID: {clientID.ToString()}");


                await Connect("25aee1926b284dfeb459111a517f7201.s2.eu.hivemq.cloud", 8883, "minigame_inf22_dhbw", "Or4Q9IkA0IPLBWpbupwr");

                await Subscribe(game_topic);
                Thread.Sleep(2000);

                //HANDSHAKE
                if (game_state == GameStatus.NO_RESPONSE && (currentMessage==null || currentMessage.gamestatus!=GameStatus.NO_OPPONENT)) //Player number 1 first message
                {
                    player_number = 1;
                    var Payload = new BasePayload();
                    game_state = GameStatus.NO_OPPONENT;
                    Payload.buildNoOpponentMsg(clientID);
                    await SendPayload(Payload);
                }
                else if(currentMessage.gamestatus == GameStatus.NO_OPPONENT && currentMessage.sender != clientID)  //Player number 2 response to first message 
                {                                                                                                   //Sets Status RUNNING isn't able to make a turn because of the if statement in the viewmodel
                    player_number = 2;
                    oponnent = senderID;
                    await Console.Out.WriteLineAsync($"Game has no Oponnent currently. Updating Ratainmessage. Oponnent {oponnent.ToString()}");
                    game_state = GameStatus.RUNNING;
                    var payload = new BasePayload();
                    payload.buildGameRunningMsg(clientID);//y ,x 
                    await SendPayload(payload);
                    return;

                }

                await Console.Out.WriteLineAsync($"This Client is: {player_number} - With Client ID: {clientID}");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }
       
        public async Task SendPayload(BasePayload payload)
        {
            await Publish(payload.toString(), game_topic);
        }
    }
}

