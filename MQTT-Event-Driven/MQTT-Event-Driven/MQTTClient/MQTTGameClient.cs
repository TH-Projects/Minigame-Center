﻿using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace MQTT_Event_Driven.MQTTClient
{

    public class MQTTGameClient : MqttBaseClient
    {
        public delegate void OnGamePayloadRecieved(BasePayload payload);

        public static GameStatus game_state = GameStatus.NO_RESPONSE;

        public const string game_topic = "4gewinnt";

        static BasePayload currentMessage;

        static public Guid oponnent {get; set;}

        static Guid senderID;

        private static OnGamePayloadRecieved GamePayloadHandler;

        /// <summary>
        /// Handels all incommiung messages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected static void HandleMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            string received_payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            if (IsValidJson(received_payload)) { 
                currentMessage = JsonSerializer.Deserialize<BasePayload>(received_payload);
                senderID = Guid.Parse(e.ClientId);

                if (game_state == GameStatus.RUNNING)
                {
                    if(e.ClientId == oponnent.ToString()) { 
                        GamePayloadHandler?.Invoke(currentMessage);
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
            GamePayloadHandler = handler;
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
                int trys = 0;
                await Connect(ConfigManager.Server, ConfigManager.Port, ConfigManager.User, ConfigManager.Password);
                await Subscribe(game_topic);

                while (trys < 5){ 
                    if(currentMessage != null && currentMessage is BasePayload)
                    {
                        if (currentMessage.gamestatus == GameStatus.RUNNING)
                        {
                            game_state = GameStatus.RUNNING;
                            await Disconnect();
                            throw new InvalidOperationException("Game in Running State");
                        }
                        else if(currentMessage.gamestatus == GameStatus.NO_OPPONENT)
                        {
                            oponnent = senderID;
                            Console.WriteLine($"Game has no Oponnent currently. Updateing Ratainmessage. Oponnent {oponnent.ToString()}");
                            game_state = GameStatus.RUNNING;
                            var payload = new BasePayload();
                            payload.buildGameRunningMsg(clientID);
                            await SendPayload(payload);
                            break;
                        }else if (currentMessage.gamestatus == GameStatus.FINISHED)
                        {
                            Console.WriteLine("Game has finished. Updateing Ratainmessage.");
                            game_state = GameStatus.NO_OPPONENT;
                            var payload = new BasePayload();
                            payload.buildNoOpponentMsg(clientID);
                            await SendPayload(payload);
                            break;
                        }
                    }
                    Thread.Sleep(500);
                    trys++;
                }

                if(game_state == GameStatus.NO_RESPONSE) { 
                    //Handles when no message is received
                    if (currentMessage == null)
                    {
                        var Payload = new BasePayload();
                        Payload.buildNoOpponentMsg(clientID);
                        await SendPayload(Payload);
                    }
                    Console.WriteLine("No packet received. NoOponnent message published");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
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

