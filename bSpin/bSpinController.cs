using UnityEngine;

namespace bSpin {
    public class bSpinController : MonoBehaviour {
        public static bSpinController Instance { get; private set; }

        #region Monobehaviour Messages
        
        private void Awake() {
            if (Instance != null) {
                Plugin.Log?.Warn($"Instance of {GetType().Name} already exists, destroying.");
                DestroyImmediate(this);
                return;
            }

            DontDestroyOnLoad(this); // Don't destroy this object on scene changes
            Instance = this;
            Plugin.Log?.Debug($"{name}: Awake()");
        }

        
        private void OnDestroy() {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            if (Instance == this)
                Instance = null;
        }

        #endregion
    }
}