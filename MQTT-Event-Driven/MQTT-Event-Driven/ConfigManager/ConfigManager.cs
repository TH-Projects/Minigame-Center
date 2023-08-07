using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace MQTT_Event_Driven
{
    public static class ConfigManager
    {
        static public IConfiguration config;

        static public string Server;
        static public string User;
        static public string Password;
        static public string Port;
        
        
        public static void BuildConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config = builder.Build();
            Server = config["MQTT-Broker:Server"];
            Port = config["MQTT-Broker:Port"];
            Password = config["MQTT-Broker:Password"];
            User = config["MQTT-Broker:User"];
        }

        
    }
}
