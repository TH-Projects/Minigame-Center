using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTnet_Kommunikation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string brokerAddress = "localhost";
            int brokerPort = 1883;
            string topic = "test";

            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerAddress, brokerPort)
                .Build();

            Connect(mqttClient, options);

            // Wait a bit to ensure the connection is established.
            Thread.Sleep(2000);

            string message = "Hello, MQTT!";
            Publish(mqttClient, message, topic);

            Subscribe(mqttClient, topic);
            string userInput;
            do
            {
                Console.WriteLine("S: zum Nachrichten Schreiben. E: Zum Beenden");
                userInput = Console.ReadLine();
                if (userInput == "S")
                {
                    Console.WriteLine("Gib nun deine Nachricht ein: ");
                    message = Console.ReadLine();
                    Publish(mqttClient, message, topic);
                }
           
            } while (userInput != "E");
           
                
            

            Console.ReadLine(); // Wait for user input to keep the application running.
            Disconnect(mqttClient);
        }

        public static async Task Publish(IMqttClient mqttClient, string message, string topic)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(message)
                    .WithRetainFlag()
                    .Build();

                MqttClientPublishResult publishResult = await mqttClient.PublishAsync(applicationMessage);

                if (publishResult.ReasonCode == MqttClientPublishReasonCode.Success)
                {
                    Console.WriteLine($"Nachricht erfolgreich gepublished! Topic = {topic}, Payload = {message}");
                }
                else
                {
                    Console.WriteLine($"Fehler beim Publishen der Nachricht: {publishResult.ReasonString}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        public static async Task Subscribe(IMqttClient mqttClient, string topic)
        {
            try
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine("Received application message.");
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    Console.WriteLine($"Empfangene Nachricht: Thema = {e.ApplicationMessage.Topic}, Payload = {payload}");
                    return Task.CompletedTask;
                };
            }



            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        public static async Task Connect(IMqttClient mqttClient, MqttClientOptions options)
        {
            try
            {
                await mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
            Console.WriteLine("Verbunden mit MQTT Broker!");
        }

        public static async Task Disconnect(IMqttClient mqttClient)
        {
            try
            {
                await mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
            Console.WriteLine("Verbindung getrennt!");
        }
    }
}