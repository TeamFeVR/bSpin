using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.FloatingScreen;
using UnityEngine;
using static HMUI.ViewController;

namespace bSpin.UI.IngameMenu {
    internal class FloatingScreenCreator {
        internal static bool didThing;
        internal static FloatingScreen pauseFloatingScreen;

        internal static void Create(bool up) {
            var y = up ? 2.2f : 2.0f;

            pauseFloatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(100, 20), false, new Vector3(0, y, 2),
                Quaternion.Euler(0, 0, 0), 175f);
        }

        internal static void Yeet(Transform menu = null) {
            if (true) {
                var _pm = BeatSaberUI.CreateViewController<PauseMenuController>();
                pauseFloatingScreen.SetRootViewController(_pm, AnimationType.None);
                pauseFloatingScreen.gameObject.name = "bSpin_pause_toggle";
                pauseFloatingScreen.gameObject.transform.SetParent(menu.GetChild(1));
                didThing = true;
            }
        }
    }
}