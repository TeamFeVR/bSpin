using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bSpin.CustomTypes;

namespace bSpin.UI.Spin_Editor
{
    class SpinPanel : BSMLResourceViewController
    {
        public override string ResourceName => "bSpin.UI.SpinEditor.SpinPanel.bsml";
        private static SpinProfile spins = new SpinProfile();

        internal static void LoadInList(SpinProfile profile)
        {

        }
    }
}
