﻿using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using HiveMQtt.Client;


namespace MQTTnet_Kommunikation
{

    internal class Program
    {
        static List<string> connectedClientSessionIds = new List<string>();
        static void Main(string[] args)
        {

            // Generate a new UUID (Guid)
            Guid uuid = Guid.NewGuid();

            // Convert the Guid to a string representation
            string uuidString = uuid.ToString();

            string brokerAddress = "25aee1926b284dfeb459111a517f7201.s2.eu.hivemq.cloud";
            int brokerPort = 8884;      //1883 ohne TLS
            string topic = "test";

            //var clientId = "YourClientID"; // Ihr Client-ID
            var username = "test1"; // Ihr Client-Nutzer
            var password = "StrongPassword1"; // Ihr Password
           
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerAddress, brokerPort)
                .WithCredentials(username, password)
                .WithClientId(uuidString) //Hier evtl. die UUID mitgeben
                .Build();
            

            Connect(mqttClient);

            // Wait a bit to ensure the connection is established.
            Thread.Sleep(2000);

            string message;

            Subscribe(mqttClient, topic, uuidString);
            string userInput;
            do
            {       
                    message = uuidString  + ": ";
                    //Console.WriteLine("Gib nun deine Nachricht ein: ");
                    message += Console.ReadLine();
                    Publish(mqttClient, message, topic);
                
           
            } while (message != "E");


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
                   // Console.WriteLine($"Nachricht erfolgreich gepublished! Topic = {topic}, Payload = {message}");
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
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    if (!payload.Contains(uuid))
                    {
                        //  Console.WriteLine($">>Empfangene Nachricht: Thema = {e.ApplicationMessage.Topic}, Payload = {payload}");
                        Console.WriteLine($">>Thema = {e.ApplicationMessage.Topic}, Payload = {payload}");
                    }
                    return Task.CompletedTask;
                };
            }



            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Subscribe: {ex.Message}");
            }
        }

        //public static async Task Connect(IMqttClient mqttClient, MqttClientOptions options)
        //{
        //    try
        //    {
        //        await mqttClient.ConnectAsync(options);
        //        Console.WriteLine("Verbunden mit MQTT Broker!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Fehler beim Connect: {ex.Message}");
        //    }

        //}
        public static async Task Connect(IMqttClient mqttClient)
        {
            // Use builder classes where possible in this project.
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("25aee1926b284dfeb459111a517f7201.s2.eu.hivemq.cloud").Build();

            var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            Console.WriteLine("The MQTT client is connected.");

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