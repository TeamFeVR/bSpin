using System.Collections.Generic;
using System.IO;
using bSpin.CustomTypes;
using IPA.Utilities;
using Newtonsoft.Json;
using SpinMod;
using UnityEngine;

namespace bSpin.CustomTypes {
    public struct SpinProfile {
        public List<Spin> spins;
        public string name;
        public string jsonPath;
    }

    public struct Spin {
        public float DelayBeforeSpin;
        public float Length;

        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 Begin;

        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 End;

        public float DelayAfterSpin;

        [JsonConverter(typeof(EasingJsonConverter))]
        public EasingFunction.Ease Easing;

        public Spin(float delayBeforeSpin, float length, Vector3 start, Vector3 end, float delayAfterSpin,
            EasingFunction.Ease easing = EasingFunction.Ease.Linear) {
            DelayBeforeSpin = delayBeforeSpin;
            Length = length;
            Begin = start;
            End = end;
            DelayAfterSpin = delayAfterSpin;
            Easing = easing;
        }
    }

    public struct WobbleProfile {
        public List<Wobble> Wobbles;
        public string Name;
        public string JsonPath;

        public WobbleProfile(string name, List<Wobble> wobbles) {
            Name = name;
            Wobbles = wobbles;
            JsonPath = Path.Combine(UnityGame.UserDataPath, "bSpin", "Wobbles", name + ".json");
        }
    }

    public struct Wobble {
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

        public Wobble(float length, Vector3 rstart, Vector3 rend, Vector3 pstart, Vector3 pend,
            EasingFunction.Ease easing = EasingFunction.Ease.Linear) {
            Length = length;
            BeginRot = rstart;
            BeginPos = pstart;
            EndRot = rend;
            EndPos = pend;
            Easing = easing;
        }
    }
}

namespace bSpin {
    public class SpinTools {
        public static List<Spin> LoadJson(string path) {
            using (var r = new StreamReader(path)) {
                var json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<Spin>>(json);
                r.Close();
                return items;
            }
        }

        public static void SaveJson(List<Spin> sList, string path) {
            using (var w = new StreamWriter(path)) {
                var serializer = new JsonSerializer();
                var contents = JsonConvert.SerializeObject(sList);
                var jtw = new JsonTextWriter(w);


                jtw.Formatting = Formatting.Indented;
                serializer.Serialize(jtw, sList);
                w.Close();
            }
        }
    }

    public class WobbleTools {
        public static List<Wobble> LoadJson(string path) {
            using (var r = new StreamReader(path)) {
                var json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<Wobble>>(json);
                r.Close();
                return items;
            }
        }

        public static void SaveJson(List<Wobble> sList, string path) {
            using (var w = new StreamWriter(path)) {
                var serializer = new JsonSerializer();
                var contents = JsonConvert.SerializeObject(sList);
                var jtw = new JsonTextWriter(w);


                jtw.Formatting = Formatting.Indented;
                serializer.Serialize(jtw, sList);
                w.Close();
            }
        }
    }
}