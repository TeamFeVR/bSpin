using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ChatCore;
using ChatCore.Interfaces;
using static bSpin.Twitch.Types;

namespace bSpin.Twitch
{
    static class CommandHandler
    {
        public static ChatCore.Services.Twitch.TwitchService twitchService;
        private static ChatCoreInstance coreInstance;
        internal static System.Threading.Thread UDPListenerThread = new System.Threading.Thread(async => UDP.NetworkHandler.StartListener());

        internal static void SendMsg(string msg)
        {
            twitchService.SendTextMessage(msg, (twitchService).Channels.ElementAt(0).Value.Name.ToString());
        }

        public static void Start()
        {
            Plugin.Log.Notice("Starting up Twitch");
            coreInstance = ChatCoreInstance.Create();
            twitchService = coreInstance.RunTwitchServices();
            twitchService.OnJoinChannel += login;
            twitchService.OnTextMessageReceived += StreamServiceProvider_OnMessageReceived;
        }

        static void login(IChatService svc, IChatChannel channel)
        {
            Plugin.Log.Notice($"ChatCore currently has {twitchService.Channels.Count().ToString()} Channels joined");
            SendMsg($"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} connected");
        }

        public static void StreamServiceProvider_OnMessageReceived(IChatService svc, IChatMessage msg)
        {
            Console.WriteLine($"{msg.Sender.DisplayName}: {msg.Message}");
            HandleMessage(msg.Message, msg.Sender);
        }

        internal static void HandleMessage(string message, IChatUser user)
        {
            if (message.ToLower().Contains("!debug"))
            {
                SendMsg($"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version}");
                string loadedProfs = Plugin.spinProfiles.Count.ToString();
                SendMsg($"{loadedProfs} Spin profiles loaded!");
                string loadedWobs = Plugin.wobbles.Count.ToString();
                SendMsg($"{loadedWobs} Wobble{(Plugin.wobbles.Count > 1 ? "s" : "")} loaded!");
            }
            else if (message.ToLower().Equals("!wobbles"))
            {
                string woblist = "Wobbles: ";
                foreach (var wob in Plugin.wobbles)
                {
                    woblist += (wob.name) + ", ";
                }
                SendMsg(woblist);
            }
            else if (message.IsFirstWord("wobble"))
            {
                Wobbler.Instance.Wob(Plugin.wobbles.ElementAt(UnityEngine.Random.Range(0, Plugin.wobbles.Count)).name);
            }


            if (user.IsBroadcaster || user.IsModerator)
            {

                if (message.StartsWith("!wadmin wobble"))
                {
                    string toWobble = message.Substring("!wadmin wobble ".Length);
                    try
                    {
                        Wobbler.Instance.Wob(toWobble);
                    }
                    catch (NullReferenceException)
                    {
                        SendMsg("Requested wobble does not exist");
                    }
                }
                else if (message.ToLower().Equals("!clear"))
                {
                    Wobbler.Instance.Clear();
                }
                else if (message.ToLower().Equals("!skip"))
                {
                    Wobbler.Instance.Skip();
                }
                SendMsg(message.ParseWadmin());
            }
        }


        
        public static bool IsVip(IChatUser user)
        {
            foreach (IChatBadge badge in user.Badges)
            {
                if (badge.Id.Equals("TwitchGlobalBadge_vip1"))
                {
                    return true;
                }
            }
            return false;
        }
        public static async Task SendErrorMessage(string type, string error)
        {
            Plugin.Log.Critical(error);
        }
    }
}
