using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using minigame_center.Model.MQTTClient;
using minigame_center.Model.Payload;

namespace minigame_center.ViewModel
{
    public class VierGewinntViewModel : BaseViewModel
    {
        
        public static MQTTGameClient _mq = new MQTTGameClient("4gewinnt", PayloadHandler);

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
            
        }
    }
}