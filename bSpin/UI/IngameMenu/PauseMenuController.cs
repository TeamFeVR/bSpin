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
        [UIValue("spin-speed")]
        internal float spinSpeed
        {
            get => HarmonyPatches.sharedValues.speed;
            set
            {
                HarmonyPatches.sharedValues.speed = value;
                AngleChanger.instance.spinSpeed = value;
                NotifyPropertyChanged();
            }
        }
    }
}
