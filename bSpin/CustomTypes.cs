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

    public struct WobbleProfile
    {
        public List<Wobble> Wobbles;
        public string Name;
        public string JsonPath;

        public WobbleProfile(string name, List<Wobble> wobbles)
        {
            this.Name = name;
            this.Wobbles = wobbles;
            this.JsonPath = Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles", name + ".json");
        }
    }

    public struct Wobble
    {
        public float Length;
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 BeginRot;
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 EndRot;
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 BeginPos;
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 EndPos;
        [JsonConverter(typeof(EasingJsonConverter))]
        public EasingFunction.Ease Easing;
        public Wobble(float length, Vector3 rstart, Vector3 rend, Vector3 pstart, Vector3 pend, EasingFunction.Ease easing = EasingFunction.Ease.Linear)
        {
            this.Length = length;
            this.BeginRot = rstart;
            this.BeginPos = pstart;
            this.EndRot = rend;
            this.EndPos = pend;
            this.Easing = easing;
        }
    }
    
}
namespace bSpin
{
    public class SpinTools
    {
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
    public class WobbleTools
    {
        public static List<CustomTypes.Wobble> LoadJson(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<CustomTypes.Wobble> items = JsonConvert.DeserializeObject<List<CustomTypes.Wobble>>(json);
                r.Close();
                return items;
            }
        }
        public static void SaveJson(List<CustomTypes.Wobble> sList, string path)
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
