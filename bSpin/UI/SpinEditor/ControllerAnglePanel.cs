﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;

namespace bSpin.UI.Spin_Editor
{
    [ViewDefinition("bSpin.UI.SpinEditor.ControllerAngle.bsml")]
    [HotReload(RelativePathToLayout = @"\ControllerAngle.bsml")]
    class ControllerAnglePanel : BSMLAutomaticViewController
    {
        [UIValue("calibrating")]
        internal bool isCalibrating = false;

        [UIValue("hand-options")]
        internal List<object> handOptions = new object[] { "Left", "Right" }.ToList();
        [UIValue("hand-value")]
        internal string handValue = "";
        [UIComponent("cal-type-list")]
        internal CustomCellListTableData tableList;

        public class CalibListCell
        {
            [UIValue("text")] private string Label = "FUCK";

            public CalibListCell(string label)
            {
                this.Label = label;
            }
        }

        [UIAction("#post-parse")]
        private void PostParse()
        {
            tableList.data.AddRange(new CustomListTableData.CustomCellInfo[]
            {
                new CustomListTableData.CustomCellInfo
                (
                    "Changelog\n",
                    "The changelog for Enhancements"

                )
            });
            tableList.tableView.ReloadData();
        }

    }
}
