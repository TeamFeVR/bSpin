using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace bSpin.Configuration {
    internal class PluginConfig {
        public static PluginConfig Instance { get; set; }
        public virtual int spinProfile { get; set; }
        public virtual bool Enabled { get; set; } = true;
        public virtual bool WobbleEnabled { get; set; } = true;
        public virtual bool NoodleCompat { get; set; } = false;
        public virtual float SpinSpeed { get; set; } = 1.0f;
        public virtual bool AccountForLiv { get; set; } = false;
        public virtual bool PauseMenu { get; set; } = true;
        public virtual bool Experiments { get; set; } = true;
        public virtual int UdpPort { get; set; } = 3233;
        public virtual bool UdpEnabled { get; set; } = true;
        public virtual bool TwitchEnabled { get; set; } = true;
        public virtual bool TwitchAnnounce { get; set; } = true;
        
        public virtual void OnReload() {
        }
        
        public virtual void Changed() {
            // Do stuff when the config is changed.
        }
        
        public virtual void CopyFrom(PluginConfig other) {
            // This instance's members populated from other
        }
    }
}