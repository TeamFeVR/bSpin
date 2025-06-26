using System;
using BeatSaberMarkupLanguage;
using HMUI;

namespace bSpin.UI.Spin_Editor {
    internal class SpinEditorFlowCoordinator : FlowCoordinator {
        private static SpinPanel _spinPanel;
        private static RotationPanel _rotationPanel;
        private static ControllerAnglePanel _controllerAnglePanel;

        internal static FlowCoordinator _previousFlowCoordinator;
        internal static SpinEditorFlowCoordinator Instance;

        public void Awake() {
            if (!_spinPanel)
                _spinPanel = BeatSaberUI.CreateViewController<SpinPanel>();
            if (!_rotationPanel)
                _rotationPanel = BeatSaberUI.CreateViewController<RotationPanel>();
            if (!_controllerAnglePanel)
                _controllerAnglePanel = BeatSaberUI.CreateViewController<ControllerAnglePanel>();
        }


        internal static void SetPreviousFlowCoordinator(FlowCoordinator it) {
            _previousFlowCoordinator = it;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
            try {
                if (firstActivation) {
                    SetTitle("Spin Editor");
                    showBackButton = true;
                    ProvideInitialViewControllers(_spinPanel, _rotationPanel);
                }
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController) {
            _previousFlowCoordinator.DismissFlowCoordinator(Instance);
        }
    }
}