using ChatCore.Interfaces;
using ChatCore.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace bSpin.UDP
{
    class NetworkHandler
    {
        private static Thread ListenerThread;

        private static int Port
        {
            get => Configuration.PluginConfig.Instance.UdpPort;
        }
        internal static bool IsOn;
        internal static UdpState state;
        public struct UdpState
        {
            public UdpClient u;
            public IPEndPoint e;
        }
        internal static void Init()
        {
            IPEndPoint e = new IPEndPoint(IPAddress.Any, Port);
            state.u = new UdpClient(e);
            state.e = e;
            Set(Configuration.PluginConfig.Instance.UdpEnabled);
        }
        internal static void Set(bool on)
        {
            switch (on)
            {
                case true:
                    if (!IsOn)
                    {
                        ListenerThread = new Thread(async => ReceiveMessages(state.u, state));
                        ListenerThread.Start();
                        IsOn = true;
                    }
                    Restart();
                    break;
                case false:
                    if (IsOn)
                    {
                        ListenerThread.Abort();
                        ListenerThread.Join();
                        IsOn = false;
                    }
                    break;
            }
        }
        internal static void Restart()
        {
            IPEndPoint e = new IPEndPoint(IPAddress.Any, Port);
            state.u = new UdpClient(e);
            state.e = e;
            if (IsOn)
            {
                ListenerThread.Abort();
                ListenerThread.Join();
            }
            ListenerThread = new Thread(async => ReceiveMessages(state.u, state));
            ListenerThread.Start();
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {

            byte[] receiveBytes = state.u.EndReceive(ar, ref state.e);
            string receiveString = Encoding.ASCII.GetString(receiveBytes);
            Twitch.CommandHandler.HandleMessage(receiveString, new FakeUser("UDP Client"));
            Restart();
        }

        private static void ReceiveMessages(UdpClient u, UdpState s)
        {
            u.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            while (true)
                Thread.Sleep(100);
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
