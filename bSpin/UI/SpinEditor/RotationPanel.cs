using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace bSpin.UI.Spin_Editor
{
    class RotationPanel : BSMLResourceViewController
    {
        public override string ResourceName => "bSpin.UI.SpinEditor.RotationPanel.bsml";
        public static RotationPanel Instance;
        private static CustomTypes.Spin cacheSpin;
        private static CustomTypes.Spin editingSpin;
        internal static bool ChangesMade = false;
        private void Awake()
        {
            Instance = this;
        }
        private static VectorListObject tempObj;
        private string temp = "";
        [UIValue("numpad-number-preview")] string numpadPreview
        {
            get => temp;
            set
            {
                temp = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("changed")]
        internal bool changed
        {
            get => ChangesMade;
            set
            {
                ChangesMade = value;
                NotifyPropertyChanged();
            }
        }
        [UIValue("unchanged")] bool unchanged => !ChangesMade;

        [UIComponent("kb-text")] static TextMeshProUGUI kbText;

        [UIComponent("vector-list")]
        internal CustomCellListTableData VectorList;

        [UIValue("vectors")]
        public List<object> vectorsList = new List<object>();

        [UIAction("revert-edits")]
        void Revert()
        {
            editingSpin = cacheSpin;
            Load(cacheSpin);
            ChangesMade = false;
        }
        [UIAction("apply-edits")]
        void Apply()
        {
            cacheSpin = editingSpin;
            SpinPanel.ReplaceSpin(editingSpin);
            ChangesMade = false;
        }
        #region NUMPAD TIMEE
        [UIAction("1")] void one() => numpadPreview += "1";
        [UIAction("2")] void two() => numpadPreview += "2";
        [UIAction("3")] void three() => numpadPreview += "3";
        [UIAction("4")] void four() => numpadPreview += "4";
        [UIAction("5")] void five() => numpadPreview += "5";
        [UIAction("6")] void six() => numpadPreview += "6";
        [UIAction("7")] void seven() => numpadPreview += "7";
        [UIAction("8")] void eight() => numpadPreview += "8";
        [UIAction("9")] void nine() => numpadPreview += "9";
        [UIAction("0")] void zero() => numpadPreview += "0";
        [UIAction("backspace")] void backspace() => numpadPreview = numpadPreview.Substring(0, numpadPreview.Length - 1);
        [UIAction("negative")]
        void negate()
        {
            if(!numpadPreview.Contains("-"))
                numpadPreview += "-";
        }
        #endregion


        [UIAction("vector-selected")]
        internal static void VectorSelected(TableView sender, VectorListObject obj)
        {
            tempObj = obj;
            Instance.numpadPreview = obj.vector.ToString();
        }

        [UIParams]
        BSMLParserParams parserParams;


        [UIAction("numpad-confirm")]
        void NumpadConfirm()
        {
            ChangesMade = true;
            parserParams.EmitEvent("numpad-confirm");
            int tmp = Int32.Parse(numpadPreview);
            switch (tempObj.vectorIndex)
            {
                case 0:
                    
                    if (tmp < 0)
                        tmp = -tmp;
                    editingSpin.DelayBeforeSpin = tmp;
                    break;
                case 1:
                    editingSpin.Begin.x = Int32.Parse(numpadPreview);
                    break;
                case 2:
                    editingSpin.Begin.y = Int32.Parse(numpadPreview);
                    break;
                case 3:
                    editingSpin.Begin.z = Int32.Parse(numpadPreview);
                    break;
                case 4:
                    editingSpin.End.x = Int32.Parse(numpadPreview);
                    break;
                case 5:
                    editingSpin.End.y = Int32.Parse(numpadPreview);
                    break;
                case 6:
                    editingSpin.End.z = Int32.Parse(numpadPreview);
                    break;
                case 7:
                    if (tmp < 0)
                        tmp = -tmp;
                    editingSpin.DelayAfterSpin = tmp;
                    break;
                case 8:
                    if (tmp < 0)
                        tmp = -tmp;
                    editingSpin.Length = tmp;
                    break;
            }
            ((VectorListObject)VectorList.data.ElementAt(tempObj.vectorIndex)).vectorValue = tmp.ToString();
            VectorList.tableView.ReloadData();
            numpadPreview = "";
        }

        public class VectorListObject
        {
            public float vector;
            public int vectorIndex;
            [UIValue("vector-name")]
            private string vectorName;

            [UIValue("vector-value")]
            public string vectorValue
            {
                get => vector.ToString();
                set
                {
                    vector = float.Parse(value);
                }
            }

            [UIComponent("bg")]
            private ImageView background;

            public VectorListObject(float vector, string name, int index)
            {
                this.vector = (int)vector;
                this.vectorName = name;
                this.vectorValue = vector.ToString();
                this.vectorIndex = index;
            }
            [UIAction("refresh-visuals")]
            public void Refresh(bool selected, bool highlighted)
            {
                var x = new UnityEngine.Color(0, 0, 0, 0.45f);

                if (selected || highlighted)
                    x.a = selected ? 0.9f : 0.6f;

                background.color = x;
            }
        }

        internal void Load(CustomTypes.Spin spin)
        {
            cacheSpin = spin;
            editingSpin = spin;
            VectorList.data.Clear();
            VectorList.data.Add(new VectorListObject(spin.DelayBeforeSpin, "Delay Before", 0));
            VectorList.data.Add(new VectorListObject(spin.Begin.x, "Start X", 1));
            VectorList.data.Add(new VectorListObject(spin.Begin.y, "Start Y",2));
            VectorList.data.Add(new VectorListObject(spin.Begin.z, "Start Z",3));
            VectorList.data.Add(new VectorListObject(spin.End.x, "End X",4));
            VectorList.data.Add(new VectorListObject(spin.End.y, "End Y",5));
            VectorList.data.Add(new VectorListObject(spin.End.z, "End Z",6));
            VectorList.data.Add(new VectorListObject(spin.DelayAfterSpin, "Delay After",7));
            VectorList.data.Add(new VectorListObject(spin.Length, "Length",8));
            VectorList.tableView.ReloadData();
            tempObj = (VectorListObject)VectorList.data.ElementAt(0);
            VectorList.tableView.SelectCellWithIdx(0);
        }


    }
}
