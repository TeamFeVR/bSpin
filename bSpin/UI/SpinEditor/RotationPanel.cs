using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bSpin.UI.Spin_Editor
{
    class RotationPanel : BSMLResourceViewController
    {
        public override string ResourceName => "bSpin.UI.SpinEditor.RotationPanel.bsml";
        public static RotationPanel Instance;
        private static CustomTypes.Spin editingSpin;
        internal static bool ChangesMade = false;
        private void Awake()
        {
            Instance = this;
        }

        [UIValue("changed")] bool changed => ChangesMade;
        [UIValue("unchanged")] bool unchanged => !ChangesMade;

        [UIValue("start-delay")]
        string StartDelay
        {
            get => editingSpin.DelayBeforeSpin.ToString();
            set
            {
                ChangesMade = true;
                editingSpin.DelayBeforeSpin = Int32.Parse(value);
                NotifyPropertyChanged();
            }
        }
        [UIValue("s-rot-x")] string SRotX
        {
            get => editingSpin.Begin.x.ToString();
            set
            {
                ChangesMade = true;
                editingSpin.Begin.x = Int32.Parse(value);
                NotifyPropertyChanged();
            }
        }
        [UIValue("s-rot-y")] string SRotY
        {
            get => editingSpin.Begin.y.ToString();
            set
            {
                ChangesMade = true;
                editingSpin.Begin.y = Int32.Parse(value);
                NotifyPropertyChanged();
            }
        }
        [UIValue("s-rot-z")] string SRotZ
        {
            get => editingSpin.Begin.z.ToString();
            set
            {
                ChangesMade = true;
                editingSpin.Begin.z= Int32.Parse(value);
                NotifyPropertyChanged();
            }
        }
        [UIValue("e-rot-x")] static string ERotX = "";
        [UIValue("e-rot-y")] static string ERotY = "";
        [UIValue("e-rot-z")] static string ERotZ = "";
        [UIValue("end-delay")] static string EndDelay = "";
        [UIValue("spin-length")] static string SpinLength = "";

        [UIAction("revert-changes")]
        void Revert()
        {
            ChangesMade = false;
        }
        [UIAction("apply-changes")]
        void Apply()
        {
            ChangesMade = false;
        }
        internal void Load(CustomTypes.Spin spin)
        {
            StartDelay = spin.DelayBeforeSpin.ToString();
            SRotX = spin.Begin.x.ToString();
            SRotY = spin.Begin.y.ToString();
            SRotZ = spin.Begin.z.ToString();
            ERotX = spin.End.x.ToString();
            ERotY = spin.End.y.ToString();
            ERotZ = spin.End.z.ToString();
            EndDelay = spin.DelayAfterSpin.ToString();
            SpinLength = spin.Length.ToString();
        }


    }
}
