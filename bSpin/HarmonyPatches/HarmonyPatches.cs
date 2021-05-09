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


			sharedValues.spins = Plugin.spinProfiles.ElementAt(Configuration.PluginConfig.Instance.spinProfile).spins;

#if DEBUG


			Plugin.Log.Info("AudioTimeSyncController.StartSong()");
	#endif
			sharedValues.player = GameObject.Find("LocalPlayerGameCore");

			if (sharedValues.noodle && Configuration.PluginConfig.Instance.NoodleCompat)
            {
				var noodlePlayerTrack = sharedValues.player.transform.GetChild(0).gameObject;
				//sharedValues.player.AddComponent(Type.GetType("NoodleExtensions.Animation.PlayerTrack"));
				sharedValues.player = noodlePlayerTrack;
			}




			Plugin.Log.Notice("There are currently " + sharedValues.spins.Count.ToString() + " Spins");
			//Plugin.Log.Info("Found" + sharedValues.player.ToString());

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
						sharedValues.player.transform.Spin(speen, sharedValues.speed);
					}
					else if (sharedValues.noodle && Configuration.PluginConfig.Instance.NoodleCompat)
					{
						sharedValues.player.transform.NoodleSpin(speen, sharedValues.speed);
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
			bSpinController.Instance.StopAllCoroutines();
			
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
						sharedValues.player.transform.Spin(speen, sharedValues.speed);
					}
					else if (sharedValues.noodle && Configuration.PluginConfig.Instance.NoodleCompat)
					{
						sharedValues.player.transform.NoodleSpin(speen, sharedValues.speed);
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
