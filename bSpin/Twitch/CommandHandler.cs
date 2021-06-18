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
        internal static ChatCoreInstance coreInstance;

        internal static void SendMsg(string msg)
        {
            try
            {
                if (Configuration.PluginConfig.Instance.TwitchEnabled)
                    twitchService.SendTextMessage(msg, (twitchService).Channels.ElementAt(0).Value.Name.ToString());
                else if (!Configuration.PluginConfig.Instance.TwitchEnabled)
                {
                    Console.WriteLine(msg);
                }
            }
            catch (Exception)
            {
                Plugin.Log.Debug($"Failed to send twitch message, but if it succeeded, it would be \"{msg}\"");
            }
        }

        internal static void Init()
        {
            coreInstance = ChatCoreInstance.Create();
            UDP.NetworkHandler.Init();
        }

        internal static void Start()
        {
            Plugin.Log.Notice("Starting up Twitch");
            
            twitchService = coreInstance.RunTwitchServices();
            twitchService.OnJoinChannel += login;
            twitchService.OnTextMessageReceived += StreamServiceProvider_OnMessageReceived;
        }
        internal static void Stop()
        {
            Plugin.Log.Notice("Shutting down Twitch");
            coreInstance.StopTwitchServices();
            twitchService.OnJoinChannel -= login;
            twitchService.OnTextMessageReceived -= StreamServiceProvider_OnMessageReceived;
            twitchService = null;
            
        }

        static void login(IChatService svc, IChatChannel channel)
        {
            Plugin.Log.Notice($"ChatCore currently has {twitchService.Channels.Count().ToString()} Channels joined");
            if(Configuration.PluginConfig.Instance.TwitchAnnounce)
                SendMsg($"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} connected");
        }

        public static void StreamServiceProvider_OnMessageReceived(IChatService svc, IChatMessage msg)
        {
            //Console.WriteLine($"{msg.Sender.DisplayName}: {msg.Message}");
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
                SendMsg($"{loadedWobs} Wobble{(Plugin.wobbles.Count != 1 ? "s" : "")} loaded!");
            }
            if (message.ToLower().Equals("!wobbles"))
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

                if (Configuration.PluginConfig.Instance.WobbleEnabled)
                    Wobbler.Instance.Wob(Plugin.wobbles.ElementAt(UnityEngine.Random.Range(0, Plugin.wobbles.Count)).name);
                else if (!Configuration.PluginConfig.Instance.WobbleEnabled)
                    SendMsg($"{user.DisplayName}:  Wobble is currently disabled.");
            }


            if (user.IsBroadcaster || user.IsModerator)
            {
                if (Configuration.PluginConfig.Instance.WobbleEnabled)
                {
                    if (message.StartsWith("!wadmin wobble"))
                    {
                        if (Configuration.PluginConfig.Instance.WobbleEnabled)
                        {
                            string toWobble = message.Substring("!wadmin wobble ".Length);
                            try
                            {
                                Wobbler.Instance.Wob(toWobble);
                            }
                            catch (NullReferenceException)
                            {
                                SendMsg($"{user.DisplayName}:  Requested wobble does not exist");
                            }
                        }
                        else if (!Configuration.PluginConfig.Instance.WobbleEnabled)
                            SendMsg($"{user.DisplayName}:  Wobble is currently disabled.");

                    }
                    else if (message.ToLower().Equals("!clear"))
                    {
                        if (Configuration.PluginConfig.Instance.WobbleEnabled)
                            Wobbler.Instance.Clear();
                        else if (!Configuration.PluginConfig.Instance.WobbleEnabled)
                            SendMsg($"{user.DisplayName}:  Wobble is currently disabled.");
                    }
                    else if (message.ToLower().Equals("!skip"))
                    {
                        if (Configuration.PluginConfig.Instance.WobbleEnabled)
                            Wobbler.Instance.Skip();
                        else if (!Configuration.PluginConfig.Instance.WobbleEnabled)
                            SendMsg($"{user.DisplayName}:  Wobble is currently disabled.");
                    }
                    if (Configuration.PluginConfig.Instance.WobbleEnabled)
                        SendMsg(message.ParseWadmin());
                    else if (!Configuration.PluginConfig.Instance.WobbleEnabled)
                        SendMsg($"{user.DisplayName}:  Wobble is currently disabled.");
                }
                
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
