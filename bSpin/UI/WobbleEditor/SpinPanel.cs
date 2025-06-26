using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using bSpin.Configuration;
using bSpin.CustomTypes;
using HMUI;
using UnityEngine;

namespace bSpin.UI.Wobble_Editor {
    [ViewDefinition("bSpin.UI.WobbleEditor.SpinPanel.bsml")]
    [HotReload(RelativePathToLayout = @"SpinPanel.bsml")]
    internal class SpinPanel : BSMLAutomaticViewController {
        internal static SpinPanel Instance;
        internal static int selectedSpin;
        internal static WobbleProfile cacheSpins;
        internal static WobbleProfile tempSpins;
        internal static Material RoundedEdge;


        [UIComponent("spin-list")] internal CustomCellListTableData SpinList;

        [UIValue("spins")] public List<object> spinsList = new List<object>();

        [UIValue("spin-name")]
        internal string SpinName {
            get => tempSpins.Name;
            set {
                NotifyPropertyChanged();
                tempSpins.Name = value;
            }
        }

        internal void Awake() {
            Instance = this;
        }

        [UIAction("#post-parse")]
        private void PostParse() {
            RoundedEdge = Resources.FindObjectsOfTypeAll<Material>().Where(m => m.name == "UINoGlowRoundEdge").First();
            SpinList.TableView.SelectCellWithIdx(selectedSpin);
        }

        [UIAction("spin-selected")]
        internal static void SpinSelected(TableView sender, SpinListObject obj) {
            RotationPanel.Instance.Load(obj.spin);
            selectedSpin = obj.index;
        }

        [UIAction("apply-edits")]
        internal void ApplyEdits() {
            cacheSpins = tempSpins;
            Plugin.wobbles.RemoveAt(selectedSpin);
            Plugin.wobbles.Insert(selectedSpin, tempSpins);
            FileManager.SaveWobbleProfile(tempSpins);
        }

        [UIAction("revert-edits")]
        internal void RevertEdits() {
            Plugin.spinProfiles = FileManager.GetSpinProfiles();
            cacheSpins = Plugin.wobbles.ElementAt(PluginConfig.Instance.spinProfile);
            tempSpins = cacheSpins;
            ReloadSpins();
        }

        [UIAction("move-up")]
        internal void MoveSpinUp() {
            if (selectedSpin > 0) {
                var spin = tempSpins.Wobbles.ElementAt(selectedSpin);
                tempSpins.Wobbles.RemoveAt(selectedSpin);
                tempSpins.Wobbles.Insert(selectedSpin - 1, spin);
                selectedSpin -= 1;
                ReloadSpins();
                SpinList.TableView.SelectCellWithIdx(selectedSpin);
            }
        }

        [UIAction("move-down")]
        internal void MoveSpinDown() {
            if (selectedSpin < tempSpins.Wobbles.Count - 1) {
                var spin = tempSpins.Wobbles.ElementAt(selectedSpin);
                tempSpins.Wobbles.RemoveAt(selectedSpin);
                tempSpins.Wobbles.Insert(selectedSpin + 1, spin);
                selectedSpin += 1;
                ReloadSpins();
                SpinList.TableView.SelectCellWithIdx(selectedSpin);
            }
        }

        [UIAction("add")]
        internal void AddSpin() {
            tempSpins.Wobbles.Add(new Wobble(0, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero));
            ReloadSpins();
            selectedSpin = tempSpins.Wobbles.Count - 1;
            SpinList.TableView.SelectCellWithIdx(tempSpins.Wobbles.Count - 1);
            RotationPanel.Instance.Load(tempSpins.Wobbles.ElementAt(selectedSpin));
        }

        [UIAction("remove")]
        internal void RemoveSpin() {
            tempSpins.Wobbles.RemoveAt(selectedSpin);
            ReloadSpins();
            SpinList.TableView.SelectCellWithIdx(selectedSpin - 1);
            selectedSpin = tempSpins.Wobbles.Count - 1;
            RotationPanel.Instance.Load(tempSpins.Wobbles.ElementAt(selectedSpin));
        }

        internal static void ReplaceSpin(Wobble spin) {
            tempSpins.Wobbles.RemoveAt(selectedSpin);
            tempSpins.Wobbles.Insert(selectedSpin, spin);
            Instance.ReloadSpins();
        }

        internal void ReloadSpins() {
            spinsList.Clear();
            var i = 0;
            foreach (var speen in tempSpins.Wobbles) {
                spinsList.Add(new SpinListObject(speen, i));
                i++;
            }

            i = 0;
            SpinList.TableView.ReloadData();
        }


        internal void LoadInList(WobbleProfile profile) {
            cacheSpins = profile;
            tempSpins = profile;
            spinsList.Clear();
            var i = 0;
            foreach (var speen in profile.Wobbles) {
                spinsList.Add(new SpinListObject(speen, i));
                i++;
            }

            i = 0;
            SpinList.TableView.ReloadData();
            RotationPanel.Instance.Load(tempSpins.Wobbles.ElementAt(selectedSpin));
        }

        public class SpinListObject {
            [UIComponent("bgContainer")] internal ImageView bg;

            [UIValue("easing-label")] private string EasingLabel;

            [UIValue("spin-end-delay")] private string endDelay;

            public int index;

            [UIValue("pos-vectors")] private string posVectors;

            public Wobble spin;

            [UIValue("spin-length")] private string spinLength;

            [UIValue("spin-vectors")] private string spinVectors;

            [UIValue("spin-start-delay")] private string startDelay;

            public SpinListObject(Wobble spin, int index) {
                this.spin = spin;
                this.index = index;
                spinLength = "In " + spin.Length + " Seconds";
                EasingLabel = spin.Easing.ToString();
                spinVectors = "Rot: (<#FF0000>" + spin.BeginRot.x + "<#FFFFFF>,<#00FF00>" + spin.BeginRot.y +
                              "<#FFFFFF>,<#0000FF>" + spin.BeginRot.z + "<#FFFFFF>) to (<#FF0000>" + spin.EndRot.x +
                              "<#FFFFFF>,<#00FF00>" + spin.EndRot.y + "<#FFFFFF>,<#0000FF>" + spin.EndRot.z +
                              "<#FFFFFF>)";
                posVectors = "Pos: (<#FF0000>" + spin.BeginPos.x + "<#FFFFFF>,<#00FF00>" + spin.BeginPos.y +
                             "<#FFFFFF>,<#0000FF>" + spin.BeginPos.z + "<#FFFFFF>) to (<#FF0000>" + spin.EndPos.x +
                             "<#FFFFFF>,<#00FF00>" + spin.EndPos.y + "<#FFFFFF>,<#0000FF>" + spin.EndPos.z +
                             "<#FFFFFF>)";
            }

            [UIAction("refresh-visuals")]
            public void Refresh(bool selected, bool highlighted) {
                bg.material = RoundedEdge;
                var x = new Color(0, 0, 0, 0.45f);
                if (selected || highlighted) {
                    x.a = selected ? 0.75f : 0.6f;
                    x.r = selected ? 0.75f : 0.3f;
                    x.g = selected ? 0.75f : 0.3f;
                    x.b = selected ? 0.9f : 0.6f;
                }

                bg.color = x;
            }
        }
    }
}