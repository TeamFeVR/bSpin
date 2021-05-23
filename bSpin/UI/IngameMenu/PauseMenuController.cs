using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace bSpin.UI.IngameMenu
{
    class PauseMenuController : BSMLResourceViewController
    {
        public override string ResourceName => "bSpin.UI.IngameMenu.PauseMenu.bsml";

        [UIValue("spin-en")]
        internal bool spinEn
        {
            get => Configuration.PluginConfig.Instance.Enabled;
            set
            {
                Configuration.PluginConfig.Instance.Enabled = value;
                NotifyPropertyChanged();
            }
        }
    }
}
