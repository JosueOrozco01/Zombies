using System.Collections.Generic;
using UnityEngine;

namespace GameplayAdaptado
{
    // Single Responsibility Principle: Esta clase solo maneja el acceso a AudioSources
    public class AudioSourceManager : AudioSourceProviderBase
    {
        private readonly Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

        public void RegisterAudio(string key, AudioSource source)
        {
            if (source != null && !audioSources.ContainsKey(key))
            {
                audioSources[key] = source;
            }
        }

        public void RegisterFromGameObject(GameObject obj, Dictionary<string, string> keyPaths)
        {
            if (obj == null) return;

            foreach (var kvp in keyPaths)
            {
                var child = obj.transform.Find(kvp.Value);
                if (child != null)
                {
                    var source = child.GetComponent<AudioSource>();
                    if (source != null)
                    {
                        RegisterAudio(kvp.Key, source);
                    }
                }
            }
        }

        protected override AudioSource GetAudioSourceForKey(string key)
        {
            audioSources.TryGetValue(key, out var source);
            return source;
        }
    }
}