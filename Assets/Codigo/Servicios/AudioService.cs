using System.Collections.Generic;
using UnityEngine;

namespace GameplayAdaptado
{
    // Servicio de Audio adaptado: mapea nombres a AudioSource existentes y proporciona
    // una interfaz limpia para gestionar audio en el juego adaptado.
    // Implementamos la interfaz canónica GameplayAdaptado.IAudioService
    // para ser inyectable desde GameBootstrapper y compatible con el código refactorizado.
    public class AudioService : MonoBehaviour, GameplayAdaptado.IAudioService
    {
        public static AudioService instance;

        private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

        void Awake()
        {
            if (instance == null) instance = this;
            else { Destroy(gameObject); return; }

            DontDestroyOnLoad(gameObject);
        }

        // Registra un AudioSource con una clave única
        public void RegisterAudio(string key, AudioSource source)
        {
            if (source != null && !audioSources.ContainsKey(key))
            {
                audioSources[key] = source;
            }
        }

        // Registra múltiples AudioSources desde un GameObject (p.ej. el personaje)
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

        public float GetVolume(string key)
        {
            if (audioSources.TryGetValue(key, out var source))
            {
                return source.volume;
            }
            return 1f;
        }

        public void SetVolume(string key, float volume)
        {
            if (audioSources.TryGetValue(key, out var source))
            {
                source.volume = Mathf.Clamp01(volume);
            }
        }

        public void PlayOneShot(string key, float volumeScale = 1f)
        {
            if (audioSources.TryGetValue(key, out var source))
            {
                source.PlayOneShot(source.clip, volumeScale);
            }
        }

        public void Play(string key)
        {
            if (audioSources.TryGetValue(key, out var source))
            {
                source.Play();
            }
        }

        public void Stop(string key)
        {
            if (audioSources.TryGetValue(key, out var source))
            {
                source.Stop();
            }
        }
    }
}