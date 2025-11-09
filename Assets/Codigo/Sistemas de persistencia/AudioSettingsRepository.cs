using System;
using UnityEngine;

namespace GameplayAdaptado
{
    // Single Responsibility Principle: Esta clase solo maneja la persistencia de configuraciones de audio
    public class AudioSettingsRepository : MonoBehaviour
    {
        private const string VolumeKeyPrefix = "AudioVolume_";

        public float LoadVolume(string key, float defaultValue = 1f)
        {
            return PlayerPrefs.GetFloat(VolumeKeyPrefix + key, defaultValue);
        }

        public void SaveVolume(string key, float value)
        {
            PlayerPrefs.SetFloat(VolumeKeyPrefix + key, value);
            PlayerPrefs.Save();
        }
    }
}