using UnityEngine;
using System.Collections;

namespace GameplayAdaptado
{
    // Single Responsibility: Maneja las corrutinas para otras clases
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner instance;

        private static CoroutineRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("CoroutineRunner");
                    instance = go.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        public static Coroutine Run(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine routine)
        {
            if (routine != null)
                Instance.StopCoroutine(routine);
        }
    }
}