using UnityEngine;

namespace GameplayAdaptado
{
    // Dependency Inversion: Esta clase depende de abstracciones, no implementaciones concretas
    public class GameFactoryProvider : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject particulaPrefab;
        [SerializeField] private GameObject granadaPrefab;

        public static IGameObjectFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = new DefaultGameObjectFactory(explosionPrefab, particulaPrefab, granadaPrefab);
            }
        }
    }
}