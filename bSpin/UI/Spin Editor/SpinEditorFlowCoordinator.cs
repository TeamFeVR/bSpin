using BeatSaberMarkupLanguage;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bSpin.UI.Spin_Editor
{
    internal class SpinEditorFlowCoordinator : FlowCoordinator
    {
        
        private FilePanel _filePanel;
        private SpinPanel _spinPanel;
        private RotationPanel _rotationPanel;
        internal static FlowCoordinator _previousFlowCoordinator;
        internal static SpinEditorFlowCoordinator Instance = new SpinEditorFlowCoordinator();
        public void Awake()
        {
            if (!_filePanel)
                _filePanel = BeatSaberUI.CreateViewController<FilePanel>();
            if (!_spinPanel)
                _spinPanel = BeatSaberUI.CreateViewController<SpinPanel>();
            if (!_rotationPanel)
                _rotationPanel = BeatSaberUI.CreateViewController<RotationPanel>();
        }
        internal static void Show()
        {
            FlowCoordinator flowCoordinator = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
            if (!Instance)
            {
                Plugin.bSpinController.AddComponent<SpinEditorFlowCoordinator>();
                Instance = Plugin.bSpinController.GetComponent<SpinEditorFlowCoordinator>();
            }
            if (!Instance)
                Plugin.Log.Notice("No instance of new flow coordinator");
            if (!flowCoordinator)
                Plugin.Log.Notice("No flow coordinator to dismiss");
            SetPreviousFlowCoordinator(flowCoordinator);
            flowCoordinator.PresentFlowCoordinator(Instance, null, ViewController.AnimationDirection.Horizontal, false, true);
        }

        internal static void SetPreviousFlowCoordinator(FlowCoordinator it)
        {
            _previousFlowCoordinator = it;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            try
            {
                if (firstActivation)
                {
                    SetTitle("Spin Editor");
                    showBackButton = true;
                    ProvideInitialViewControllers(_spinPanel, _filePanel, _rotationPanel);
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e);
            }
        }
        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.DismissFlowCoordinator(_previousFlowCoordinator, Instance, null, ViewController.AnimationDirection.Horizontal, false);
        }
    }
}
