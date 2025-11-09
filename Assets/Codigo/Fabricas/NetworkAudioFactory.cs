using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Threading.Tasks;

namespace GameplayAdaptado
{
    // Interfaz para cargar clips de audio de diferentes fuentes
    public interface IAudioClipLoader
    {
        Task<AudioClip> LoadClipAsync(string clipId);
    }

    // Factory Method: Define una familia de productos para efectos de sonido
    public interface ISoundEffectFactory
    {
        IAudioClip CreateImpactSound();
        IAudioClip CreateExplosionSound();
        IAudioClip CreateAmbientSound();
    }

    // Abstract Product: Define la interfaz para clips de audio
    public interface IAudioClip
    {
        void Play(Vector3 position, float volume = 1f);
        void Stop();
        bool IsPlaying { get; }
        float Duration { get; }
    }

    // Concrete Factory para cargar audio desde la red
    public class NetworkAudioComponentFactory : IAudioComponentFactory
    {
        private readonly AudioSourceManager sourceManager;
        private readonly AudioSettingsRepository settingsRepository;
        private readonly string baseUrl;

        public NetworkAudioComponentFactory(AudioSourceManager sourceManager, AudioSettingsRepository settingsRepository, string baseUrl)
        {
            this.sourceManager = sourceManager;
            this.settingsRepository = settingsRepository;
            this.baseUrl = baseUrl;
        }

        public IAudioPlayer CreateAudioPlayer()
        {
            return new NetworkAudioPlayer(sourceManager, baseUrl);
        }

        public IAudioVolumeControl CreateVolumeControl()
        {
            return new NetworkVolumeControl(sourceManager, settingsRepository);
        }

        public IAudioRegistry CreateAudioRegistry()
        {
            return new NetworkAudioRegistry(sourceManager, baseUrl);
        }
    }

    // Concrete Products para audio en red
    internal class NetworkAudioPlayer : IAudioPlayer
    {
        private readonly AudioSourceManager sourceManager;
        private readonly string baseUrl;
        private readonly MonoBehaviour coroutineRunner;

        public NetworkAudioPlayer(AudioSourceManager sourceManager, string baseUrl)
        {
            this.sourceManager = sourceManager;
            this.baseUrl = baseUrl;
            this.coroutineRunner = sourceManager;
        }

        public void Play(string key)
        {
            // Ejemplo: cargar clip desde URL y reproducir
            coroutineRunner.StartCoroutine(LoadAndPlayClip(key));
        }

        public void PlayOneShot(string key, float volumeScale = 1f)
        {
            coroutineRunner.StartCoroutine(LoadAndPlayOneShot(key, volumeScale));
        }

        public void Stop(string key)
        {
            var source = sourceManager.GetComponent<AudioSource>();
            source?.Stop();
        }

        private IEnumerator LoadAndPlayClip(string key)
        {
            string url = $"{baseUrl}/audio/{key}.wav";
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    var source = sourceManager.GetComponent<AudioSource>();
                    if (source != null && clip != null)
                    {
                        source.clip = clip;
                        source.Play();
                    }
                }
            }
        }

        private IEnumerator LoadAndPlayOneShot(string key, float volumeScale)
        {
            string url = $"{baseUrl}/audio/{key}.wav";
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    var source = sourceManager.GetComponent<AudioSource>();
                    if (source != null && clip != null)
                    {
                        source.PlayOneShot(clip, volumeScale);
                    }
                }
            }
        }
    }

    internal class NetworkVolumeControl : IAudioVolumeControl
    {
        private readonly AudioSourceManager sourceManager;
        private readonly AudioSettingsRepository settingsRepository;

        public NetworkVolumeControl(AudioSourceManager sourceManager, AudioSettingsRepository settingsRepository)
        {
            this.sourceManager = sourceManager;
            this.settingsRepository = settingsRepository;
        }

        public float GetVolume(string key)
        {
            return settingsRepository.LoadVolume(key);
        }

        public void SetVolume(string key, float volume)
        {
            settingsRepository.SaveVolume(key, volume);
            var source = sourceManager.GetComponent<AudioSource>();
            if (source != null)
            {
                source.volume = Mathf.Clamp01(volume);
            }
        }
    }

    internal class NetworkAudioRegistry : IAudioRegistry
    {
        private readonly AudioSourceManager sourceManager;
        private readonly string baseUrl;

        public NetworkAudioRegistry(AudioSourceManager sourceManager, string baseUrl)
        {
            this.sourceManager = sourceManager;
            this.baseUrl = baseUrl;
        }

        public void RegisterAudio(string key, AudioSource source)
        {
            sourceManager.RegisterAudio(key, source);
        }

        public void RegisterFromGameObject(GameObject obj, System.Collections.Generic.Dictionary<string, string> keyPaths)
        {
            sourceManager.RegisterFromGameObject(obj, keyPaths);
        }
    }
}