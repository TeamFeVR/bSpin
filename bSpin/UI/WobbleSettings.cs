﻿using System;
using System.Linq;
using HMUI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Util;

namespace bSpin.UI
{
    [HotReload(RelativePathToLayout ="AngleChanger.bsml")]
    class WobbleSettings : NotifiableSingleton<WobbleSettings>
    {
        private int SelectedEditWobble = 0;
        [UIValue("enablewobble")]
        public bool enableWobble
        {
            get => HarmonyPatches.sharedValues.wobble;
            set => HarmonyPatches.sharedValues.wobble = value;
        }
        [UIComponent("SpinList")] public CustomListTableData spinListData = new CustomListTableData();
        [UIAction("profileSelect")]
        void profileSelected(TableView _, int row)
        {
            SelectedEditWobble = row;
        }
        [UIAction("refresh")]
        internal void Refresh()
        {
            Plugin.wobbles = FileManager.GetWobbleProfiles();
            setupLists();
        }
        [UIAction("open-editor")]
        private void Show()
        {
            
            FlowCoordinator flowCoordinator = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
            if (!Wobble_Editor.SpinEditorFlowCoordinator.Instance)
            {
                Wobble_Editor.SpinEditorFlowCoordinator.Instance = BeatSaberUI.CreateFlowCoordinator<Wobble_Editor.SpinEditorFlowCoordinator>();
            }
            if (!Wobble_Editor.SpinEditorFlowCoordinator.Instance)
                Plugin.Log.Critical("uh");
            Wobble_Editor.SpinEditorFlowCoordinator.SetPreviousFlowCoordinator(flowCoordinator);
            try
            {
                flowCoordinator.PresentFlowCoordinator(Wobble_Editor.SpinEditorFlowCoordinator.Instance);
                Wobble_Editor.SpinPanel.Instance.LoadInList(Plugin.wobbles.ElementAt(SelectedEditWobble));
            }
            catch (Exception e)
            {
                Plugin.Log.Critical(e.ToString());
            }


        }
        [UIAction("#post-parse")]
        void setupLists()
        {
            spinListData.Data.Clear();
            foreach(var profile in Plugin.wobbles)
            {
                var tempCell = new CustomListTableData.CustomCellInfo(profile.Name);

                spinListData.Data.Add(tempCell);
            }
            spinListData.TableView.ReloadData();
            spinListData.TableView.SelectCellWithIdx(Configuration.PluginConfig.Instance.spinProfile);
        }
        
        public void AddTab()
        {
            //GameplaySetup.instance.AddTab("Wobbles", "bSpin.UI.WobbleSettings.bsml", this);
        }
        public void RemoveTab()
        {
            //GameplaySetup.instance.RemoveTab("Wobbles");
        }
    }
}
