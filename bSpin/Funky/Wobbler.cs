using System.Collections;
using System.Collections.Generic;
using bSpin.CustomTypes;
using bSpin.Extentions;
using UnityEngine;

namespace bSpin.Funky {
    internal class Wobbler : MonoBehaviour {
        internal static GameObject Handle;
        internal static List<List<Wobble>> WobQueue = new List<List<Wobble>>();
        internal static Wobbler Instance;
        private readonly Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
        private IEnumerator currentWob;

        internal void Innit() {
            Instance.StartCoroutine(CoroutineCoordinator());
        }

        internal void Exit() {
            Instance.StopCoroutine(CoroutineCoordinator());
        }

        internal void Clear() {
            coroutineQueue.Clear();
        }

        internal void Stop() {
            Handle.transform.localPosition = Vector3.zero;
            Handle.transform.rotation = Quaternion.Euler(Vector3.zero);
            Handle.transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);
            if (currentWob != null) Instance.StopCoroutine(currentWob);
            if (coroutineQueue != null) Instance.StopCoroutine(CoroutineCoordinator());
        }

        internal void Waitaminute() {
            if (currentWob != null) Instance.StartCoroutine(currentWob);
            if (coroutineQueue != null) Instance.StartCoroutine(CoroutineCoordinator());
        }

        internal void Skip() {
            if (currentWob != null) Instance.StopCoroutine(currentWob);
            if (coroutineQueue != null) {
                Instance.StopCoroutine(CoroutineCoordinator());
                Instance.StartCoroutine(CoroutineCoordinator());
            }

            Handle.transform.localPosition = Vector3.zero;
            Handle.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        internal void Wob(string profile) {
            foreach (var wobb in Plugin.wobbles) {
                Plugin.Log.Info(wobb.Name);


                if (wobb.Name.ToLower().Equals(profile))
                    coroutineQueue.Enqueue(wob(wobb.Wobbles));
            }
        }

        private IEnumerator CoroutineCoordinator() {
            while (true) {
                while (coroutineQueue.Count > 0) {
                    currentWob = coroutineQueue.Dequeue();
                    yield return Instance.StartCoroutine(currentWob);
                }

                yield return null;
            }
        }

        private IEnumerator wob(List<Wobble> wobs) {
            foreach (var wob in wobs) yield return Handle.transform.Wobble(wob);
        }
    }
}