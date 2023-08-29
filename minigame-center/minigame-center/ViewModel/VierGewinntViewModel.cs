using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using minigame_center.Model.MQTTClient;
using minigame_center.Model.Payload;
using minigame_center.HelperClasses;

namespace minigame_center.ViewModel
{
    public class VierGewinntViewModel : BaseViewModel
    {
        
        public static MQTTGameClient _mq = new MQTTGameClient("4gewinnt", PayloadHandler);

        public static Connect_Four game_logic = new Connect_Four(8,7,1);

        public VierGewinntViewModel()
        {
            


        }

        private static void PayloadHandler(BasePayload payload)
        {
            throw new NotImplementedException();
        }


        public void GameControls()
        {

        }


        public static void DropButton_Click(object sender, RoutedEventArgs e, int column)
        {
            game_logic.SetStone(column);
            Console.WriteLine(column);
            var payload = new BasePayload();
            payload.buildGameRunningMsg(_mq.ClientID, game_logic.getGameFieldAsArray());
            _mq.SendPayload(payload);
        }
    }
}