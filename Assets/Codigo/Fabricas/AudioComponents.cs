using UnityEngine;

namespace GameplayAdaptado
{
    // Factory Method: Productos concretos para la familia de audio
    internal class DefaultAudioPlayer : IAudioPlayer
    {
        private readonly AudioSourceManager sourceManager;

        public DefaultAudioPlayer(AudioSourceManager sourceManager)
        {
            this.sourceManager = sourceManager;
        }

        public void Play(string key)
        {
            var source = sourceManager.GetComponent<AudioSource>();
            source?.Play();
        }

        public void PlayOneShot(string key, float volumeScale = 1f)
        {
            var source = sourceManager.GetComponent<AudioSource>();
            if (source?.clip != null)
            {
                source.PlayOneShot(source.clip, volumeScale);
            }
        }

        public void Stop(string key)
        {
            var source = sourceManager.GetComponent<AudioSource>();
            source?.Stop();
        }
    }

    internal class DefaultVolumeControl : IAudioVolumeControl
    {
        private readonly AudioSourceManager sourceManager;
        private readonly AudioSettingsRepository settingsRepository;

        public DefaultVolumeControl(AudioSourceManager sourceManager, AudioSettingsRepository settingsRepository)
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
            volume = Mathf.Clamp01(volume);
            settingsRepository.SaveVolume(key, volume);

            var source = sourceManager.GetComponent<AudioSource>();
            if (source != null)
            {
                source.volume = volume;
            }
        }
    }

    internal class DefaultAudioRegistry : IAudioRegistry
    {
        private readonly AudioSourceManager sourceManager;

        public DefaultAudioRegistry(AudioSourceManager sourceManager)
        {
            this.sourceManager = sourceManager;
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