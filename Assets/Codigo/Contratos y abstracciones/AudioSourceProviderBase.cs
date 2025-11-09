using UnityEngine;

namespace GameplayAdaptado
{
    // Open/Closed Principle: Esta clase base permite extensión pero no modificación
    public abstract class AudioSourceProviderBase : MonoBehaviour
    {
        protected abstract AudioSource GetAudioSourceForKey(string key);
    }
}