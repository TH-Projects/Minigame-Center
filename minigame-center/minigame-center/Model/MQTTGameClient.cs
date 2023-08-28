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

namespace minigame_center.Model.MQTTClient
{
    public delegate void OnGamePayloadRecieved(BasePayload payload);

    public class MQTTGameClient : MqttBaseClient
    {
        public static GameStatus game_state = GameStatus.NO_RESPONSE;

        public static string game_topic = "4gewinnt";

        static BasePayload currentMessage;

        static public Guid oponnent { get; set; }

        static Guid senderID;

        public static Guid SenderID { get; }

        private static  OnGamePayloadRecieved GamePayloadHandler;


        /// <summary>
        /// Handels all incommiung messages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected static void HandleMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            string received_payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            if (IsValidJson(received_payload))
            {
                currentMessage = JsonSerializer.Deserialize<BasePayload>(received_payload);

                senderID = currentMessage.sender;

                if (game_state == GameStatus.RUNNING)
                {
                    if (oponnent == senderID)
                    {
                        GamePayloadHandler(currentMessage);
                    }
                }
            }
        }

        public static bool IsValidJson(string input)
        {
            // Simple regular expression to check for curly braces indicative of a JSON object
            // This might not catch all edge cases, but it's a basic and fast check
            string pattern = @"^\s*\{.*\}\s*$";
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// MQTT Client with all recieving and player functionalities.
        /// Requieres a function that can be triggered from a GameController which handles 
        /// The Payload from the oponent.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="handler"></param>
        public MQTTGameClient(String game, OnGamePayloadRecieved handler) : base()
        {
            MessageReceived += HandleMessageReceived;
            GamePayloadHandler += handler;
            MQTTGameClient.game_topic = game;
        }

        /// <summary>
        /// Subscibes to the game_topic and executes a procedure.
        /// When a game is already running the program blocks and throws an IvalidOperation error.
        /// </summary>
        /// <returns></returns>
        public async Task Setup()
        {
            try
            {
                Console.WriteLine("Setting up game connection");
                Console.WriteLine($"ClientID: {clientID.ToString()}");

                await Connect(ConfigManager.Server, ConfigManager.Port, ConfigManager.User, ConfigManager.Password);
                await Subscribe(game_topic);

                await establishOponnent(5);

                if (game_state == GameStatus.NO_RESPONSE)
                {
                    //Handles when no message is received
                    await Console.Out.WriteLineAsync("Waiting for connection");
                    var Payload = new BasePayload();
                    Payload.buildNoOpponentMsg(clientID);
                    await SendPayload(Payload);
                    await establishOponnent(100);

                    if (game_state != GameStatus.RUNNING)
                    {
                        throw new ExternalException("No oponnent found");
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Handshake completed | Player 1: {clientID} | Player 2: {oponnent}");
                    }
                }



            }

            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        private async Task establishOponnent(int maxtrys)
        {
            int trys = 0;
            while (trys < maxtrys)
            {
                if (currentMessage != null && currentMessage is BasePayload)
                {
                    if (currentMessage.gamestatus == GameStatus.RUNNING)
                    {
                        game_state = GameStatus.RUNNING;
                        await Disconnect();
                        await Console.Out.WriteLineAsync($"Game already running. Disconnecting!");
                        throw new InvalidOperationException("Game in Running State");
                    }
                    else if (currentMessage.gamestatus == GameStatus.NO_OPPONENT)
                    {
                        if (senderID != clientID)
                        {
                            oponnent = senderID;
                            await Console.Out.WriteLineAsync($"Game has no Oponnent currently. Updating Ratainmessage. Oponnent {oponnent.ToString()}");
                            game_state = GameStatus.WAITING;
                            var payload = new BasePayload();
                            payload.buildWaitingMessage(clientID);//y ,x 
                            await SendPayload(payload);
                            return;
                        }
                    }
                    else if (currentMessage.gamestatus == GameStatus.WAITING)
                    {
                        if (senderID != clientID)
                        {
                            oponnent = senderID;
                            await Console.Out.WriteLineAsync($"Approving No Opponent Message. Oponnent {oponnent.ToString()}");
                            game_state = GameStatus.RUNNING;
                            var payload = new BasePayload();
                            payload.buildGameRunningMsg(clientID);//y ,x 
                            await SendPayload(payload);
                            return;
                        }
                    }
                    else if (currentMessage.gamestatus == GameStatus.FINISHED)
                    {
                        await Console.Out.WriteLineAsync("Game has finished. Updating Ratainmessage.");
                        game_state = GameStatus.NO_OPPONENT;
                        var payload = new BasePayload();
                        payload.buildNoOpponentMsg(clientID);
                        await SendPayload(payload);
                        await establishOponnent(100);
                        return;
                    }
                }
                Thread.Sleep(500);
                trys++;
            }
        }
        /// <summary>
        /// Sends a deserialised BasePayload to the topic defined above.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task SendPayload(BasePayload payload)
        {
            await Publish(payload.toString(), game_topic);
        }
    }
}

