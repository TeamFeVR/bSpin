using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace bSpin.HarmonyPatches
{
    [HarmonyPatch(typeof(PauseMenuManager), nameof(PauseMenuManager.ShowMenu))]
    internal static class PauseMenuPatch
    {
        internal static bool MenuFound = false;
        static Transform PauseMenu;

        static void Postfix(PauseMenuManager __instance)
        {
            if(Configuration.PluginConfig.Instance.PauseMenu)
                if (!MenuFound)
                {
                    Type type = __instance.GetType();
                    BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                    FieldInfo menuTransform = type.GetField("_pauseContainerTransform", bindingFlags);
                    PauseMenu = (Transform)menuTransform.GetValue(__instance);
                    UI.IngameMenu.FloatingScreenCreator.Yeet(PauseMenu);
                }
        }
    }
}
