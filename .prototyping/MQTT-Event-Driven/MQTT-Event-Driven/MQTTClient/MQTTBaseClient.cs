using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using System;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTT_Event_Driven
{
    public abstract class MqttBaseClient
    {
        private readonly IMqttClient _mqttClient;
        protected Guid clientID;

        public event EventHandler<MqttApplicationMessageReceivedEventArgs> MessageReceived;

        public Guid ClientID { get; }

        public MqttBaseClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            clientID = Guid.NewGuid();

            _mqttClient.ApplicationMessageReceivedAsync += async (e) =>
            {
                Console.WriteLine($"Received message on topic on Main {e.ApplicationMessage.Topic} from ClientID {e.ClientId} : {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");

                // Trigger the event when a message is received
                MessageReceived?.Invoke(this, e);
            };
        }

        public async Task Publish(string message, string topic)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(message)
                    .WithRetainFlag(true)     // Retain Flag belässt Nachricht im Topic
                    .Build();

                MqttClientPublishResult publishResult = await _mqttClient.PublishAsync(applicationMessage);

                if (publishResult.ReasonCode == MqttClientPublishReasonCode.Success)
                {
                    Console.WriteLine($"Deine Nachricht: {message}");
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

        public async Task Subscribe(string topic)
        {
            try
            {
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        public async Task Connect(string broker, int port, string username, string password)
        {
            // Create MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                .WithTcpServer(broker, port)
                .WithCredentials(username, password)
                .WithClientId(clientID.ToString())
                .WithTls(
                o =>
                {
                    o.CertificateValidationHandler = _ => true;

                    o.SslProtocol = SslProtocols.Tls12;

                })
                .WithCleanSession()
                .Build();
            try
            {
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