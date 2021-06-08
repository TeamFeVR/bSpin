using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatCore;
using ChatCore.Interfaces;
using static bSpin.Twitch.Types;

namespace bSpin.Twitch
{
    class CommandHandler
    {
        public string Usernamequestionmark = "";
        public ChatCore.Services.Twitch.TwitchService twitchService;
        internal static CommandHandler Instance;

        internal static List<Command> Commands;
        
        internal void SendMsg(string msg)
        {
            Instance.twitchService.SendTextMessage(msg, (Instance.twitchService).Channels.ElementAt(0).Value.Name.ToString());
        }

        public void Start()
        {
            Plugin.Log.Notice("Starting up Twitch");
            var streamCore = ChatCoreInstance.Create();
            var streamingService = streamCore.RunAllServices();
            streamingService.OnLogin += StreamingService_OnLogin;
            streamingService.OnTextMessageReceived += StreamServiceProvider_OnMessageReceived;
            twitchService = streamingService.GetTwitchService();
            
        }
        public void StreamingService_OnLogin(IChatService svc)
        {
            Plugin.Log.Info($"First twitch channel is \"{((ChatCore.Services.Twitch.TwitchService)svc).Channels.ElementAt(0).Value.Name.ToString()}\"");

            if (svc is ChatCore.Services.Twitch.TwitchService twitchService)
            {
                Plugin.Log.Info("it's a twitch channel!");
                twitchService.JoinChannel(twitchService.Channels.ElementAt(0).Value.Name.ToString());
                twitchService.SendTextMessage("bSpin 1.3.0 alpha, unexpectedly-bonks-you edition, " + twitchService.DisplayName + twitchService.Channels.ElementAt(0).Value.Name.ToString(), twitchService.Channels.ElementAt(0).Value.Name.ToString());
            }
        }
        public void StreamServiceProvider_OnMessageReceived(IChatService svc, IChatMessage msg)
        {
            if (svc is ChatCore.Services.Twitch.TwitchService twitchService)
            {
                Console.WriteLine($"{msg.Sender.DisplayName}: {msg.Message}");



                if (msg.Message.ToLower().Contains("!debug"))
                {
                    SendMsg($"bSpin alpha 1.3.0");
                    string loadedProfs = Plugin.spinProfiles.Count.ToString();
                    SendMsg($"{loadedProfs} Spin profiles loaded!");
                    string loadedWobs = Plugin.wobbles.Count.ToString();
                    SendMsg($"{loadedWobs} Wobble{(Plugin.wobbles.Count > 1 ? "s" : "")} loaded!");
                }
                else if (msg.Message.ToLower().Equals("!wobbles"))
                {
                    string woblist = "Wobbles: ";
                    foreach (var wob in Plugin.wobbles)
                    {
                        woblist += (wob.name) + ", ";
                    }
                    SendMsg(woblist);
                }
                else if (msg.Message.IsFirstWord("wobble"))
                {
                    Wobbler.Instance.Wob(Plugin.wobbles.ElementAt(UnityEngine.Random.Range(0, Plugin.wobbles.Count)).name);
                }

                if (msg.Sender.IsBroadcaster || msg.Sender.IsModerator)
                {
                    
                    if (msg.Message.StartsWith("!wadmin wobble"))
                    {
                        string toWobble = msg.Message.Substring("!wadmin wobble ".Length);
                        Wobbler.Instance.Wob(toWobble);
                    }
                    else if (msg.Message.ToLower().Equals("!clear"))
                    {
                        Wobbler.Instance.Clear();
                    }
                    else if (msg.Message.ToLower().Equals("!skip"))
                    {
                        Wobbler.Instance.Skip();
                    }
                    SendMsg(msg.Message.ParseWadmin());
                }
            }
        }
        
        public bool IsVip(IChatUser user)
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
        public async Task SendErrorMessage(string type, string error)
        {
            Plugin.Log.Critical(error);
        }
    }
}
