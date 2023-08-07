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
        }

        static void Init()
        {
            ConfigManager.BuildConfig();
            var mq = new MQ
        }
    }
}
