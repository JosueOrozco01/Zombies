using GameplayAdaptado;
using UnityEngine;

namespace GameplayAdaptado.Bootstrap
{
    // Single point to wire dependencies. Attach to a GameObject in the first scene.
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Prefabs para factories")]
        public GameObject explosionPrefab;
        public GameObject particulaPrefab;
        public GameObject granadaPrefab;

    // Lightweight bootstrapper that can be used to manually trigger wiring.
    // The actual composition root implementation is provided by `CompositionRoot` which
    // implements `ICompositionRoot`.
    public GameplayAdaptado.IGameObjectFactory GameObjectFactory { get; private set; }
    public GameplayAdaptado.IAudioService AudioService { get; private set; }

        void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            // Create concrete factories (use fully-qualified to avoid duplicate-type ambiguity)
            GameObjectFactory = new GameplayAdaptado.DefaultGameObjectFactory(explosionPrefab, particulaPrefab, granadaPrefab);

            // Create services
            // Prefer an existing MonoBehaviour audio service (rich features); otherwise fall back
            // to the lightweight non-Mono implementation.
            if (GameplayAdaptado.AudioService.instance != null)
            {
                AudioService = GameplayAdaptado.AudioService.instance;
            }
            else
            {
                AudioService = new GameplayAdaptado.SimpleAudioService();
            }

            // Find existing entities that expect injection and assign
            var personajes = UnityEngine.Object.FindObjectsByType<GameplayAdaptado.Entities.PersonajeRefactor>(UnityEngine.FindObjectsSortMode.None);
            foreach (var personaje in personajes)
            {
                personaje.factories = GameObjectFactory;
                personaje.audioService = AudioService;
            }

            var granadas = UnityEngine.Object.FindObjectsByType<GameplayAdaptado.Weapons.GranadaRefactor>(UnityEngine.FindObjectsSortMode.None);
            foreach (var g in granadas)
            {
                g.factories = GameObjectFactory;
            }

            var zombies = UnityEngine.Object.FindObjectsByType<GameplayAdaptado.Entities.ZombiesRefactor>(UnityEngine.FindObjectsSortMode.None);
            foreach (var z in zombies)
            {
                z.factories = GameObjectFactory;
            }
        }
    }
}
