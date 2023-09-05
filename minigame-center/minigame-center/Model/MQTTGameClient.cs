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
    /// Diese Klasse handelt Aktionen, die ausgeführt werden sollen wenn ein Gegenspieler schon definiert wurde.
    /// So könnte das Update der UI in dieses Verlagert werden.
    /// <param name="payload"></param>
    public delegate void OnGamePayloadRecieved(BasePayload payload);

    public class MQTTGameClient : MqttBaseClient
    {
        /// Speichert den Spieleverlauf des Spiels
        public static GameStatus game_state = GameStatus.NO_RESPONSE;

        /// Speichert den Namen des Topics über dem die Nachrichten ausgetauscht werden
        public static string game_topic;

        /// Speichert die letzte Nachricht gesetzt über die HandleMessageReceived Funktion
        public static BasePayload currentMessage { get; set; }

        /// Speichert die UUID des Gegners und wird über den Handshake in der Setupfunktion
        /// gesetzt
        static public Guid oponnent { get; set; }


        /// Speichert die UUID des Senders der letzten Nachricht und wird in der HandleMessageReceived Funktion gesetzt
        static Guid senderID;

        /// Speichert um welchen Spieler es sich handelt bei zwei Spielen ist der erste, der in das GameTopic zuerst geschrieben hat
        /// Der zweite ist, der Client, der auf die Nachricht des ersten Spielers reagiert hat.
        public static int player_number = 0;

        /// Speichert die Referenze zum GamePayload Handler und wird in  HandleMessageReceived ausgeführt
        private static OnGamePayloadRecieved GamePayloadHandler;


        /// Verarbeitet eingehende Nachrichten
        /// <param name="sender">Enthält eine Referenz zum sendenden Obejkt dem MQTT Basic Client</param>
        /// <param name="e">Enthält die Nachricht des MQTT Clients</param>
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
                    GamePayloadHandler(currentMessage);
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

        /// Der MQTT Game Client bassiert auf einem rudimentären MQTT Client und fügt weitere Funktionen 
        /// spezifisch für Spiele hinzu. 
        /// <param name="game">Das Topic über das das Spiel ausgehandelt werden soll</param>
        /// <param name="handler">Die Funktion, die ausgeführt werden soll wenn das Spiel begonnen hat</param>
        public MQTTGameClient(string game, OnGamePayloadRecieved handler) : base()
        {
            MessageReceived += HandleMessageReceived;
            GamePayloadHandler += handler;
            MQTTGameClient.game_topic = game;
        }

        /// Verbindet sich mit dem MQTT-Broker, Subscribed auf das MQTT-Topic in dem die Spielerdaten
        /// ausgetauscht werden und defineirt den Gegenspieler. Sollte am Anfang ausgeführt werden
        /// bevor ein Spiel beginnt.
        public async Task Setup()
        {
            try
            {
                Console.WriteLine("Setting up game connection");
                Console.WriteLine($"ClientID: {clientID.ToString()}");


                await Connect("25aee1926b284dfeb459111a517f7201.s2.eu.hivemq.cloud", 8883, "minigame_inf22_dhbw", "Or4Q9IkA0IPLBWpbupwr");

                // Subscribed auf das Topic in dem Spiele Daten ausgetauscht werden
                await Subscribe(game_topic);

                // Wartet auf eine eingehende Nachricht
                Thread.Sleep(2000);

                //HANDSHAKE
                if (currentMessage == null) //Player number 1 first message
                {
                    player_number = 1;
                    var Payload = new BasePayload();
                    game_state = GameStatus.NO_OPPONENT;
                    Payload.buildNoOpponentMsg(clientID);
                    await SendPayload(Payload);
                }
                else if (currentMessage.gamestatus == GameStatus.NO_OPPONENT && currentMessage.sender != clientID && currentMessage.gamestatus != GameStatus.RUNNING)  //Player number 2 response to first message 
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
       
        /// Sendet einen BasePayload mit definierten Attributen für Spiele mit 2D Arrays
        public async Task SendPayload(BasePayload payload)
        {
            await Publish(payload.toString(), game_topic);
        }
    }
}

