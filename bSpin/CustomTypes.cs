using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using IPA.Utilities;
using SpinMod;
//using SongCore;
namespace bSpin.CustomTypes
{
    public struct SpinProfile
    {
        public List<Spin> spins;
        public string name;
        public string jsonPath;
    }

    public struct Spin
    {
        public float DelayBeforeSpin;
        public float Length;
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 Begin;
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 End;
        public float DelayAfterSpin;
        [JsonConverter(typeof(EasingJsonConverter))]
        public EasingFunction.Ease Easing;
        public Spin(float delayBeforeSpin, float length, Vector3 start, Vector3 end, float delayAfterSpin, EasingFunction.Ease easing = EasingFunction.Ease.Linear)
        {
            this.DelayBeforeSpin = delayBeforeSpin;
            this.Length = length;
            this.Begin = start;
            this.End = end;
            this.DelayAfterSpin = delayAfterSpin;
            this.Easing = easing;
        }
    }
    
}
namespace bSpin
{
    public class Tools
    {
        public static bool IsModdedMap(IDifficultyBeatmap map)
        {
            /*try
            {
                return Collections.RetrieveDifficultyData(map)?
                    .additionalDifficultyData?
                    ._requirements?.Any(x => x == "Noodle Extensions") == true;
            }
            catch
            {
                return false;
            }*/
            return false;
        }


        public static List<CustomTypes.Spin> LoadJson(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<CustomTypes.Spin> items = JsonConvert.DeserializeObject<List<CustomTypes.Spin>>(json);
                r.Close();
                return items;
            }
        }
        public static void SaveJson(List<CustomTypes.Spin> sList, string path)
        {
            using (StreamWriter w = new StreamWriter(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                string contents = JsonConvert.SerializeObject(sList);
                var jtw = new JsonTextWriter(w);


                jtw.Formatting = Formatting.Indented;
                serializer.Serialize(jtw, sList);
                w.Close();
            }
        }
    }
}
