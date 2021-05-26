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

        public override string ResourceName => "bSpin.UI.SpinEditor.SpinPanel.bsml";
        private static SpinProfile spins = new SpinProfile();

        [UIComponent("spin-list")]
        internal CustomCellListTableData SpinList;

        [UIValue("spins")]
        public List<object> spinsList = new List<object>();

        public class SpinListObject
        {
            public Spin spin;

            [UIValue("spin-start-delay")]
            private string startDelay;

            [UIValue("spin-vectors")]
            private string spinVectors;

            [UIValue("spin-length")]
            private string spinLength;

            [UIValue("spin-end-delay")]
            private string endDelay;

            [UIComponent("bg")]
            private RawImage background;

            public SpinListObject(Spin spin)
            {
                this.spin = spin;
                this.spinLength = "In " + spin.Length.ToString() + " Seconds";
                this.startDelay = spin.DelayBeforeSpin.ToString() + "s Before";
                this.endDelay = spin.DelayAfterSpin.ToString() + "s After";
                this.spinVectors = "(<#FF0000>" + spin.Begin.x + "<#FFFFFF>,<#00FF00>" + spin.Begin.y + "<#FFFFFF>,<#0000FF>" + spin.Begin.z + "<#FFFFFF>) to (<#FF0000>" + spin.End.x + "<#FFFFFF>,<#00FF00>" + spin.End.y + "<#FFFFFF>,<#0000FF>" + spin.End.z + "<#FFFFFF>)";
            }
            [UIAction("refresh-visuals")]
            public void Refresh(bool selected, bool highlighted)
            {
                
                background.texture = Texture2D.whiteTexture;
                if (highlighted)
                    background.color = new Color(1f, 1f, 1f, 0.125f);
                if (selected)
                    background.color = new Color(0.5f, 0.5f, 1f, 0.5f);
            }
        }

        [UIAction("spin-selected")]
        internal static void SpinSelected(TableView sender, SpinListObject obj)
        {
            RotationPanel.Instance.Load(obj.spin);
        }


        internal void LoadInList(SpinProfile profile)
        {
            spinsList.Clear();
            foreach(var speen in profile.spins)
            {
                spinsList.Add(new SpinListObject(speen));
            }
            SpinList.tableView.ReloadData();
        }
    }
}
