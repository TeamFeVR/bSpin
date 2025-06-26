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
                    editingSpin.BeginPos.x = Int32.Parse(numpadPreview);
                    break;
                case 1:
                    editingSpin.BeginPos.y = Int32.Parse(numpadPreview);
                    break;
                case 2:
                    editingSpin.BeginPos.z = Int32.Parse(numpadPreview);
                    break;
                case 3:
                    editingSpin.EndPos.x = Int32.Parse(numpadPreview);
                    break;
                case 4:
                    editingSpin.EndPos.y = Int32.Parse(numpadPreview);
                    break;
                case 5:
                    editingSpin.EndPos.z = Int32.Parse(numpadPreview);
                    break;
                case 6:
                    editingSpin.BeginRot.x = Int32.Parse(numpadPreview);
                    break;
                case 7:
                    editingSpin.BeginRot.y = Int32.Parse(numpadPreview);
                    break;
                case 8:
                    editingSpin.BeginRot.z = Int32.Parse(numpadPreview);
                    break;
                case 9:
                    editingSpin.EndPos.x = Int32.Parse(numpadPreview);
                    break;
                case 10:
                    editingSpin.EndPos.y = Int32.Parse(numpadPreview);
                    break;
                case 11:
                    editingSpin.EndPos.z = Int32.Parse(numpadPreview);
                    break;
                case 12:
                    if (tmp < 0)
                        tmp = -tmp;
                    editingSpin.Length = tmp;
                    break;
            }
            ((VectorListObject)VectorList.Data[tempObj.vectorIndex]).vectorValue = tmp.ToString();
            VectorList.TableView.ReloadData();
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
            VectorList.Data.Clear();
            VectorList.Data.Add(new VectorListObject(spin.BeginPos.x, "Start X Pos", 0));
            VectorList.Data.Add(new VectorListObject(spin.BeginPos.y, "Start Y Pos", 1));
            VectorList.Data.Add(new VectorListObject(spin.BeginPos.z, "Start Z Pos", 2));
            VectorList.Data.Add(new VectorListObject(spin.EndPos.x, "End X Pos", 3));
            VectorList.Data.Add(new VectorListObject(spin.EndPos.y, "End Y Pos", 4));
            VectorList.Data.Add(new VectorListObject(spin.EndPos.z, "End Z Pos", 5));
            VectorList.Data.Add(new VectorListObject(spin.BeginRot.x, "Start X Rotation", 6));
            VectorList.Data.Add(new VectorListObject(spin.BeginRot.y, "Start Y Rotation", 7));
            VectorList.Data.Add(new VectorListObject(spin.BeginRot.z, "Start Z Rotation", 8));
            VectorList.Data.Add(new VectorListObject(spin.EndRot.x, "End X Rotation", 9));
            VectorList.Data.Add(new VectorListObject(spin.EndRot.y, "End Y Rotation", 10));
            VectorList.Data.Add(new VectorListObject(spin.EndRot.z, "End Z Rotation", 11));
            VectorList.Data.Add(new VectorListObject(spin.Length, "Length",12));
            VectorList.TableView.ReloadData();
            tempObj = (VectorListObject)VectorList.Data[0];
            VectorList.TableView.SelectCellWithIdx(0);
        }
    }
}
