namespace GameplayAdaptado.Services
{
    public interface IAudioService
    {
        float GetVolume(string key);
        void SetVolume(string key, float value);
        void Play(string key);
        void Stop(string key);
    }
}
