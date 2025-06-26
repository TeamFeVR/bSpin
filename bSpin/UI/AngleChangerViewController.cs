using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMUI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Util;

namespace bSpin.UI
{
    [HotReload(RelativePathToLayout ="./AngleChanger.bsml")]
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

        [UIComponent("SpinList")] public CustomListTableData spinListData = new CustomListTableData();
        [UIAction("profileSelect")]
        void profileSelected(TableView _, int row)
        {
            Configuration.PluginConfig.Instance.spinProfile = row;
        }
        [UIAction("refresh")]
        internal void Refresh()
        {
            Plugin.spinProfiles = FileManager.GetSpinProfiles();
            setupLists();
        }
        [UIAction("open-editor")]
        private void Show()
        {
            
            FlowCoordinator flowCoordinator = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
            if (!Spin_Editor.SpinEditorFlowCoordinator.Instance)
                Spin_Editor.SpinEditorFlowCoordinator.Instance = BeatSaberUI.CreateFlowCoordinator<Spin_Editor.SpinEditorFlowCoordinator>();
            if (!Spin_Editor.SpinEditorFlowCoordinator.Instance)
                Plugin.Log.Critical("uh");
            Spin_Editor.SpinEditorFlowCoordinator.SetPreviousFlowCoordinator(flowCoordinator);
            try
            {
                flowCoordinator.PresentFlowCoordinator(Spin_Editor.SpinEditorFlowCoordinator.Instance);
                Spin_Editor.SpinPanel.Instance.LoadInList(Plugin.spinProfiles.ElementAt(Configuration.PluginConfig.Instance.spinProfile));
            }
            catch (Exception e)
            {
                Plugin.Log.Critical(e.ToString());
            }


        }
        [UIAction("#post-parse")]
        void setupLists()
        {
            spinSpeed = HarmonyPatches.sharedValues.speed;
            spinListData.Data.Clear();
            foreach(var profile in Plugin.spinProfiles)
            {
                var tempCell = new CustomListTableData.CustomCellInfo(profile.name);

                spinListData.Data.Add(tempCell);
            }
            spinListData.TableView.ReloadData();
            spinListData.TableView.SelectCellWithIdx(Configuration.PluginConfig.Instance.spinProfile);
        }
        
        public void AddTab()
        {
            GameplaySetup.Instance.AddTab("bSpin", "bSpin.UI.AngleChanger.bsml", this);
        }
        public void RemoveTab()
        {
            GameplaySetup.Instance.RemoveTab("bSpin");
        }
    }
}
