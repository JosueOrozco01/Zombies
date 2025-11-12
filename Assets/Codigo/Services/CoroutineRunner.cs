using System.Collections;
using UnityEngine;

namespace GameplayAdaptado.Services
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        private static void EnsureInstance()
        {
            if (_instance != null) return;
            var go = new GameObject("__CoroutineRunner");
            _instance = go.AddComponent<CoroutineRunner>();
            Object.DontDestroyOnLoad(go);
        }

        public static Coroutine Run(IEnumerator routine)
        {
            EnsureInstance();
            return _instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine routine)
        {
            if (_instance == null || routine == null) return;
            _instance.StopCoroutine(routine);
        }
    }
}
