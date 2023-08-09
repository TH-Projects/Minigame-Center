using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlTypes;
using System.Threading;
using MQTTnet.Client;

namespace MQTT_Event_Driven
{
    internal class Program
    {
        static MqttClientService mq = new MqttClientService();
        static void Main(string[] args)
        {
            Init();
            
            while (true) { }
        }

        // Event handler for received messages
        static void HandleMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Received message on topic on Main {e.ApplicationMessage.Topic}: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            // Add your custom logic to handle the received message
        }

        static async void Init()
        {
            ConfigManager.BuildConfig();
            await mq.Connect(ConfigManager.Server, ConfigManager.Port, ConfigManager.User, ConfigManager.Password);
            mq.MessageReceived += HandleMessageReceived;
            Thread.Sleep(2000);
            while (true) {
                await mq.Subscribe("test");
                await mq.Publish("Deine Mom", "test");
                Thread.Sleep(1000);
            }
        }
    }
}
