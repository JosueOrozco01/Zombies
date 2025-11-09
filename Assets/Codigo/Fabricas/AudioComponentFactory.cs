using UnityEngine;

namespace GameplayAdaptado
{
    // Abstract Factory: Define una familia de productos relacionados con audio
    public interface IAudioComponentFactory
    {
        IAudioPlayer CreateAudioPlayer();
        IAudioVolumeControl CreateVolumeControl();
        IAudioRegistry CreateAudioRegistry();
    }

    // Concrete Factory para crear componentes de audio básicos
    public class DefaultAudioComponentFactory : IAudioComponentFactory
    {
        private readonly AudioSourceManager sourceManager;
        private readonly AudioSettingsRepository settingsRepository;

        public DefaultAudioComponentFactory(AudioSourceManager sourceManager, AudioSettingsRepository settingsRepository)
        {
            this.sourceManager = sourceManager;
            this.settingsRepository = settingsRepository;
        }

        public IAudioPlayer CreateAudioPlayer()
        {
            return new DefaultAudioPlayer(sourceManager);
        }

        public IAudioVolumeControl CreateVolumeControl()
        {
            return new DefaultVolumeControl(sourceManager, settingsRepository);
        }

        public IAudioRegistry CreateAudioRegistry()
        {
            return new DefaultAudioRegistry(sourceManager);
        }
    }
}