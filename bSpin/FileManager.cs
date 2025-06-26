using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bSpin.CustomTypes;

namespace bSpin
{
    class FileManager
    {
        public static List<SpinProfile> GetSpinProfiles()
        {
            List<String> jsonFiles = Directory.GetFiles(Path.Combine(UnityGame.UserDataPath, "bSpin") + "\\", "*.json").ToList();
            List<SpinProfile> spinProfiles = new List<SpinProfile>();
            foreach(var sp in jsonFiles)
            {
                SpinProfile tempSpin = new SpinProfile();
                Plugin.Log.Debug("Caching profile at " + sp);

                tempSpin.spins = SpinTools.LoadJson(sp);
                tempSpin.name = sp.Substring((Path.Combine(UnityGame.UserDataPath, "bSpin") + "\\").Length);
                tempSpin.jsonPath = sp;

                spinProfiles.Add(tempSpin);
            }
            return spinProfiles;
        }
        public static void SaveSpinProfile(SpinProfile profile)
        {
            SpinTools.SaveJson(profile.spins, profile.jsonPath);
        }
        public static List<WobbleProfile> GetWobbleProfiles()
        {
            List<String> jsonFiles = Directory.GetFiles(Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles") + "\\", "*.json").ToList();
            List<WobbleProfile> spinProfiles = new List<WobbleProfile>();
            foreach (var sp in jsonFiles)
            {
                WobbleProfile tempSpin = new WobbleProfile();
                Plugin.Log.Debug("Caching profile at " + sp);

                tempSpin.Wobbles = WobbleTools.LoadJson(sp);
                tempSpin.Name = sp.Substring((Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles") + "\\").Length);
                tempSpin.Name = tempSpin.Name.Substring(0, tempSpin.Name.IndexOf(".json"));
                tempSpin.JsonPath = sp;

                spinProfiles.Add(tempSpin);
            }
            return spinProfiles;
        }
        public static void SaveWobbleProfile(WobbleProfile profile)
        {
            WobbleTools.SaveJson(profile.Wobbles, profile.JsonPath);
        }
    }
}
