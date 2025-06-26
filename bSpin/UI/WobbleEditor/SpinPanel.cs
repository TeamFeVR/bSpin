using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bSpin.CustomTypes;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;
using HMUI;

namespace bSpin.UI.Wobble_Editor
{
    [ViewDefinition("bSpin.UI.WobbleEditor.SpinPanel.bsml")]
    [HotReload(RelativePathToLayout = @"SpinPanel.bsml")]
    class SpinPanel : BSMLAutomaticViewController
    {
        internal static SpinPanel Instance;
        internal void Awake()
        {
            Instance = this;
        }
        internal static int selectedSpin = 0;
        internal static WobbleProfile cacheSpins = new WobbleProfile();
        internal static WobbleProfile tempSpins = new WobbleProfile();
        internal static Material RoundedEdge = null;

        [UIAction("#post-parse")]
        private void PostParse()
        {
            RoundedEdge = Resources.FindObjectsOfTypeAll<Material>().Where(m => m.name == "UINoGlowRoundEdge").First();
            SpinList.TableView.SelectCellWithIdx(selectedSpin);
            
        }
        [UIValue("spin-name")]
        internal string SpinName
        {
            get => tempSpins.Name;
            set
            {
                NotifyPropertyChanged();
                tempSpins.Name = value;
            }
        }


        [UIComponent("spin-list")]
        internal CustomCellListTableData SpinList;

        [UIValue("spins")]
        public List<object> spinsList = new List<object>();

        public class SpinListObject
        {
            public Wobble spin;
            public int index;
            [UIValue("spin-start-delay")]
            private string startDelay;

            [UIValue("spin-vectors")]
            private string spinVectors;
            
            [UIValue("pos-vectors")]
            private string posVectors;

            [UIValue("spin-length")]
            private string spinLength;

            [UIValue("spin-end-delay")]
            private string endDelay;

            [UIComponent("bgContainer")]
            internal ImageView bg;

            [UIValue("easing-label")]
            private string EasingLabel;

            public SpinListObject(Wobble spin, int index)
            {
                this.spin = spin;
                this.index = index;
                this.spinLength = "In " + spin.Length.ToString() + " Seconds";
                this.EasingLabel = spin.Easing.ToString();
                this.spinVectors = "Rot: (<#FF0000>" + spin.BeginRot.x + "<#FFFFFF>,<#00FF00>" + spin.BeginRot.y + "<#FFFFFF>,<#0000FF>" + spin.BeginRot.z + "<#FFFFFF>) to (<#FF0000>" + spin.EndRot.x + "<#FFFFFF>,<#00FF00>" + spin.EndRot.y + "<#FFFFFF>,<#0000FF>" + spin.EndRot.z + "<#FFFFFF>)";
                this.posVectors = "Pos: (<#FF0000>" + spin.BeginPos.x + "<#FFFFFF>,<#00FF00>" + spin.BeginPos.y + "<#FFFFFF>,<#0000FF>" + spin.BeginPos.z + "<#FFFFFF>) to (<#FF0000>" + spin.EndPos.x + "<#FFFFFF>,<#00FF00>" + spin.EndPos.y + "<#FFFFFF>,<#0000FF>" + spin.EndPos.z + "<#FFFFFF>)";
            }
            [UIAction("refresh-visuals")]
            public void Refresh(bool selected, bool highlighted)
            {
                bg.material = RoundedEdge;
                var x = new Color(0, 0, 0, 0.45f);
                if (selected || highlighted)
                {
                    x.a = selected ? 0.75f : 0.6f;
                    x.r = selected ? 0.75f : 0.3f;
                    x.g = selected ? 0.75f : 0.3f;
                    x.b = selected ? 0.9f : 0.6f;
                }
                bg.color = x;
            }
        }

        [UIAction("spin-selected")]
        internal static void SpinSelected(TableView sender, SpinListObject obj)
        {
            RotationPanel.Instance.Load(obj.spin);
            selectedSpin = obj.index;
        }

        [UIAction("apply-edits")]
        internal void ApplyEdits()
        {
            cacheSpins = tempSpins;
            Plugin.wobbles.RemoveAt(selectedSpin);
            Plugin.wobbles.Insert(selectedSpin, tempSpins);
            FileManager.SaveWobbleProfile(tempSpins);
        }
        [UIAction("revert-edits")]
        internal void RevertEdits()
        {
            Plugin.spinProfiles = FileManager.GetSpinProfiles();
            cacheSpins = Plugin.wobbles.ElementAt(Configuration.PluginConfig.Instance.spinProfile);
            tempSpins = cacheSpins;
            ReloadSpins();
        }

        [UIAction("move-up")]
        internal void MoveSpinUp()
        {
            if(selectedSpin > 0)
            {
                var spin = tempSpins.Wobbles.ElementAt(selectedSpin);
                tempSpins.Wobbles.RemoveAt(selectedSpin);
                tempSpins.Wobbles.Insert(selectedSpin - 1, spin);
                selectedSpin -= 1;
                ReloadSpins();
                SpinList.TableView.SelectCellWithIdx(selectedSpin);
            }
        }
        [UIAction("move-down")]
        internal void MoveSpinDown()
        {
            if (selectedSpin < tempSpins.Wobbles.Count - 1)
            {
                var spin = tempSpins.Wobbles.ElementAt(selectedSpin);
                tempSpins.Wobbles.RemoveAt(selectedSpin);
                tempSpins.Wobbles.Insert(selectedSpin + 1, spin);
                selectedSpin += 1;
                ReloadSpins();
                SpinList.TableView.SelectCellWithIdx(selectedSpin);
            }
        }
        [UIAction("add")]
        internal void AddSpin()
        {
            tempSpins.Wobbles.Add(new Wobble(0, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero));
            ReloadSpins();
            selectedSpin = tempSpins.Wobbles.Count - 1;
            SpinList.TableView.SelectCellWithIdx(tempSpins.Wobbles.Count - 1);
            RotationPanel.Instance.Load(tempSpins.Wobbles.ElementAt(selectedSpin));
        }
        [UIAction("remove")]
        internal void RemoveSpin()
        {
            tempSpins.Wobbles.RemoveAt(selectedSpin);
            ReloadSpins();
            SpinList.TableView.SelectCellWithIdx(selectedSpin - 1);
            selectedSpin = tempSpins.Wobbles.Count - 1;
            RotationPanel.Instance.Load(tempSpins.Wobbles.ElementAt(selectedSpin));
        }

        internal static void ReplaceSpin(Wobble spin)
        {
            tempSpins.Wobbles.RemoveAt(selectedSpin);
            tempSpins.Wobbles.Insert(selectedSpin, spin);
            Instance.ReloadSpins();
        }

        internal void ReloadSpins()
        {
            spinsList.Clear();
            int i = 0;
            foreach (var speen in tempSpins.Wobbles)
            {
                spinsList.Add(new SpinListObject(speen, i));
                i++;
            }
            i = 0;
            SpinList.TableView.ReloadData();
        }


        internal void LoadInList(WobbleProfile profile)
        {
            cacheSpins = profile;
            tempSpins = profile;
            spinsList.Clear();
            int i = 0;
            foreach(var speen in profile.Wobbles)
            {
                spinsList.Add(new SpinListObject(speen, i));
                i++;
            }
            i = 0;
            SpinList.TableView.ReloadData();
            RotationPanel.Instance.Load(tempSpins.Wobbles.ElementAt(selectedSpin));
        }
    }
}
