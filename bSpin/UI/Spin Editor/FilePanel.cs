using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bSpin.UI.Spin_Editor
{
    class FilePanel : BSMLResourceViewController
    {
        public override string ResourceName => "bSpin.UI.IngameMenu.FilePanel.bsml";

        [UIComponent("SpinList")] public CustomListTableData spinListData = AngleChanger.instance.spinListData;

        [UIAction("fileSelect")]
        void profileSelected(TableView _, int row)
        {
            
        }
    }
}
