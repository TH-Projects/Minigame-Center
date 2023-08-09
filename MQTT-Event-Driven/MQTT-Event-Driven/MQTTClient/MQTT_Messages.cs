using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MQTT_Event_Driven.MQTTClient
{
    public class FistConnectionMessage
    {
        public string uuid { get; set; }
        public string username { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class VierGewinntMessage
    {
        public string uuid { get; set; }
        public string  x_position { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
