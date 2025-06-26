using System.Reflection;
using bSpin.Configuration;
using bSpin.UI.IngameMenu;
using HarmonyLib;
using UnityEngine;

namespace bSpin.HarmonyPatches {
    [HarmonyPatch(typeof(PauseMenuManager), nameof(PauseMenuManager.ShowMenu))]
    internal static class PauseMenuPatch {
        internal static bool MenuFound = false;
        private static Transform PauseMenu;

        private static void Postfix(PauseMenuManager __instance) {
            if (PluginConfig.Instance.PauseMenu)
                if (!MenuFound) {
                    var type = __instance.GetType();
                    var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                    var menuTransform = type.GetField("_pauseContainerTransform", bindingFlags);
                    PauseMenu = (Transform)menuTransform.GetValue(__instance);
                    FloatingScreenCreator.Yeet(PauseMenu);
                }
        }
    }
}