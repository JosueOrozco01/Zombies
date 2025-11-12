using System.Collections.Generic;
using UnityEngine;

namespace GameplayAdaptado
{
    // Simple, non-Mono fallback audio service. Implements the canonical IAudioService
    // defined in Contratos y abstracciones. Methods are minimal to avoid side-effects.
    public class SimpleAudioService : IAudioService
    {
        private readonly Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

        public void RegisterAudio(string key, AudioSource source)
        {
            if (source != null && !audioSources.ContainsKey(key))
                audioSources[key] = source;
        }

        public void RegisterFromGameObject(GameObject obj, System.Collections.Generic.Dictionary<string, string> keyPaths)
        {
            if (obj == null || keyPaths == null) return;
            foreach (var kvp in keyPaths)
            {
                var child = obj.transform.Find(kvp.Value);
                if (child == null) continue;
                var source = child.GetComponent<AudioSource>();
                if (source != null) RegisterAudio(kvp.Key, source);
            }
        }

        public float GetVolume(string key)
        {
            if (audioSources.TryGetValue(key, out var s)) return s.volume;
            return PlayerPrefs.GetFloat(key, 1f);
        }

        public void SetVolume(string key, float volume)
        {
            PlayerPrefs.SetFloat(key, volume);
            if (audioSources.TryGetValue(key, out var s)) s.volume = Mathf.Clamp01(volume);
        }

        public void PlayOneShot(string key, float volumeScale = 1f)
        {
            if (audioSources.TryGetValue(key, out var s)) s.PlayOneShot(s.clip, volumeScale);
        }

        public void Play(string key)
        {
            if (audioSources.TryGetValue(key, out var s)) s.Play();
        }

        public void Stop(string key)
        {
            if (audioSources.TryGetValue(key, out var s)) s.Stop();
        }
    }
}
