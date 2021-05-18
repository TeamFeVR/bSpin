using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.FloatingScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static HMUI.ViewController;

namespace bSpin.UI.IngameMenu
{
    class FloatingScreenCreator
    {
        internal static FloatingScreen pauseFloatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(100, 10), false, new Vector3(0, 3.25f, 3.75f), Quaternion.Euler(0, 0, 0), 175f, true);
        internal static void yeet()
        {
            var _pm = BeatSaberUI.CreateViewController<PauseMenuController>();
            pauseFloatingScreen.SetRootViewController(_pm, animationType: AnimationType.None);
        }
    }
}
