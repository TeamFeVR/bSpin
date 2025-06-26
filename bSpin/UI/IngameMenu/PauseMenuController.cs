using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using bSpin.Configuration;
using bSpin.HarmonyPatches;

namespace bSpin.UI.IngameMenu {
    internal class PauseMenuController : BSMLResourceViewController {
        public override string ResourceName => "bSpin.UI.IngameMenu.PauseMenu.bsml";

        [UIValue("spin-en")]
        internal bool spinEn {
            get => PluginConfig.Instance.Enabled;
            set {
                PluginConfig.Instance.Enabled = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("spin-speed")]
        internal float spinSpeed {
            get => sharedValues.speed;
            set {
                sharedValues.speed = value;
                AngleChanger.instance.spinSpeed = value;
                NotifyPropertyChanged();
            }
        }
    }
}