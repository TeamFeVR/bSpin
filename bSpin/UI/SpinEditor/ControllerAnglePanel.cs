﻿using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;

namespace bSpin.UI.Spin_Editor {
    [ViewDefinition("bSpin.UI.SpinEditor.ControllerAngle.bsml")]
    [HotReload(RelativePathToLayout = @"\ControllerAngle.bsml")]
    internal class ControllerAnglePanel : BSMLAutomaticViewController {
        [UIValue("hand-options")] internal List<object> handOptions = new object[] { "Left", "Right" }.ToList();

        [UIValue("hand-value")] internal string handValue = "";

        [UIValue("calibrating")] internal bool isCalibrating = false;

        [UIComponent("cal-type-list")] internal CustomCellListTableData tableList;

        [UIAction("#post-parse")]
        private void PostParse() {
            tableList.Data.Add(new CustomListTableData.CustomCellInfo(
                "WIP\n", "Idk what this was"));
            tableList.TableView.ReloadData();
        }

        public class CalibListCell {
            [UIValue("text")] private string Label = "FUCK";

            public CalibListCell(string label) {
                Label = label;
            }
        }
    }
}