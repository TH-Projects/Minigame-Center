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

namespace MQTT_Event_Driven
{
    internal class Program
    {
        static MQTTGameClient mq = new MQTTGameClient("4gew", MQTTGameClient.GamePayloadHandlingPrototype);
        static void Main(string[] args)
        {
            Init();

            while (true) { }
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
        }
    }
}
