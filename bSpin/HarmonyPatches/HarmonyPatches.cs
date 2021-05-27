using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using HarmonyLib;
using System.Collections;
using System.Reflection;
using IPA.Utilities;
using bSpin.Extentions;

namespace bSpin.HarmonyPatches
{

	[HarmonyPatch("Awake")]
	[HarmonyPatch(typeof(AudioTimeSyncController), nameof(AudioTimeSyncController.StartSong))]
	public class HookAudioTimeSyncController : MonoBehaviour
	{
		public static Transform _origin;

		static void Postfix(AudioTimeSyncController __instance)
		{
			PauseMenuPatch.MenuFound = false;
			sharedValues.spins = Plugin.spinProfiles.ElementAt(Configuration.PluginConfig.Instance.spinProfile).spins;

			if (Configuration.PluginConfig.Instance.AccountForLiv)
				sharedValues.offset = LivFinder.GetCameraAngleOffset(LivFinder.FindTracker());
			else
				sharedValues.offset = 0.0f;

			Plugin.Log.Debug("Spin angle offset is " + sharedValues.offset + "°");
			sharedValues.player = GameObject.Find("LocalPlayerGameCore");

            if (Configuration.PluginConfig.Instance.Experiments)
            {
				GameObject origin = GameObject.Find("LocalPlayerGameCore/Origin");
				GameObject spinHandle = new GameObject("bSpin_Handle");
				spinHandle.transform.SetParent(origin.transform);
				GameObject vrcore = origin.transform.GetChild(0).gameObject;
				vrcore.transform.SetParent(spinHandle.transform);
				sharedValues.player = spinHandle;

				//basically what this does is insert itself in between Origin (the thing i believe noodle spins)
				//and the player (usually called VRGameCore) which is the first child of Origin
				//i did it this way because compatibility for the win!
            }


			if (sharedValues.noodle && Configuration.PluginConfig.Instance.NoodleCompat)
            {
				sharedValues.player = sharedValues.player.transform.GetChild(0).gameObject;
			}

			Plugin.Log.Notice("There are currently " + sharedValues.spins.Count.ToString() + " Spins");
			sharedValues.player.transform.localEulerAngles = new Vector3(0, 0.0f, 0);
			Plugin.Log.Notice("I reset the rotation");
			if(Configuration.PluginConfig.Instance.Enabled)
				bSpinController.Instance.StartCoroutine(spin());
		}
		static IEnumerator spin()
		{
			while (true)
			{
				foreach (CustomTypes.Spin speen in sharedValues.spins)
				{
					if (!sharedValues.noodle)
					{
						if(Configuration.PluginConfig.Instance.Experiments)
							yield return sharedValues.player.transform.ExperimentalSpin(speen, sharedValues.speed, sharedValues.offset);
						else
							yield return sharedValues.player.transform.Spin(speen, sharedValues.speed, sharedValues.offset);
					}
					else if (sharedValues.noodle)
					{
						if (Configuration.PluginConfig.Instance.Experiments)
							yield return sharedValues.player.transform.ExperimentalNoodleSpin(speen, sharedValues.speed, sharedValues.offset);
						else
							yield return sharedValues.player.transform.NoodleSpin(speen, sharedValues.speed, sharedValues.offset);
					}
				}
			}
		}
	}

	[HarmonyPatch]
	class HookAudioTimeSyncController2
	{
		static void Postfix()
		{
			try
            {
				bSpinController.Instance.StopAllCoroutines();
			}catch(Exception e) { Plugin.Log.Critical(e.ToString()); }
			sharedValues.player.transform.eulerAngles = new Vector3(0, 0, 0);
		}
		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> TargetMethods()
		{
			yield return AccessTools.Method(typeof(AudioTimeSyncController), nameof(AudioTimeSyncController.Pause));
		}
	}

	public class sharedValues
    {
		public static List<CustomTypes.Spin> spins;
		public static float speed;
		public static bool noodle;
		public static float offset;
		public static GameObject player;

	}

	[HarmonyPatch]
	class HookAudioTimeSyncController3
	{
		static void Postfix()
		{
			if (Configuration.PluginConfig.Instance.Enabled)
				bSpinController.Instance.StartCoroutine(spin());
		}

		static IEnumerator spin()
		{
			while (true)
			{
				foreach (CustomTypes.Spin speen in sharedValues.spins)
				{
					if (!sharedValues.noodle)
					{
						if (Configuration.PluginConfig.Instance.Experiments)
							yield return sharedValues.player.transform.ExperimentalSpin(speen, sharedValues.speed, sharedValues.offset);
						else
							yield return sharedValues.player.transform.Spin(speen, sharedValues.speed, sharedValues.offset);
					}
					else if (sharedValues.noodle)
					{
						if (Configuration.PluginConfig.Instance.Experiments)
							yield return sharedValues.player.transform.ExperimentalNoodleSpin(speen, sharedValues.speed, sharedValues.offset);
						else
							yield return sharedValues.player.transform.NoodleSpin(speen, sharedValues.speed, sharedValues.offset);
					}
				}
			}
		}
		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> TargetMethods()
		{
			yield return AccessTools.Method(typeof(AudioTimeSyncController), nameof(AudioTimeSyncController.Resume));
		}
	}
}
