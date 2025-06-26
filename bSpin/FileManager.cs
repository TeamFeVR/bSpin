using System.Collections.Generic;
using System.IO;
using System.Linq;
using bSpin.CustomTypes;
using IPA.Utilities;

namespace bSpin {
    internal class FileManager {
        public static List<SpinProfile> GetSpinProfiles() {
            var jsonFiles = Directory.GetFiles(Path.Combine(UnityGame.UserDataPath, "bSpin") + "\\", "*.json").ToList();
            var spinProfiles = new List<SpinProfile>();
            foreach (var sp in jsonFiles) {
                var tempSpin = new SpinProfile();
                Plugin.Log.Debug("Caching profile at " + sp);

                tempSpin.spins = SpinTools.LoadJson(sp);
                tempSpin.name = sp.Substring((Path.Combine(UnityGame.UserDataPath, "bSpin") + "\\").Length);
                tempSpin.jsonPath = sp;

                spinProfiles.Add(tempSpin);
            }

            return spinProfiles;
        }

        public static void SaveSpinProfile(SpinProfile profile) {
            SpinTools.SaveJson(profile.spins, profile.jsonPath);
        }

        public static List<WobbleProfile> GetWobbleProfiles() {
            var directory = Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles") + Path.DirectorySeparatorChar;
            var jsonFiles = Directory
                .GetFiles(directory, "*.json").ToList();
            var spinProfiles = new List<WobbleProfile>();
            foreach (var sp in jsonFiles) {
                var tempSpin = new WobbleProfile();
                Plugin.Log.Debug("Caching profile at " + sp);

                tempSpin.Wobbles = WobbleTools.LoadJson(sp);
                tempSpin.Name = sp.Substring(directory.Length);
                tempSpin.Name = tempSpin.Name.Substring(0, tempSpin.Name.IndexOf(".json"));
                tempSpin.JsonPath = sp;

                spinProfiles.Add(tempSpin);
            }

            return spinProfiles;
        }

        public static void SaveWobbleProfile(WobbleProfile profile) {
            WobbleTools.SaveJson(profile.Wobbles, profile.JsonPath);
        }
    }
}