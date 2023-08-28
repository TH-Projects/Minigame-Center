using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using minigame_center.Model.MQTTClient;
using minigame_center.Model.Payload;

namespace minigame_center.ViewModel
{
    public class VierGewinntViewModel : BaseViewModel
    {
        static MQTTGameClient _mq;

        private readonly OnGamePayloadRecieved handler;

        public VierGewinntViewModel()
        {
            handler = PayloadHandler;
            _mq = new MQTTGameClient("4gewinnt", handler);
            _mq.Setup();
        }

        private void PayloadHandler(BasePayload payload)
        {
            throw new NotImplementedException();
        }




    }
}