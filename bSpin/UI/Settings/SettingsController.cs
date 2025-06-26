using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Util;
using bSpin.Configuration;

namespace bSpin.UI.Settings {
    internal class SettingsController : PersistentSingleton<SettingsController> {
        [UIValue("twitch-en")]
        internal bool TwitchEnabled {
            get => PluginConfig.Instance.TwitchEnabled;
            set => PluginConfig.Instance.TwitchEnabled = value;
        }

        [UIValue("twitch-ann")]
        internal bool TwitchAnnounce {
            get => PluginConfig.Instance.TwitchAnnounce;
            set => PluginConfig.Instance.TwitchAnnounce = value;
        }

        [UIValue("udp-port")]
        internal string UdpPort {
            get => PluginConfig.Instance.UdpPort.ToString();
            set => PluginConfig.Instance.UdpPort = int.Parse(value);
        }

        [UIValue("udp-en")]
        internal bool UdpEnabled {
            get => PluginConfig.Instance.UdpEnabled;
            set => PluginConfig.Instance.UdpEnabled = value;
        }

        [UIAction("#apply")]
        public void OnApply() {
            switch (PluginConfig.Instance.TwitchEnabled) {
                /*
                case true:
                    if (Twitch.CommandHandler.twitchService == null)
                        Twitch.CommandHandler.Start();
                    break;

                case false:
                    if (Twitch.CommandHandler.twitchService != null)
                        Twitch.CommandHandler.Stop();
                    break;
                    */
            }
        }
    }
}