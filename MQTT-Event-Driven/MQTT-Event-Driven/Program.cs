using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlTypes;
using System.Threading;

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

        static async void Init()
        {
            ConfigManager.BuildConfig();
            await mq.Connect(ConfigManager.Server, ConfigManager.Port, ConfigManager.User, ConfigManager.Password);
            Thread.Sleep(2000);
            while(true)
                await mq.Publish("", "test");
        }
    }
}
