using Microsoft.Extensions.Configuration;
using System.IO;

namespace minigame_center.HelperClasses
{
    public static class ConfigManager
    {
        static public IConfiguration config;

        static public string Server;
        static public string User;
        static public string Password;
        static public int Port;


        public static void BuildConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config = builder.Build();
            Server = config["MQTT-Broker:Server"];
            Port = int.Parse(config["MQTT-Broker:Port"]);
            Password = config["MQTT-Broker:Password"];
            User = config["MQTT-Broker:User"];
        }


    }
}
