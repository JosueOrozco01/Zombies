using UnityEngine;

namespace GameplayAdaptado
{
    // Concrete Factory para efectos de sonido específicos del juego
    public class GameSoundEffectFactory : ISoundEffectFactory
    {
        private readonly AudioClip impactClip;
        private readonly AudioClip explosionClip;
        private readonly AudioClip ambientClip;
        private readonly Transform audioRoot;

        public GameSoundEffectFactory(AudioClip impactClip, AudioClip explosionClip, AudioClip ambientClip, Transform audioRoot)
        {
            this.impactClip = impactClip;
            this.explosionClip = explosionClip;
            this.ambientClip = ambientClip;
            this.audioRoot = audioRoot;
        }

        public IAudioClip CreateImpactSound()
        {
            return new GameAudioClip(impactClip, audioRoot);
        }

        public IAudioClip CreateExplosionSound()
        {
            return new GameAudioClip(explosionClip, audioRoot);
        }

        public IAudioClip CreateAmbientSound()
        {
            return new GameAudioClip(ambientClip, audioRoot);
        }
    }

    // Concrete Product: Implementación específica para clips de audio del juego
    public class GameAudioClip : IAudioClip
    {
        private readonly AudioClip clip;
        private readonly Transform root;
        private AudioSource currentSource;

        public GameAudioClip(AudioClip clip, Transform root)
        {
            this.clip = clip;
            this.root = root;
        }

        public float Duration => clip != null ? clip.length : 0f;

        public bool IsPlaying => currentSource != null && currentSource.isPlaying;

        public void Play(Vector3 position, float volume = 1f)
        {
            if (clip == null) return;

            // Crear un GameObject temporal para el sonido
            GameObject tempGO = new GameObject($"SFX_{clip.name}");
            tempGO.transform.position = position;
            tempGO.transform.SetParent(root);

            currentSource = tempGO.AddComponent<AudioSource>();
            currentSource.clip = clip;
            currentSource.volume = volume;
            currentSource.spatialBlend = 1f; // Audio 3D completo
            currentSource.rolloffMode = AudioRolloffMode.Linear;
            currentSource.maxDistance = 30f;
            currentSource.Play();

            // Destruir el GameObject cuando termine el clip
            Object.Destroy(tempGO, clip.length);
        }

        public void Stop()
        {
            if (currentSource != null)
            {
                currentSource.Stop();
                Object.Destroy(currentSource.gameObject);
                currentSource = null;
            }
        }
    }

    // Ejemplo de uso con partículas
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleAudioEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip impactSound;
        [SerializeField] private AudioClip loopSound;
        [SerializeField] private Transform audioRoot;

        private ISoundEffectFactory soundFactory;
        private IAudioClip currentLoopClip;
        private ParticleSystem particleSystem;

        private void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
            InitializeSoundFactory();
        }

        private void InitializeSoundFactory()
        {
            soundFactory = new GameSoundEffectFactory(impactSound, null, loopSound, audioRoot);
        }

        private void OnParticleCollision(GameObject other)
        {
            var impactClip = soundFactory.CreateImpactSound();
            impactClip.Play(transform.position);
        }

        private void OnEnable()
        {
            if (particleSystem.isPlaying)
            {
                currentLoopClip = soundFactory.CreateAmbientSound();
                currentLoopClip.Play(transform.position);
            }
        }

        private void OnDisable()
        {
            currentLoopClip?.Stop();
        }
    }
}