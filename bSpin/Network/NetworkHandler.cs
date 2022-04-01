using System;
using System.Threading.Tasks;
using bSpin.Configuration;
using WebSocketSharp;
using Newtonsoft.Json;


namespace bSpin.Network
{
    class NetworkHandler
    {
        private static WebSocket _ws;
        private Task _connectTask;

        public NetworkHandler()
        {
            /*
            _ws = new WebSocket(PluginConfig.Instance.WebsocketURL);
            _ws.OnMessage += WebsocketMessageRecieved;
            _ws.Connect();
            */
        }

        private void WebsocketMessageRecieved(object s, MessageEventArgs e)
        {
            WebsocketMessage msg = JsonConvert.DeserializeObject<WebsocketMessage>(e.Data);
            Plugin.Log.Notice($"hey i got a something: {e.Data}");
            Plugin.Log.Notice($"sender:{msg.Sender}");
            Plugin.Log.Notice($"s: {msg.SpinAction}");
            Plugin.Log.Notice($"w: {msg.WobbleAction}");
        }
        
    }
}
