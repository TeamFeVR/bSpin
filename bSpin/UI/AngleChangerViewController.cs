using System;
using System.Linq;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Util;
using bSpin.Configuration;
using bSpin.HarmonyPatches;
using bSpin.UI.Spin_Editor;
using HMUI;

namespace bSpin.UI {
    [HotReload(RelativePathToLayout = "./AngleChanger.bsml")]
    internal class AngleChanger : NotifiableSingleton<AngleChanger> {
        [UIComponent("SpinList")] public CustomListTableData spinListData = new CustomListTableData();

        [UIValue("enablespin")]
        public bool enableSpin {
            get => PluginConfig.Instance.Enabled;
            set => PluginConfig.Instance.Enabled = value;
        }

        [UIValue("noodlecompat")]
        public bool noodleCompat {
            get => PluginConfig.Instance.NoodleCompat;
            set => PluginConfig.Instance.NoodleCompat = value;
        }

        [UIValue("speed-value")]
        public float spinSpeed {
            get => sharedValues.speed;
            set {
                sharedValues.speed = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("livcompat")]
        public bool livCompat {
            get => PluginConfig.Instance.AccountForLiv;
            set => PluginConfig.Instance.AccountForLiv = value;
        }

        [UIValue("pauseui")]
        public bool pauseUI {
            get => PluginConfig.Instance.PauseMenu;
            set => PluginConfig.Instance.PauseMenu = value;
        }

        [UIValue("jank")]
        public bool jank {
            get => PluginConfig.Instance.Experiments;
            set => PluginConfig.Instance.Experiments = value;
        }

        [UIAction("profileSelect")]
        private void profileSelected(TableView _, int row) {
            PluginConfig.Instance.spinProfile = row;
        }

        [UIAction("refresh")]
        internal void Refresh() {
            Plugin.spinProfiles = FileManager.GetSpinProfiles();
            setupLists();
        }

        [UIAction("open-editor")]
        private void Show() {
            var flowCoordinator = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
            if (!SpinEditorFlowCoordinator.Instance)
                SpinEditorFlowCoordinator.Instance = BeatSaberUI.CreateFlowCoordinator<SpinEditorFlowCoordinator>();
            if (!SpinEditorFlowCoordinator.Instance)
                Plugin.Log.Critical("uh");

            SpinEditorFlowCoordinator.SetPreviousFlowCoordinator(flowCoordinator);
            try {
                flowCoordinator.PresentFlowCoordinator(SpinEditorFlowCoordinator.Instance);
                SpinPanel.Instance.LoadInList(Plugin.spinProfiles.ElementAt(PluginConfig.Instance.spinProfile));
            }
            catch (Exception e) {
                Plugin.Log.Critical(e.ToString());
            }
        }

        [UIAction("#post-parse")]
        private void setupLists() {
            spinSpeed = sharedValues.speed;
            spinListData.Data.Clear();
            foreach (var profile in Plugin.spinProfiles) {
                var tempCell = new CustomListTableData.CustomCellInfo(profile.name);

                spinListData.Data.Add(tempCell);
            }

            spinListData.TableView.ReloadData();
            spinListData.TableView.SelectCellWithIdx(PluginConfig.Instance.spinProfile);
        }

        public void AddTab() {
            GameplaySetup.Instance.AddTab("bSpin", "bSpin.UI.AngleChanger.bsml", this);
        }

        public void RemoveTab() {
            GameplaySetup.Instance.RemoveTab("bSpin");
        }
    }
}