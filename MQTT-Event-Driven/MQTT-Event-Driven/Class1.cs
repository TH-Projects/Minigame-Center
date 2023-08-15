using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Event_Driven
{
    internal class Class1
    {
        static void Main(string[] args)
        {
            MqttClientService mqttClientService = new MqttClientService();

            mqttClientService.Connect("25aee1926b284dfeb459111a517f7201.s2.eu.hivemq.cloud", 8883, "minigame_inf22_dhbw", "Or4Q9IkA0IPLBWpbupwr");

            mqttClientService.Subscribe("Test");



            mqttClientService.Publish("Test", "Test");
        }
    }
}
