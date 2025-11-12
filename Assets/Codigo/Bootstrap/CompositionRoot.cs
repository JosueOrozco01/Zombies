using GameplayAdaptado.Factories;
using GameplayAdaptado.Services;
using UnityEngine;

namespace GameplayAdaptado.Bootstrap
{
    // Composition root: implements the composition contract and wires the system.
    public class CompositionRoot : MonoBehaviour, ICompositionRoot
    {
        [Header("Prefabs para factories")]
        public GameObject explosionPrefab;
        public GameObject particulaPrefab;
        public GameObject granadaPrefab;

    public GameplayAdaptado.IGameObjectFactory GameObjectFactory { get; private set; }
    public GameplayAdaptado.IAudioService AudioService { get; private set; }

        void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            GameObjectFactory = new DefaultGameObjectFactory(explosionPrefab, particulaPrefab, granadaPrefab);

            if (GameplayAdaptado.AudioService.instance != null)
                AudioService = GameplayAdaptado.AudioService.instance;
            else
                AudioService = new GameplayAdaptado.SimpleAudioService();

            // Inject into refactored components
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
