using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using bSpin.CustomTypes;
using HMUI;
using TMPro;
using UnityEngine;

namespace bSpin.UI.Wobble_Editor {
    [ViewDefinition("bSpin.UI.WobbleEditor.RotationPanel.bsml")]
    [HotReload(RelativePathToLayout = @"RotationPanel.bsml")]
    internal class RotationPanel : BSMLAutomaticViewController {
        public static RotationPanel Instance;
        private static Wobble cacheSpin;
        private static Wobble editingSpin;
        internal static bool ChangesMade;
        internal static Material RoundedEdge;
        private static VectorListObject tempObj;

        [UIComponent("kb-text")] private static TextMeshProUGUI kbText;

        private EasingFunction.Ease EasingChoice;

        [UIValue("easings-choices")]
        private List<object> options = Enum.GetNames(typeof(EasingFunction.Ease)).ToList<object>();

        [UIParams] private BSMLParserParams parserParams;

        private string temp = "";

        [UIComponent("vector-list")] internal CustomCellListTableData VectorList;

        [UIValue("vectors")] public List<object> vectorsList = new List<object>();

        [UIValue("easing-choice")]
        private string easeStringChoice {
            get => EasingChoice.ToString();
            set {
                Enum.TryParse(value, out EasingChoice);
                editingSpin.Easing = EasingChoice;
                NotifyPropertyChanged();
            }
        }

        [UIValue("numpad-number-preview")]
        private string numpadPreview {
            get => temp;
            set {
                temp = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("changed")]
        internal bool changed {
            get => ChangesMade;
            set {
                ChangesMade = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("unchanged")] private bool unchanged => !ChangesMade;

        private void Awake() {
            Instance = this;
        }

        [UIAction("revert-edits")]
        private void Revert() {
            editingSpin = cacheSpin;
            Load(cacheSpin);
            ChangesMade = false;
        }

        [UIAction("apply-edits")]
        private void Apply() {
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
        internal static void VectorSelected(TableView sender, VectorListObject obj) {
            tempObj = obj;
            Instance.numpadPreview = obj.vector.ToString();
        }


        [UIAction("numpad-confirm")]
        private void NumpadConfirm() {
            ChangesMade = true;
            parserParams.EmitEvent("numpad-confirm");
            var tmp = int.Parse(numpadPreview);
            switch (tempObj.vectorIndex) {
                case 0:
                    editingSpin.BeginPos.x = int.Parse(numpadPreview);
                    break;
                case 1:
                    editingSpin.BeginPos.y = int.Parse(numpadPreview);
                    break;
                case 2:
                    editingSpin.BeginPos.z = int.Parse(numpadPreview);
                    break;
                case 3:
                    editingSpin.EndPos.x = int.Parse(numpadPreview);
                    break;
                case 4:
                    editingSpin.EndPos.y = int.Parse(numpadPreview);
                    break;
                case 5:
                    editingSpin.EndPos.z = int.Parse(numpadPreview);
                    break;
                case 6:
                    editingSpin.BeginRot.x = int.Parse(numpadPreview);
                    break;
                case 7:
                    editingSpin.BeginRot.y = int.Parse(numpadPreview);
                    break;
                case 8:
                    editingSpin.BeginRot.z = int.Parse(numpadPreview);
                    break;
                case 9:
                    editingSpin.EndPos.x = int.Parse(numpadPreview);
                    break;
                case 10:
                    editingSpin.EndPos.y = int.Parse(numpadPreview);
                    break;
                case 11:
                    editingSpin.EndPos.z = int.Parse(numpadPreview);
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

        internal void Load(Wobble spin) {
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
            VectorList.Data.Add(new VectorListObject(spin.Length, "Length", 12));
            VectorList.TableView.ReloadData();
            tempObj = (VectorListObject)VectorList.Data[0];
            VectorList.TableView.SelectCellWithIdx(0);
        }

        public class VectorListObject {
            [UIComponent("bgContainer")] internal ImageView bg = null;

            public float vector;
            public int vectorIndex;

            [UIValue("vector-name")] private string vectorName;


            public VectorListObject(float vector, string name, int index) {
                this.vector = (int)vector;
                vectorName = name;
                vectorValue = vector.ToString();
                vectorIndex = index;
            }

            [UIValue("vector-value")]
            public string vectorValue {
                get => vector.ToString();
                set => vector = float.Parse(value);
            }

            [UIAction("test-selector")]
            private void ExperimentalSelect() {
                Instance.numpadPreview = vector.ToString();
                tempObj = this;
                Instance.parserParams.EmitEvent("number-picker");
                Refresh(true, false);
            }

            [UIAction("refresh-visuals")]
            public void Refresh(bool selected, bool highlighted) {
                bg.material = RoundedEdge;
                var x = new Color(0, 0, 0, 0.45f);
                if (selected || highlighted) {
                    x.a = selected ? 0.9f : 0.6f;
                    x.r = selected ? 0.45f : 0.2f;
                    x.g = selected ? 0.45f : 0.2f;
                    x.b = selected ? 0.9f : 0.6f;
                }

                bg.color = x;
            }
        }
    }
}