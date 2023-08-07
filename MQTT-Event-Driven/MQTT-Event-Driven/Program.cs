using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlTypes;


namespace MQTT_Event_Driven
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Init();
            while (true) { }
        }

        static async void Init()
        {
            ConfigManager.BuildConfig();
            var mq = new MqttClientService();
            await mq.Connect(ConfigManager.Server, ConfigManager.Port, ConfigManager.User, ConfigManager.Password);
        }
    }
}
