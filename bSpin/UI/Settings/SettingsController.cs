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
            if (Configuration.PluginConfig.Instance.UdpEnabled)
            {
                Twitch.CommandHandler.UDPListenerThread.Abort();
                Twitch.CommandHandler.UDPListenerThread.Start();
            }
        }
    }
}
