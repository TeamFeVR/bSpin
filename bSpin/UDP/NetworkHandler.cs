using ChatCore.Interfaces;
using ChatCore.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace bSpin.UDP
{
    class NetworkHandler
    {
        private static int Port
        {
            get => Configuration.PluginConfig.Instance.UdpPort;
        }

        internal static UdpClient listener;
        internal static void StartListener()
        {
            listener = new UdpClient(Port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, Port);
            try
            {
                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received broadcast from {groupEP} :");
                    Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");

                    Twitch.CommandHandler.HandleMessage($"{Encoding.ASCII.GetString(bytes, 0, bytes.Length)}", new FakeUser("UDP Client"));
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
        }

        private class FakeMessage : IChatMessage
        {
            public string Id => throw new NotImplementedException();

            public bool IsSystemMessage => throw new NotImplementedException();

            public bool IsActionMessage => throw new NotImplementedException();

            public bool IsHighlighted => throw new NotImplementedException();

            public bool IsPing => throw new NotImplementedException();

            public string Message {get; set;}

            public IChatUser Sender { get; set; }

            public IChatChannel Channel => (Twitch.CommandHandler.twitchService).Channels.ElementAt(0).Value;

            public IChatEmote[] Emotes => throw new NotImplementedException();

            public ReadOnlyDictionary<string, string> Metadata => throw new NotImplementedException();

            public JSONObject ToJson()
            {
                throw new NotImplementedException();
            }

            public FakeMessage(string msg, string username)
            {
                this.Message = msg;
                this.Sender = new FakeUser(username);
            }

        }
        private class FakeUser : IChatUser
        {
            public string Id => throw new NotImplementedException();

            public string UserName { get; set; }

            public string DisplayName { get; set; }

            public string Color => throw new NotImplementedException();

            public bool IsBroadcaster => true;

            public bool IsModerator => throw new NotImplementedException();

            public IChatBadge[] Badges => throw new NotImplementedException();

            public JSONObject ToJson()
            {
                throw new NotImplementedException();
            }

            public FakeUser(string name)
            {
                this.DisplayName = name;
                this.UserName = name.Replace(" ", "").ToLower();
            }

        }
    }
}
