using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;
using System.Reflection;
using bSpin.CustomTypes;
using IPA.Utilities;
using System.IO;

namespace bSpin
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static Harmony harmony { get; private set; }
        internal static GameObject bSpinController { get; private set; }
        public static List<SpinProfile> spinProfiles { get; set; }

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            harmony = new Harmony("headassbtw.bSpin");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            UI.AngleChanger.instance.AddTab();
            Log.Info("bSpin initialized.");

            

            if (!Directory.Exists(Path.Combine(UnityGame.UserDataPath, "bSpin")))
            {
                Directory.CreateDirectory(Path.Combine(UnityGame.UserDataPath, "bSpin"));
            }
            if (Directory.GetFiles(Path.Combine(UnityGame.UserDataPath, "bSpin") + "\\").Length <= 0)
            {
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("bSpin.spin.json"))
                {
                    using (var file = new FileStream(Path.Combine(UnityGame.UserDataPath, "bSpin", "demoSpin.json"), FileMode.Create, FileAccess.Write))
                    {
                        resource.CopyTo(file);
                        file.Close();
                    }
                }

            }

            spinProfiles = FileManager.GetSpinProfiles();
            HarmonyPatches.sharedValues.spins = spinProfiles[0].spins;
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        
        [Init]
        public void InitWithConfig(Config conf)
        {
            
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            bSpinController = new GameObject("bSpinController");
            bSpinController.AddComponent<bSpinController>();
            HarmonyPatches.sharedValues.speed = Configuration.PluginConfig.Instance.SpinSpeed;

        }
        
        [OnExit]
        public void OnApplicationQuit()
        {
            UI.AngleChanger.instance.RemoveTab();
            Configuration.PluginConfig.Instance.SpinSpeed = HarmonyPatches.sharedValues.speed;

        }
    }
}
