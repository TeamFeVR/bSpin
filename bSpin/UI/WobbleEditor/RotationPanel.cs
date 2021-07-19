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

namespace bSpin.UI.Wobble_Editor
{
    [ViewDefinition("bSpin.UI.WobbleEditor.RotationPanel.bsml")]
    [HotReload(RelativePathToLayout = @"RotationPanel.bsml")]
    class RotationPanel : BSMLAutomaticViewController
    {
        [UIValue("easings-choices")]
        private List<object> options = Enum.GetNames(typeof(EasingFunction.Ease)).ToList<object>();

        EasingFunction.Ease EasingChoice;
        public static RotationPanel Instance;
        private static CustomTypes.Wobble cacheSpin;
        private static CustomTypes.Wobble editingSpin;
        internal static bool ChangesMade = false;
        internal static Material RoundedEdge = null;
        private void Awake()
        {
            Instance = this;
        }
        private static VectorListObject tempObj;
        private string temp = "";

        [UIValue("easing-choice")]
        private string easeStringChoice
        {
            get => EasingChoice.ToString();
            set
            {
                Enum.TryParse<EasingFunction.Ease>(value, out EasingChoice);
                editingSpin.Easing = EasingChoice;
                NotifyPropertyChanged();
            }
        }

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
                    editingSpin.Begin_Pos.x = Int32.Parse(numpadPreview);
                    break;
                case 1:
                    editingSpin.Begin_Pos.y = Int32.Parse(numpadPreview);
                    break;
                case 2:
                    editingSpin.Begin_Pos.z = Int32.Parse(numpadPreview);
                    break;
                case 3:
                    editingSpin.End_Pos.x = Int32.Parse(numpadPreview);
                    break;
                case 4:
                    editingSpin.End_Pos.y = Int32.Parse(numpadPreview);
                    break;
                case 5:
                    editingSpin.End_Pos.z = Int32.Parse(numpadPreview);
                    break;
                case 6:
                    editingSpin.Begin_Rot.x = Int32.Parse(numpadPreview);
                    break;
                case 7:
                    editingSpin.Begin_Rot.y = Int32.Parse(numpadPreview);
                    break;
                case 8:
                    editingSpin.Begin_Rot.z = Int32.Parse(numpadPreview);
                    break;
                case 9:
                    editingSpin.End_Pos.x = Int32.Parse(numpadPreview);
                    break;
                case 10:
                    editingSpin.End_Pos.y = Int32.Parse(numpadPreview);
                    break;
                case 11:
                    editingSpin.End_Pos.z = Int32.Parse(numpadPreview);
                    break;
                case 12:
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

            [UIComponent("bgContainer")]
            internal ImageView bg = null;

            [UIAction("test-selector")]
            void ExperimentalSelect()
            {
                Instance.numpadPreview = vector.ToString();
                tempObj = this;
                Instance.parserParams.EmitEvent("number-picker");
                Refresh(true, false);
            }


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

                bg.material = RoundedEdge;
                var x = new Color(0,0,0,0.45f);
                if (selected || highlighted)
                {
                    x.a = selected ? 0.9f : 0.6f;
                    x.r = selected ? 0.45f : 0.2f;
                    x.g = selected ? 0.45f : 0.2f;
                    x.b = selected ? 0.9f : 0.6f;
                }
                bg.color = x;
            }
        }

        internal void Load(CustomTypes.Wobble spin)
        {
            RoundedEdge = Resources.FindObjectsOfTypeAll<Material>().Where(m => m.name == "UINoGlowRoundEdge").First();
            easeStringChoice = spin.Easing.ToString();
            cacheSpin = spin;
            editingSpin = spin;
            VectorList.data.Clear();
            VectorList.data.Add(new VectorListObject(spin.Begin_Pos.x, "Start X Pos", 0));
            VectorList.data.Add(new VectorListObject(spin.Begin_Pos.y, "Start Y Pos", 1));
            VectorList.data.Add(new VectorListObject(spin.Begin_Pos.z, "Start Z Pos", 2));
            VectorList.data.Add(new VectorListObject(spin.End_Pos.x, "End X Pos", 3));
            VectorList.data.Add(new VectorListObject(spin.End_Pos.y, "End Y Pos", 4));
            VectorList.data.Add(new VectorListObject(spin.End_Pos.z, "End Z Pos", 5));
            VectorList.data.Add(new VectorListObject(spin.Begin_Rot.x, "Start X Rotation", 6));
            VectorList.data.Add(new VectorListObject(spin.Begin_Rot.y, "Start Y Rotation", 7));
            VectorList.data.Add(new VectorListObject(spin.Begin_Rot.z, "Start Z Rotation", 8));
            VectorList.data.Add(new VectorListObject(spin.End_Rot.x, "End X Rotation", 9));
            VectorList.data.Add(new VectorListObject(spin.End_Rot.y, "End Y Rotation", 10));
            VectorList.data.Add(new VectorListObject(spin.End_Rot.z, "End Z Rotation", 11));
            VectorList.data.Add(new VectorListObject(spin.Length, "Length",12));
            VectorList.tableView.ReloadData();
            tempObj = (VectorListObject)VectorList.data.ElementAt(0);
            VectorList.tableView.SelectCellWithIdx(0);
        }
    }
}
