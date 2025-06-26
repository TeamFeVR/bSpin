using System;
using BeatSaberMarkupLanguage;
using HMUI;

namespace bSpin.UI.Wobble_Editor {
    internal class SpinEditorFlowCoordinator : FlowCoordinator {
        private static SpinPanel _spinPanel;
        private static RotationPanel _rotationPanel;

        internal static FlowCoordinator _previousFlowCoordinator;
        internal static SpinEditorFlowCoordinator Instance;

        public void Awake() {
            if (!_spinPanel)
                _spinPanel = BeatSaberUI.CreateViewController<SpinPanel>();
            if (!_rotationPanel)
                _rotationPanel = BeatSaberUI.CreateViewController<RotationPanel>();
        }


        internal static void SetPreviousFlowCoordinator(FlowCoordinator it) {
            _previousFlowCoordinator = it;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
            try {
                if (firstActivation) {
                    SetTitle("Wobble Editor");
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