using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MQTTnet.Server;


namespace MQTT_PublicBroker_Connection
{
    class MessageData
    {
        public string senderId { get; set; }
        public string messageType { get; set; }
        public string content { get; set; }

    }

    internal class Program
    {
        static List<string> connectedClientSessionIds = new List<string>();
        static void Main(string[] args)
        {
            Guid uuid = Guid.NewGuid();

            string uuidString = uuid.ToString();

            string brokerAddress = "25aee1926b284dfeb459111a517f7201.s2.eu.hivemq.cloud";
            int brokerPort = 8883;      
            string topic = "test";

            var username = "minigame_inf22_dhbw"; 
            var password = "Or4Q9IkA0IPLBWpbupwr"; 

            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                .WithTcpServer(brokerAddress, brokerPort)
                .WithCredentials(username, password)
                .WithTls(
                o =>
                {
                    o.CertificateValidationHandler = _ => true;

                    o.SslProtocol = SslProtocols.Tls12;

                })
                .WithCleanSession()
                .Build();


            Connect(mqttClient, options);

            // Wait a bit to ensure the connection is established
            Thread.Sleep(2000);

            //string message;
            var messageData = new MessageData();
            string messageJSON;


            Subscribe(mqttClient, topic, uuidString);
            string userInput;

            do
            {
                //Console.WriteLine("Gib nun deine Nachricht ein: ");
                messageData.senderId = uuidString;
                messageData.messageType = "JSON-Test";
                messageData.content = Console.ReadLine();

                if (messageData.content == "unsubscribe") Unsubscribe(mqttClient, topic);    


                messageJSON = JsonConvert.SerializeObject(messageData);

                Publish(mqttClient, messageJSON, topic);

                                         
            } while (true);

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
                    //.WithRetainFlag()     // Retain Flag belässt Nachricht im Topic
                    .Build();

                MqttClientPublishResult publishResult = await mqttClient.PublishAsync(applicationMessage);

                if (publishResult.ReasonCode == MqttClientPublishReasonCode.Success)
                {
                    //Console.WriteLine($"Deine Nachricht: {message}");
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

        public static async Task Subscribe(IMqttClient mqttClient, string topic, string uuid)
        {
            try
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    string messageJSON = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                    var messageData = JsonConvert.DeserializeObject<MessageData>(messageJSON);

                    if (messageData.senderId != uuid)
                    {
                        Console.WriteLine($">>Antwort: Topic = {e.ApplicationMessage.Topic}, messageContent = {messageData.content}, messageType = {messageData.messageType}");
                    }
                    return Task.CompletedTask;
                };
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        public static async Task Unsubscribe(IMqttClient mqttClient, string topic)
        {
            try
            {
                Console.WriteLine($"Unsubscribe from topic: {topic}");
                await mqttClient.UnsubscribeAsync(topic);
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
                Console.WriteLine("Verbunden mit MQTT Broker!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        public static async Task Disconnect(IMqttClient mqttClient)
        {
            try
            {
                await mqttClient.DisconnectAsync();
                Console.WriteLine("Verbindung getrennt!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }
    }
}