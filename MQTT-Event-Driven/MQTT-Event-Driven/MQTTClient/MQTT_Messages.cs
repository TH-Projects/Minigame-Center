using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using MQTTnet;
using System.Diagnostics.Tracing;

namespace MQTT_Event_Driven.MQTTClient
{
    public enum GameStatus
    {
        NO_RESPONSE,
        NO_OPPONENT,
        RUNNING,
        FINISHED
    }
    public class BasePayload
    {
        public int[,] gamefield { get; set; }
        public GameStatus gamestatus { get; set; }

        public Guid sender{ get; set; }
        public Guid winner { get; set; }
        public DateTime timestamp { get; set; }

        public void buildNoOpponentMsg(Guid sender)
        {
            this.sender = sender;
            gamestatus = GameStatus.NO_OPPONENT;
            timestamp = DateTime.Now;
        }

        public void buildGameRunningMsg(Guid sender)
        {
            this.sender = sender;
            gamefield = null;
            gamestatus = GameStatus.RUNNING;
            timestamp = DateTime.Now;
        }

        public void buildGameRunningMsg(Guid sender, int[,] gamefield)
        {
            this.sender = sender;
            this.gamefield = gamefield;
            gamestatus = GameStatus.RUNNING;
            timestamp = DateTime.Now;
        }

        public void buildGameFinishedMsg(Guid sender)
        {
            this.sender = sender;
            gamestatus = GameStatus.FINISHED;
            timestamp = DateTime.Now;
        }

        public void buildGameFinishedMsg(Guid sender, Guid winner)
        {
            this.sender = sender;
            gamestatus = GameStatus.FINISHED;
            this.winner = winner;
            timestamp = DateTime.Now;
        }
        public string toString()
        {
            return JsonSerializer.Serialize<BasePayload>(this);
        }
    }

}
