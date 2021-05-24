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
        private static bool didThing = false;
        internal static FloatingScreen pauseFloatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(100, 20), false, new Vector3(0, 2, 2), Quaternion.Euler(0, 0, 0), 175f, false);
        internal static void Yeet(Transform menu = null)
        {
            if (!didThing)
            {
                var _pm = BeatSaberUI.CreateViewController<PauseMenuController>();
                pauseFloatingScreen.SetRootViewController(_pm, animationType: AnimationType.None);
                pauseFloatingScreen.gameObject.name = "bSpin_pause_toggle";
                pauseFloatingScreen.gameObject.transform.SetParent(menu.GetChild(1));
                didThing = true;
            }
        }
    }
}
