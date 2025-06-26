﻿using IPA;
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
using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.Util;
using bSpin.Network;

namespace bSpin
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static Harmony harmony { get; private set; }
        internal static GameObject bSpinController { get; private set; }
        public static List<SpinProfile> spinProfiles { get; internal set; }
        public static List<WobbleProfile> wobbles { get; internal set; }

        [Init]
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            harmony = new Harmony("headassbtw.bSpin");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            
            MainMenuAwaiter.MainMenuInitializing += delegate {
                //BSMLSettings.Instance.AddSettingsMenu("bSpin", "bSpin.UI.Settings.settings.bsml", UI.Settings.SettingsController.instance);
                UI.AngleChanger.instance.AddTab();
                UI.WobbleSettings.instance.AddTab();
            };
            
            Log.Info("bSpin initialized.");

            

            if (!Directory.Exists(Path.Combine(UnityGame.UserDataPath, "bSpin")))
            {
                Directory.CreateDirectory(Path.Combine(UnityGame.UserDataPath, "bSpin"));
            }
            if (!Directory.Exists(Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles")))
            {
                Directory.CreateDirectory(Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles"));
            }
            if (Directory.GetFiles(Path.Combine(UnityGame.UserDataPath, "bSpin") + "\\").Length <= 0)
            {
                var demoSpins = new List<string>
                {
                    "bSpin.spin.json",
                    "bSpin.spin_45.json",
                    "bSpin.spin_90.json",
                    "bSpin.spin_180.json",
                    "bSpin.spin_360.json"
                };

                demoSpins.ForEach(x =>
                {
                    using var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(x);
                    using var fileStream = new FileStream(Path.Combine(UnityGame.UserDataPath, "bSpin", x.Replace("bSpin.", "")), FileMode.Create, FileAccess.Write);
                    manifestResourceStream?.CopyTo(fileStream);
                    fileStream.Close();
                });
            }
            if (Directory.GetFiles(Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles") + "\\").Length <= 0)
            {
                var wb = new List<Wobble>
                {
                    new Wobble(0.25f, new Vector3(0, 0, 0), new Vector3(180, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 3, 0)),
                    new Wobble(0.25f, new Vector3(180, 0, 0), new Vector3(360, 0, 0), new Vector3(0, 3, 0), new Vector3(0, 0, 0))
                };
                FileManager.SaveWobbleProfile(new WobbleProfile("flip", wb));
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("bSpin.zoom.json"))
                {
                    using (var file = new FileStream(Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles", "zoom.json"), FileMode.Create, FileAccess.Write))
                    {
                        resource.CopyTo(file);
                        file.Close();
                    }
                }

            }
            wobbles = FileManager.GetWobbleProfiles();
            spinProfiles = FileManager.GetSpinProfiles();
            HarmonyPatches.sharedValues.spins = spinProfiles[0].spins;

            var net = new NetworkHandler();


        }

        #region BSIPA Config
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
            /*Twitch.CommandHandler.Init();
            bSpinController.AddComponent<Twitch.Wobbler>();*/
            HarmonyPatches.sharedValues.speed = Configuration.PluginConfig.Instance.SpinSpeed;
            if(!Configuration.PluginConfig.Instance.TwitchEnabled)
                return;
            //Twitch.CommandHandler.Start();
        }
        
        [OnExit]
        public void OnApplicationQuit()
        {
            UI.AngleChanger.instance.RemoveTab();
            UI.WobbleSettings.instance.RemoveTab();
            Configuration.PluginConfig.Instance.SpinSpeed = HarmonyPatches.sharedValues.speed;

        }
    }
}
