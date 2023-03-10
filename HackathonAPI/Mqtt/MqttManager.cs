using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApi.Core.Context;

namespace HackathonAPI.Mqtt
{
    public class MqttManager
    {
        readonly MqttFactory mqttFactory = null;
        MqttClient mqttClient = null;

        private readonly IContextManager _contextManager;

        public MqttManager(IContextManager contextManager)
        {
            _contextManager = contextManager;
            mqttFactory = new MqttFactory();
        }

        public async void DisconnectFromMqttServer()
        {
            if (mqttClient != null && mqttClient.IsConnected)
            {
                _contextManager.loggerManager.info("Disconnecting from Mqtt server");
                MqttClientDisconnectOptions mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
                await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
            }
        }

        private async Task<bool> ConnectToMqttServer()
        {
            try
            {
                string MqttUsername = System.Configuration.ConfigurationManager.AppSettings["MqttUsername"];
                string MqttPassword = System.Configuration.ConfigurationManager.AppSettings["MqttPassword"];
                string MqttHost = System.Configuration.ConfigurationManager.AppSettings["MqttHost"];

                if (string.IsNullOrEmpty(MqttHost))
                {
                    throw new Exception("MQTT connect data not set in configuration (MqttHost)");
                }

                if (string.IsNullOrEmpty(MqttPassword) || string.IsNullOrEmpty(MqttUsername))
                {
                    throw new Exception("MQTT Credentials not set in configuration (MqttUsername, MqttPassword)");
                }

                mqttClient = (MqttClient)mqttFactory.CreateMqttClient();
                MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder().WithWebSocketServer(MqttHost).WithCredentials(MqttUsername, MqttPassword).Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                if (!mqttClient.IsConnected)
                {
                    throw new Exception("Could not establish connection to server!");
                }
                _contextManager.loggerManager.info("Connected to Mqtt server");
                return true;
            }
            catch (Exception ex)
            {
                _contextManager.loggerManager.info(ex.Message + " => " + ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> Publish(string topic, string payload)
        {
            try
            {
                if (mqttClient == null || !mqttClient.IsConnected)
                {
                    bool successfulReconnect = await ConnectToMqttServer();
                    if (!successfulReconnect) return false;
                }

                MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                           .WithTopic(topic)
                           .WithPayload(payload)
                           .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                           .WithRetainFlag(false)
                           .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                _contextManager.loggerManager.info("Publishing success");
                return true;
            }
            catch (Exception ex)
            {
                _contextManager.loggerManager.info(ex.Message + " => " + ex.StackTrace);
                return false;
            }
        }
    }
}