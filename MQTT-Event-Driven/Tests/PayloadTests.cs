using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using MQTT_Event_Driven.MQTTClient;
using MQTT_Event_Driven;

namespace Tests
{
    [TestClass]
    public class PayloadTests
    {
        [TestMethod]
        public void EqualObjects()
        {
            var basepayload = new BasePayload();
            basepayload.buildNoOpponentMsg();
            var basepayload2 = new BasePayload();
            basepayload.buildNoOpponentMsg();
            Assert.AreEqual(basepayload, basepayload2);
        }
        [TestMethod]
        public void UnEqualObjects()
        {
            var basepayload = new BasePayload();
            basepayload.buildNoOpponentMsg();
            var basepayload2 = new BasePayload();
            basepayload2.buildGameRunningMsg();
            Assert.AreNotEqual(basepayload, basepayload2);
        }
        [TestMethod]
        public void ParsingJson()
        {

            string json = "{ \"hasOpponent\":false,\"gamefield\":null,\"gamestatus\":0,\"winner\":\"00000000-0000-0000-0000-000000000000\",\"timestamp\":\"2023-08-16T08:48:40.476998-07:00\"}";
            
            BasePayload serialized = JsonSerializer.Deserialize<BasePayload>(json);
            var basepayload2 = new BasePayload();
            basepayload2.buildNoOpponentMsg();
            Assert.AreEqual(basepayload2.gamestatus, serialized.gamestatus);
        }

        [TestMethod]
        public void ValidJson()
        {

            string json = "{ \"hasOpponent\":false,\"gamefield\":null,\"gamestatus\":0,\"winner\":\"00000000-0000-0000-0000-000000000000\",\"timestamp\":\"2023-08-16T08:48:40.476998-07:00\"}";
            bool test = MQTTGameClient.IsValidJson(json);

            Assert.AreEqual(basepayload2.gamestatus, serialized.gamestatus);
        }
    }
}
