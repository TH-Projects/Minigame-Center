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


namespace MQTT_PublicBroker_Connection
{

    class MessageData
    {
        public string senderId { get; set; }
        public string messageType { get; set; }
        public string content { get; set; }

    }
    

    class MQTTConnection
    {
        const string  brokerAddress = "25aee1926b284dfeb459111a517f7201.s2.eu.hivemq.cloud";
        const int brokerPort = 8883;

        const string username = "minigame_inf22_dhbw";
        const string password = "Or4Q9IkA0IPLBWpbupwr";

        MqttFactory factory;
        IMqttClient mqttClient;
        MqttClientOptionsBuilder options;

        static List<string> connectedClientSessionIds;
        string uuidString;
        public MQTTConnection()
        {
            Guid uuid = Guid.NewGuid();
            uuidString = uuid.ToString();
            connectedClientSessionIds = new List<string>();

            factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

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