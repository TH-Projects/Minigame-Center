using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using MQTTnet;

namespace MQTT_Event_Driven.MQTTClient
{
    public enum GameStatus
    {
        NO_OPPONENT,
        RUNNING,
        FINISHED
    }
    public class BasePayload
    {
        public bool hasOpponent { get; set; }
        public string gamefield { get; set; }
        public GameStatus gamestatus { get; set; }
        public Guid winner { get; set; }
        public DateTime timestamp { get; set; }

        public void buildBasicMsg()
        {
            hasOpponent = false;
            gamestatus = GameStatus.NO_OPPONENT;
            timestamp = DateTime.Now;
        }

        public string toString()
        {
            return JsonSerializer.Serialize<BasePayload>(this);
        }
    }

}
