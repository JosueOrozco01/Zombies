namespace GameplayAdaptado
{
    // Interface Segregation Principle: Separamos las responsabilidades en interfaces pequeñas y específicas
    public interface IAudioPlayer
    {
        void Play(string key);
        void PlayOneShot(string key, float volumeScale = 1f);
        void Stop(string key);
    }

    public interface IAudioVolumeControl
    {
        float GetVolume(string key);
        void SetVolume(string key, float volume);
    }

    public interface IAudioRegistry
    {
        void RegisterAudio(string key, UnityEngine.AudioSource source);
        void RegisterFromGameObject(UnityEngine.GameObject obj, System.Collections.Generic.Dictionary<string, string> keyPaths);
    }

    // Composite interface que une todas las capacidades
    // Open/Closed Principle: Podemos extender añadiendo nuevas interfaces sin modificar las existentes
    public interface IAudioService : IAudioPlayer, IAudioVolumeControl, IAudioRegistry
    {
    }
}