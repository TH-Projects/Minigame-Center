using System;
using System.Text.Json;

namespace minigame_center.Model.Payload
{
    public enum GameStatus
    {
        NO_RESPONSE,
        NO_OPPONENT,
        WAITING,
        RUNNING,
        FINISHED
    }
    public class BasePayload
    {
        public int[][] gamefield { get; set; }
        public GameStatus gamestatus { get; set; }

        public Guid sender { get; set; }
        public Guid winner { get; set; }
        public DateTime timestamp { get; set; }

        public void buildNoOpponentMsg(Guid sender)
        {
            this.sender = sender;
            gamestatus = GameStatus.NO_OPPONENT;
            timestamp = DateTime.Now;
        }
        public void buildWaitingMessage(Guid sender)
        {
            this.sender = sender;
            gamestatus = GameStatus.WAITING;
            timestamp = DateTime.Now;
        }

        public void buildGameRunningMsg(Guid sender)
        {
            this.sender = sender;
            int[][] field = {           // Initial Matrix for test cases
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0},
            };
            gamefield = field;
            gamestatus = GameStatus.RUNNING;
            timestamp = DateTime.Now;
        }

        public void buildGameRunningMsg(Guid sender, int[][] gamefield)
        {
            this.sender = sender;
            this.gamefield = gamefield;
            gamestatus = GameStatus.RUNNING;
            timestamp = DateTime.Now;
        }

        public void buildGameFinishedMsg(Guid sender, int[][] gamefield)
        {
            this.sender = sender;
            this.gamefield = gamefield;
            gamestatus = GameStatus.FINISHED;
            this.winner = sender;
            timestamp = DateTime.Now;
        }
        public string toString()
        {
            return JsonSerializer.Serialize<BasePayload>(this);
        }
    }

}
