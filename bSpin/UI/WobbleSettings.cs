using System;
using System.Linq;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Util;
using bSpin.Configuration;
using bSpin.HarmonyPatches;
using bSpin.UI.Wobble_Editor;
using HMUI;

namespace bSpin.UI {
    [HotReload(RelativePathToLayout = "AngleChanger.bsml")]
    internal class WobbleSettings : NotifiableSingleton<WobbleSettings> {
        private int SelectedEditWobble;
        [UIComponent("SpinList")] public CustomListTableData spinListData = new CustomListTableData();

        [UIValue("enablewobble")]
        public bool enableWobble {
            get => sharedValues.wobble;
            set => sharedValues.wobble = value;
        }

        [UIAction("profileSelect")]
        private void profileSelected(TableView _, int row) {
            SelectedEditWobble = row;
        }

        [UIAction("refresh")]
        internal void Refresh() {
            Plugin.wobbles = FileManager.GetWobbleProfiles();
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
                SpinPanel.Instance.LoadInList(Plugin.wobbles.ElementAt(SelectedEditWobble));
            }
            catch (Exception e) {
                Plugin.Log.Critical(e.ToString());
            }
        }

        [UIAction("#post-parse")]
        private void setupLists() {
            spinListData.Data.Clear();
            foreach (var profile in Plugin.wobbles) {
                var tempCell = new CustomListTableData.CustomCellInfo(profile.Name);

                spinListData.Data.Add(tempCell);
            }

            spinListData.TableView.ReloadData();
            spinListData.TableView.SelectCellWithIdx(PluginConfig.Instance.spinProfile);
        }

        public void AddTab() {
            //GameplaySetup.instance.AddTab("Wobbles", "bSpin.UI.WobbleSettings.bsml", this);
        }

        public void RemoveTab() {
            //GameplaySetup.instance.RemoveTab("Wobbles");
        }
    }
}