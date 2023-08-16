using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;



namespace MQTT_Event_Driven.MQTTClient
{

    internal class MQTTGameClient : MqttBaseClient
    {
        public const string game_topic = "4gew";

        static MqttApplicationMessageReceivedEventArgs currentMessage;

        static void HandleMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            currentMessage = e;
        }

        
        public MQTTGameClient(String game) : base()
        {
            MessageReceived += HandleMessageReceived;
        }

        public async Task Setup()
        {
            try
            {
                int trys = 0;
                await Connect(ConfigManager.Server, ConfigManager.Port, ConfigManager.User, ConfigManager.Password);
                await Subscribe(game_topic);
                
                while(trys < 5){ 
                    if(currentMessage != null)
                    {
                        // Handles when a message is recieved
                    }
                    Thread.Sleep(500);
                }
                //Handles when no message is received
                if (currentMessage == null)
                {
                    var Payload = new BasePayload();
                    Payload.buildBasicMsg();
                    await SendPayload(Payload);
                }
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
