using BeatSaberMarkupLanguage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bSpin.UI.Settings
{
    class SettingsController : PersistentSingleton<SettingsController>
    {
        [UIValue("twitch-en")]
        internal bool TwitchEnabled
        {
            get => Configuration.PluginConfig.Instance.TwitchEnabled;
            set
            {
                Configuration.PluginConfig.Instance.TwitchEnabled = value;
            }
        }

        [UIValue("twitch-ann")]
        internal bool TwitchAnnounce
        {
            get => Configuration.PluginConfig.Instance.TwitchAnnounce;
            set
            {
                Configuration.PluginConfig.Instance.TwitchAnnounce = value;
            }
        }

        [UIValue("udp-port")]
        internal string UdpPort
        {
            get => Configuration.PluginConfig.Instance.UdpPort.ToString();
            set
            {
                Configuration.PluginConfig.Instance.UdpPort = Int32.Parse(value);
            }
        }

        [UIValue("udp-en")]
        internal bool UdpEnabled
        {
            get => Configuration.PluginConfig.Instance.UdpEnabled;
            set
            {
                Configuration.PluginConfig.Instance.UdpEnabled = value;
            }
        }

        [UIAction("#apply")]
        public void OnApply()
        {
            switch (Configuration.PluginConfig.Instance.UdpEnabled)
            {
                case true:
                    UDP.NetworkHandler.Listening = true;
                    break;
                case false:
                    UDP.NetworkHandler.Listening = false;
                    break;
            }
            switch (Configuration.PluginConfig.Instance.TwitchEnabled)
            {
                case true:
                    if (Twitch.CommandHandler.twitchService == null)
                        Twitch.CommandHandler.Start();
                    break;

                case false:
                    if (Twitch.CommandHandler.twitchService != null)
                        Twitch.CommandHandler.Stop();
                    break;
            }
        }
    }
}
