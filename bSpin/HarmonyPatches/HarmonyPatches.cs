using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using bSpin.Configuration;
using bSpin.CustomTypes;
using bSpin.Extentions;
using bSpin.UI.IngameMenu;
using HarmonyLib;
using IPA.Loader;
using UnityEngine;

namespace bSpin.HarmonyPatches {
    [HarmonyPatch("Awake")]
    [HarmonyPatch(typeof(AudioTimeSyncController), nameof(AudioTimeSyncController.StartSong))]
    public class HookAudioTimeSyncController : MonoBehaviour {
        public static Transform _origin;

        private static void Postfix(AudioTimeSyncController __instance) {
            FloatingScreenCreator.Create(sharedValues.ts);
            FloatingScreenCreator.didThing = false;
            PauseMenuPatch.MenuFound = false;
            sharedValues.spins = Plugin.spinProfiles.ElementAt(PluginConfig.Instance.spinProfile).spins;

            /*if (Configuration.PluginConfig.Instance.AccountForLiv)
                sharedValues.offset = LivFinder.GetCameraAngleOffset(LivFinder.FindTracker());
            else*/
            //LIV SDK update broke this, not worth fixing
            sharedValues.offset = 0.0f;

            Plugin.Log.Debug("Spin angle offset is " + sharedValues.offset + "°");
            sharedValues.player = GameObject.Find("LocalPlayerGameCore");

            if (PluginConfig.Instance.Experiments) {
                var origin = GameObject.Find("LocalPlayerGameCore");
                var spinHandle = new GameObject("bSpin_Handle");
                spinHandle.transform.SetParent(origin.transform);
                var vrcore = origin.transform.GetChild(0).gameObject;
                vrcore.transform.SetParent(spinHandle.transform, true);
                sharedValues.player = spinHandle;

                //basically what this does is insert itself in between Origin (the thing i believe noodle spins)
                //and the player (usually called VRGameCore) which is the first child of Origin
                //i did it this way because compatibility for the win!
            }


            if (sharedValues.noodle && PluginConfig.Instance.NoodleCompat)
                sharedValues.player = sharedValues.player.transform.GetChild(0).gameObject;

            Plugin.Log.Debug("There are currently " + sharedValues.spins.Count + " Spins");
            sharedValues.player.transform.localEulerAngles = new Vector3(0, 0.0f, 0);

            if (sharedValues.wobble) {
                /*
                Twitch.Wobbler.Handle = new GameObject("bSpin_Handle_Wobble");
                Twitch.Wobbler.Instance = Twitch.Wobbler.Handle.AddComponent<Twitch.Wobbler>();
                Twitch.Wobbler.Instance.Innit();

                sharedValues.player.transform.SetParent(Twitch.Wobbler.Handle.transform);
                */
            }


            if (PluginConfig.Instance.Enabled)
                bSpinController.Instance.StartCoroutine(spin());
        }

        private static IEnumerator spin() {
            while (true)
                foreach (var speen in sharedValues.spins)
                    if (sharedValues.noodle && PluginConfig.Instance.Experiments)
                        yield return sharedValues.player.transform.ExperimentalNoodleSpin(speen, sharedValues.speed,
                            sharedValues.offset);
                    else if (sharedValues.noodle && !PluginConfig.Instance.Experiments)
                        yield return sharedValues.player.transform.NoodleSpin(speen, sharedValues.speed,
                            sharedValues.offset);
                    else if (!sharedValues.noodle && PluginConfig.Instance.Experiments)
                        yield return sharedValues.player.transform.ExperimentalSpin(speen, sharedValues.speed,
                            sharedValues.offset);
                    else
                        yield return sharedValues.player.transform.Spin(speen, sharedValues.speed, sharedValues.offset);
        }
    }

    [HarmonyPatch]
    internal class HookAudioTimeSyncController2 {
        private static void Postfix() {
            try {
                bSpinController.Instance.StopAllCoroutines();
            }
            catch (Exception e) {
                Plugin.Log.Critical(e.ToString());
            }

            sharedValues.player.transform.eulerAngles = new Vector3(0, 0, 0);
            /*if(Twitch.Wobbler.Instance != null)
                Twitch.Wobbler.Instance.Stop();*/
        }

        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> TargetMethods() {
            yield return AccessTools.Method(typeof(AudioTimeSyncController), nameof(AudioTimeSyncController.Pause));
        }
    }

    public class sharedValues {
        public static List<Spin> spins;
        public static float speed;
        public static bool noodle;
        public static float offset;
        public static GameObject player;

        public static bool wobble {
            get => PluginConfig.Instance.WobbleEnabled;
            set => PluginConfig.Instance.WobbleEnabled = value;
        }

        public static bool ts => PluginManager.GetPlugin("TrickSaber") != null;
    }

    [HarmonyPatch]
    internal class HookAudioTimeSyncController3 {
        private static void Postfix() {
            if (PluginConfig.Instance.Enabled)
                bSpinController.Instance.StartCoroutine(spin());
            /*if (Twitch.Wobbler.Instance != null)
                Twitch.Wobbler.Instance.Waitaminute();*/
        }

        private static IEnumerator spin() {
            while (true)
                foreach (var speen in sharedValues.spins)
                    if (sharedValues.noodle && PluginConfig.Instance.Experiments)
                        yield return sharedValues.player.transform.ExperimentalNoodleSpin(speen, sharedValues.speed,
                            sharedValues.offset);
                    else if (sharedValues.noodle && !PluginConfig.Instance.Experiments)
                        yield return sharedValues.player.transform.NoodleSpin(speen, sharedValues.speed,
                            sharedValues.offset);
                    else if (!sharedValues.noodle && PluginConfig.Instance.Experiments)
                        yield return sharedValues.player.transform.ExperimentalSpin(speen, sharedValues.speed,
                            sharedValues.offset);
                    else
                        yield return sharedValues.player.transform.Spin(speen, sharedValues.speed, sharedValues.offset);
        }

        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> TargetMethods() {
            yield return AccessTools.Method(typeof(AudioTimeSyncController), nameof(AudioTimeSyncController.Resume));
        }
    }
}