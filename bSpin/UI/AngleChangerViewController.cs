﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMUI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine.UI;
using BeatSaberMarkupLanguage.Attributes;
using BS_Utils.Utilities;

namespace bSpin.UI
{
    class AngleChanger : NotifiableSingleton<AngleChanger>
    {
        [UIValue("enablespin")]
        public bool enableSpin
        {
            get => Configuration.PluginConfig.Instance.Enabled;
            set => Configuration.PluginConfig.Instance.Enabled = value;
        }

        [UIValue("noodlecompat")]
        public bool noodleCompat
        {
            get => Configuration.PluginConfig.Instance.NoodleCompat;
            set => Configuration.PluginConfig.Instance.NoodleCompat = value;
        }
        [UIValue("speed-value")]
        public float spinSpeed
        {
            get => HarmonyPatches.sharedValues.speed;
            set
            {
                HarmonyPatches.sharedValues.speed = value;
                NotifyPropertyChanged();
            }
        }
        [UIValue("livcompat")]
        public bool livCompat
        {
            get => Configuration.PluginConfig.Instance.AccountForLiv;
            set => Configuration.PluginConfig.Instance.AccountForLiv = value;
        }
        [UIValue("pauseui")]
        public bool pauseUI
        {
            get => Configuration.PluginConfig.Instance.PauseMenu;
            set => Configuration.PluginConfig.Instance.PauseMenu = value;
        }
        [UIValue("jank")]
        public bool jank
        {
            get => Configuration.PluginConfig.Instance.Experiments;
            set => Configuration.PluginConfig.Instance.Experiments = value;
        }
        [UIAction("speed-increase")]
        public void speedIncrease()
        {
            spinSpeed += 0.1f;
            NotifyPropertyChanged("spinSpeed");
        }
        [UIAction("speed-decrease")]
        public void speedDecrease()
        {
            spinSpeed -= 0.1f;
            NotifyPropertyChanged("spinSpeed");
        }

        [UIComponent("SpinList")] public CustomListTableData spinListData = new CustomListTableData();
        [UIAction("profileSelect")]
        void profileSelected(TableView _, int row)
        {
            Configuration.PluginConfig.Instance.spinProfile = row;
        }
        [UIAction("refresh")]
        void Refresh()
        {
            Plugin.spinProfiles = FileManager.GetSpinProfiles();
            setupLists();
        }
        [UIAction("open-editor")]
        void editor() => Spin_Editor.SpinEditorFlowCoordinator.Show();
        [UIAction("#post-parse")]
        void setupLists()
        {
            spinSpeed = HarmonyPatches.sharedValues.speed;
            spinListData.data.Clear();
            foreach(var profile in Plugin.spinProfiles)
            {
                var tempCell = new CustomListTableData.CustomCellInfo(profile.name);

                spinListData.data.Add(tempCell);
            }
            spinListData.tableView.ReloadData();
            spinListData.tableView.SelectCellWithIdx(Configuration.PluginConfig.Instance.spinProfile);
        }
        
        public void AddTab()
        {
            GameplaySetup.instance.AddTab("bSpin", "bSpin.UI.AngleChanger.bsml", this);
        }
        public void RemoveTab()
        {
            GameplaySetup.instance.RemoveTab("bSpin");
        }
    }
}
