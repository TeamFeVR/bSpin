using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bSpin.UI.Spin_Editor
{
    class RotationPanel : BSMLResourceViewController
    {
        public override string ResourceName => "bSpin.UI.SpinEditor.RotationPanel.bsml";

        [UIValue("start-delay")] static string StartDelay = "";
        [UIValue("s-rot-x")] static string SRotX = "";
        [UIValue("s-rot-y")] static string SRotY = "";
        [UIValue("s-rot-z")] static string SRotZ = "";
        [UIValue("e-rot-x")] static string ERotX = "";
        [UIValue("e-rot-y")] static string ERotY = "";
        [UIValue("e-rot-z")] static string ERotZ = "";
        [UIValue("end-delay")] static string EndDelay = "";
        [UIValue("spin-length")] static string SpinLength = "";

        internal static void Load(CustomTypes.Spin spin)
        {
            StartDelay = spin.DelayBeforeSpin.ToString();
            SRotX = spin.Begin.x.ToString();
            SRotY = spin.Begin.y.ToString();
            SRotZ = spin.Begin.z.ToString();
            ERotX = spin.End.x.ToString();
            ERotY = spin.End.y.ToString();
            ERotZ = spin.End.z.ToString();
            EndDelay = spin.DelayAfterSpin.ToString();
            SpinLength = spin.Length.ToString();
        }


    }
}
