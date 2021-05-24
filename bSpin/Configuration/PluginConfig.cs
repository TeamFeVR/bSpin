
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using bSpin.CustomTypes;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace bSpin.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual int spinProfile {get; set;}
        public virtual bool Enabled { get; set; } = true;
        public virtual bool NoodleCompat { get; set; } = false;
        public virtual float SpinSpeed { get; set; } = 1.0f;
        public virtual bool AccountForLiv { get; set; } = false;
        public virtual bool PauseMenu { get; set; } = true;

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {

        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }
}
