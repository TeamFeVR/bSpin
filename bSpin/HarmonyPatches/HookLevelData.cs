﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
/*
namespace bSpin.HarmonyPatches
{
    [HarmonyPatch]
    class HookLeveldata
    {
        public static IDifficultyBeatmap difficultyBeatmap;
        public static bool is360Level = false;
        public static bool isModdedMap = false;
        public static bool isWallMap = false;

        //static SpawnRotationProcessor spawnRotationProcessor = new SpawnRotationProcessor();

        static void Prefix(IDifficultyBeatmap difficultyBeatmap)
        {
#if DEBUG
            Plugin.Log.Info("Got level data!");
#endif
            HookLeveldata.difficultyBeatmap = difficultyBeatmap;

            is360Level = difficultyBeatmap?.beatmapData?.beatmapEventsData.Any(
                x => x.type.IsRotationEvent() && spawnRotationProcessor.RotationForEventValue(x.value) != 0f
            ) == true;
            isModdedMap = Tools.IsModdedMap(difficultyBeatmap);

            sharedValues.noodle = isModdedMap;
        }

        internal static void Reset()
        {
            is360Level = isModdedMap = isWallMap = false;
            difficultyBeatmap = null;
        }

        [HarmonyTargetMethods]
        static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var t in new Type[] { typeof(StandardLevelScenesTransitionSetupDataSO), typeof(MissionLevelScenesTransitionSetupDataSO), typeof(MultiplayerLevelScenesTransitionSetupDataSO) })
                yield return t.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
        }
    }
}
*/