using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
namespace bSpin.UI.Spin_Editor
{
    [ViewDefinition("bSpin.UI.SpinEditor.RotationPanel.bsml")]
    [HotReload(RelativePathToLayout = @"RotationPanel.bsml")]
    class RotationPanel : BSMLAutomaticViewController
    {
        [UIValue("easings-choices")]
        private List<object> options = Enum.GetNames(typeof(EasingFunction.Ease)).ToList<object>();

        EasingFunction.Ease EasingChoice;
        public static RotationPanel Instance;
        private static CustomTypes.Spin cacheSpin;
        private static CustomTypes.Spin editingSpin;
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

        internal void Load(CustomTypes.Spin spin)
        {
            RoundedEdge = Resources.FindObjectsOfTypeAll<Material>().Where(m => m.name == "UINoGlowRoundEdge").First();
            easeStringChoice = spin.Easing.ToString();
            cacheSpin = spin;
            editingSpin = spin;
            VectorList.Data.Clear();
            VectorList.Data.Add(new VectorListObject(spin.DelayBeforeSpin, "Delay Before", 0));
            VectorList.Data.Add(new VectorListObject(spin.Begin.x, "Start X", 1));
            VectorList.Data.Add(new VectorListObject(spin.Begin.y, "Start Y",2));
            VectorList.Data.Add(new VectorListObject(spin.Begin.z, "Start Z",3));
            VectorList.Data.Add(new VectorListObject(spin.End.x, "End X",4));
            VectorList.Data.Add(new VectorListObject(spin.End.y, "End Y",5));
            VectorList.Data.Add(new VectorListObject(spin.End.z, "End Z",6));
            VectorList.Data.Add(new VectorListObject(spin.DelayAfterSpin, "Delay After",7));
            VectorList.Data.Add(new VectorListObject(spin.Length, "Length",8));
            VectorList.TableView.ReloadData();
            tempObj = (VectorListObject)VectorList.Data[0];
            VectorList.TableView.SelectCellWithIdx(0);
        }


    }
}
