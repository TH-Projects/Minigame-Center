using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;

namespace MQTT_Event_Driven
{
    class MessageData
    {
        public string senderId { get; set; }
        public string messageType { get; set; }
        public string content { get; set; }

    }
    public class MqttClientService
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptionsBuilder _optionsBuilder;
        private readonly Guid clientID;

        public MqttClientService()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _optionsBuilder = new MqttClientOptionsBuilder();
            clientID = Guid.NewGuid();
        }

        public async Task Publish(string message, string topic)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(message)
                    //.WithRetainFlag()     // Retain Flag belässt Nachricht im Topic
                    .Build();

                MqttClientPublishResult publishResult = await _mqttClient.PublishAsync(applicationMessage);

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

        public async Task Subscribe(string topic, string uuid)
        {
            try
            {
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());

                _mqttClient.ApplicationMessageReceivedAsync += e =>
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

        public async Task Connect(string broker, int port, string username, string password)
        {
            try
            {


                // Create MQTT client options
                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(broker, port) // MQTT broker address and port
                    .WithCredentials(username, password) // Set username and password
                    .WithClientId(clientID.ToString())
                    .WithCleanSession()
                    .WithTls(
                    o =>
                    {
                        o.CertificateValidationHandler = _ => true;

                        o.SslProtocol = SslProtocols.Tls12;

                    })
                    .Build();

                await _mqttClient.ConnectAsync(options, CancellationToken.None);
                Console.WriteLine("Verbunden mit MQTT Broker!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        public async Task Disconnect()
        {
            try
            {
                await _mqttClient.DisconnectAsync();
                Console.WriteLine("Verbindung getrennt!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }
    }
}