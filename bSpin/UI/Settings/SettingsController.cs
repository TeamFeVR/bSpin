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

        [UIAction("#apply")]
        public void OnApply()
        {
            UDP.NetworkHandler.listener.Close();
            UDP.NetworkHandler.StartListener();
        }
    }
}
