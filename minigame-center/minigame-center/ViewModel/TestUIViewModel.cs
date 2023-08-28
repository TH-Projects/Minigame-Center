using minigame_center.HelperClasses;
using minigame_center.Model.MQTTClient;
using minigame_center.Model.Payload;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;

namespace minigame_center.ViewModel
{
    internal class TestUIViewModel : BaseViewModel
    {

        string firstLabelHeadline = "Error";
        public string FirstLabelHeadline
        {
            get => firstLabelHeadline;
            set
            {
                if (FirstLabelHeadline != value)
                {
                    firstLabelHeadline = value;
                    OnPropertyChanged(nameof(FirstLabelHeadline));
                }
            }
        }
        string secondLabelHeadline = "Success";
        public string SecondLabelHeadline
        {
            get => secondLabelHeadline;
            set
            {
                if (SecondLabelHeadline != value)
                {
                    secondLabelHeadline = value;
                    OnPropertyChanged(nameof(SecondLabelHeadline));
                }
            }
        }
        public DelegateCommand OptionalCommand { get; set; }
        string buttonContent = "Click here";
        public string ButtonContent
        {
            get => buttonContent;
            set
            {
                if (ButtonContent != value)
                {
                    buttonContent = value;
                    OnPropertyChanged(nameof(ButtonContent));
                }
            }
        }
        string bigBoxOne;
        public string BigBoxOne
        {
            get => bigBoxOne;
            set
            {
                if (BigBoxOne != value)
                {
                    bigBoxOne = value;
                    OnPropertyChanged(nameof(BigBoxOne));
                }
            }
        }
        string bigBoxTwo;
        public string BigBoxTwo
        {
            get => bigBoxTwo;
            set
            {
                if (BigBoxTwo != value)
                {
                    bigBoxTwo = value;
                    OnPropertyChanged(nameof(BigBoxTwo));
                }
            }
        }

        public int[][] GameField { get; set; }

        public TestUIViewModel()    //Constructor
        {
            Guid uuidLocal = Guid.NewGuid();
            string uuidString = uuidLocal.ToString();


            int localMessageCount = 0;


            GameField = new int[2][];

            for (int i = 0; i<2; i++)
            {
                GameField[i] = new int[2];
                for (int j = 0; j<2; j++)
                {
                     GameField[i][j] = 0;

                }
            }

            // Configuration settings for test reasons hardcoded
            string Topic = "TestUI3";
            int brokerPort = 8883;
            string brokerAddress = "25aee1926b284dfeb459111a517f7201.s2.eu.hivemq.cloud";
            var username = "minigame_inf22_dhbw";
            var password = "Or4Q9IkA0IPLBWpbupwr";

            MqttBaseClient mqttBaseClient = new MqttBaseClient();
            mqttBaseClient.Connect(brokerAddress, brokerPort, username, password);
            Thread.Sleep(2000);

            var currentMessage = new BasePayload();

            BigBoxOne = "";
            BigBoxTwo = "";

            mqttBaseClient.Subscribe(Topic);

            mqttBaseClient.MessageReceived += (sender, e) => {
                BigBoxOne = "";
                BigBoxTwo = "";

                string receivedString = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                dynamic stringDataAsPayload = JsonConvert.DeserializeObject(receivedString);

                if (stringDataAsPayload.sender == uuidLocal)
                {    // Your message
                    BigBoxOne += $"JSON PAYLOAD STRING {receivedString}\n" +
                    $"\nJSON PAYLOAD CONVERTED from your {localMessageCount}th Message: \n{{ \n Gamefield: {stringDataAsPayload.gamefield} " +
                    $"\n Game Status: {stringDataAsPayload.gamestatus} \n Sender: {stringDataAsPayload.sender}" +
                    $"\n Winner: {stringDataAsPayload.winner} \n Timestamp: {stringDataAsPayload.timestamp} \n}}";
                }
                else // other messages
                {
                    BigBoxTwo += $"JSON PAYLOAD STRING {receivedString}\n" +
                    $"\nJSON PAYLOAD CONVERTED: \n{{ \n Gamefield: {stringDataAsPayload.gamefield} " +
                    $"\n Game Status: {stringDataAsPayload.gamestatus} \n Sender: {stringDataAsPayload.sender}" +
                    $"\n Winner: {stringDataAsPayload.winner} \n Timestamp: {stringDataAsPayload.timestamp} \n}}";
                }
            };


            this.OptionalCommand = new DelegateCommand(
               async (o) =>
               {
                   if (localMessageCount == 0) currentMessage.buildNoOpponentMsg(uuidLocal);
                   else if (localMessageCount == 1)
                   {
                       currentMessage.buildGameRunningMsg(uuidLocal, GameField);
                       //currentMessage.buildGameRunningMsg(uuid);
                   }
                   else
                   {
                       currentMessage.buildGameFinishedMsg(uuidLocal);
                   }

                   localMessageCount++;

                   var payloadString = currentMessage.toString();
                   await mqttBaseClient.Publish(payloadString, Topic);
               });
            firstLabelHeadline = "Message you have sent";
            secondLabelHeadline = "Message you received";
            buttonContent = "Click";

        }

        private void MqttBaseClient_MessageReceived(object sender, MQTTnet.Client.MqttApplicationMessageReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

