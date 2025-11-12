using UnityEngine;

namespace GameplayAdaptado.Legacy
{
    // Simple abstract-factory that composes concrete factories
    public class DefaultGameObjectFactory : IGameObjectFactory
    {
        public IExplosionFactory ExplosionFactory { get; private set; }
        public IParticulaFactory ParticulaFactory { get; private set; }
        public IGranadaFactory GranadaFactory { get; private set; }

        public DefaultGameObjectFactory(GameObject explosionPrefab, GameObject particulaPrefab, GameObject granadaPrefab)
        {
            // Instantiate local Legacy implementations (keeps Legacy folder self-contained)
            ExplosionFactory = new DefaultExplosionFactory(explosionPrefab);
            ParticulaFactory = new DefaultParticulaFactory(particulaPrefab);
            GranadaFactory = new DefaultGranadaFactory(granadaPrefab);
        }
    }
}
