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

namespace bSpin.UI.Spin_Editor
{
    class SpinPanel : BSMLResourceViewController
    {
        internal static SpinPanel Instance;
        internal void Awake()
        {
            Instance = this;
        }
        internal static int selectedSpin = 0;
        public override string ResourceName => "bSpin.UI.SpinEditor.SpinPanel.bsml";
        internal static SpinProfile cacheSpins = new SpinProfile();
        internal static SpinProfile tempSpins = new SpinProfile();
        internal static Material RoundedEdge = null;

        [UIAction("#post-parse")]
        private void PostParse()
        {
            RoundedEdge = Resources.FindObjectsOfTypeAll<Material>().Where(m => m.name == "UINoGlowRoundEdge").First();
            SpinList.tableView.SelectCellWithIdx(selectedSpin);
            
        }

        [UIComponent("spin-list")]
        internal CustomCellListTableData SpinList;

        [UIValue("spins")]
        public List<object> spinsList = new List<object>();

        public class SpinListObject
        {
            public Spin spin;
            public int index;
            [UIValue("spin-start-delay")]
            private string startDelay;

            [UIValue("spin-vectors")]
            private string spinVectors;

            [UIValue("spin-length")]
            private string spinLength;

            [UIValue("spin-end-delay")]
            private string endDelay;

            [UIComponent("bgContainer")]
            internal ImageView bg;

            public SpinListObject(Spin spin, int index)
            {
                this.spin = spin;
                this.index = index;
                this.spinLength = "In " + spin.Length.ToString() + " Seconds";
                this.startDelay = spin.DelayBeforeSpin.ToString() + "s Before";
                this.endDelay = spin.DelayAfterSpin.ToString() + "s After";
                this.spinVectors = "(<#FF0000>" + spin.Begin.x + "<#FFFFFF>,<#00FF00>" + spin.Begin.y + "<#FFFFFF>,<#0000FF>" + spin.Begin.z + "<#FFFFFF>) to (<#FF0000>" + spin.End.x + "<#FFFFFF>,<#00FF00>" + spin.End.y + "<#FFFFFF>,<#0000FF>" + spin.End.z + "<#FFFFFF>)";
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
            Plugin.spinProfiles.RemoveAt(Configuration.PluginConfig.Instance.spinProfile);
            Plugin.spinProfiles.Insert(Configuration.PluginConfig.Instance.spinProfile, tempSpins);
            FileManager.SaveSpinProfile(tempSpins);
        }
        [UIAction("revert-edits")]
        internal void RevertEdits()
        {
            Plugin.spinProfiles = FileManager.GetSpinProfiles();
            cacheSpins = Plugin.spinProfiles.ElementAt(Configuration.PluginConfig.Instance.spinProfile);
            tempSpins = cacheSpins;
            ReloadSpins();
        }

        [UIAction("move-up")]
        internal void MoveSpinUp()
        {
            if(selectedSpin > 0)
            {
                var spin = tempSpins.spins.ElementAt(selectedSpin);
                tempSpins.spins.RemoveAt(selectedSpin);
                tempSpins.spins.Insert(selectedSpin - 1, spin);
                selectedSpin -= 1;
                ReloadSpins();
                SpinList.tableView.SelectCellWithIdx(selectedSpin);
            }
        }
        [UIAction("move-down")]
        internal void MoveSpinDown()
        {
            if (selectedSpin < tempSpins.spins.Count - 1)
            {
                var spin = tempSpins.spins.ElementAt(selectedSpin);
                tempSpins.spins.RemoveAt(selectedSpin);
                tempSpins.spins.Insert(selectedSpin + 1, spin);
                selectedSpin += 1;
                ReloadSpins();
                SpinList.tableView.SelectCellWithIdx(selectedSpin);
            }
        }
        [UIAction("add")]
        internal void AddSpin()
        {
            tempSpins.spins.Add(new Spin(0, 10, Vector3.zero, Vector3.zero, 0));
            ReloadSpins();
            selectedSpin = tempSpins.spins.Count - 1;
            SpinList.tableView.SelectCellWithIdx(tempSpins.spins.Count - 1);
            RotationPanel.Instance.Load(tempSpins.spins.ElementAt(selectedSpin));
        }
        [UIAction("remove")]
        internal void RemoveSpin()
        {
            tempSpins.spins.RemoveAt(selectedSpin);
            ReloadSpins();
            SpinList.tableView.SelectCellWithIdx(selectedSpin - 1);
            selectedSpin = tempSpins.spins.Count - 1;
            RotationPanel.Instance.Load(tempSpins.spins.ElementAt(selectedSpin));
        }

        internal static void ReplaceSpin(Spin spin)
        {
            tempSpins.spins.RemoveAt(selectedSpin);
            tempSpins.spins.Insert(selectedSpin, spin);
            Instance.ReloadSpins();
        }

        internal void ReloadSpins()
        {
            spinsList.Clear();
            int i = 0;
            foreach (var speen in tempSpins.spins)
            {
                spinsList.Add(new SpinListObject(speen, i));
                i++;
            }
            i = 0;
            SpinList.tableView.ReloadData();
        }


        internal void LoadInList(SpinProfile profile)
        {
            cacheSpins = profile;
            tempSpins = profile;
            spinsList.Clear();
            int i = 0;
            foreach(var speen in profile.spins)
            {
                spinsList.Add(new SpinListObject(speen, i));
                i++;
            }
            i = 0;
            SpinList.tableView.ReloadData();
            RotationPanel.Instance.Load(tempSpins.spins.ElementAt(selectedSpin));
        }
    }
}
