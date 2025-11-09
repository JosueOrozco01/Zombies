using UnityEngine;
using System.Collections.Generic;

namespace GameplayAdaptado
{
    // Single Responsibility: Cada fábrica concreta maneja un solo tipo de objeto
    public class DefaultExplosionFactory : IExplosionFactory
    {
        private readonly GameObject prefab;

        public DefaultExplosionFactory(GameObject prefab)
        {
            this.prefab = prefab;
        }

        public GameObject CrearExplosion(Vector3 posicion, Quaternion rotacion)
        {
            return Object.Instantiate(prefab, posicion, rotacion);
        }
    }

    public class DefaultParticulaFactory : IParticulaFactory
    {
        private readonly GameObject prefab;
        private readonly Dictionary<GameObject, Coroutine> activeParticles = new Dictionary<GameObject, Coroutine>();

        public DefaultParticulaFactory(GameObject prefab)
        {
            this.prefab = prefab;
        }

        public GameObject EmitirParticula(Vector3 posicion, Quaternion rotacion, float duracion)
        {
            var particula = Object.Instantiate(prefab, posicion, rotacion);
            if (duracion > 0)
            {
                var routine = CoroutineRunner.Run(DestroyAfterDelay(particula, duracion));
                activeParticles[particula] = routine;
            }
            return particula;
        }

        private System.Collections.IEnumerator DestroyAfterDelay(GameObject particula, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (activeParticles.TryGetValue(particula, out var routine))
            {
                activeParticles.Remove(particula);
                CoroutineRunner.Stop(routine);
                Object.Destroy(particula);
            }
        }
    }

    public class DefaultGranadaFactory : IGranadaFactory
    {
        private readonly GameObject prefab;

        public DefaultGranadaFactory(GameObject prefab)
        {
            this.prefab = prefab;
        }

        public GameObject CrearGranada(Vector3 posicion, Quaternion rotacion)
        {
            return Object.Instantiate(prefab, posicion, rotacion);
        }
    }

    // Abstract Factory implementación concreta
    // Open/Closed: Podemos extender creando nuevas implementaciones de IGameObjectFactory
    public class DefaultGameObjectFactory : IGameObjectFactory
    {
        public IExplosionFactory ExplosionFactory { get; }
        public IParticulaFactory ParticulaFactory { get; }
        public IGranadaFactory GranadaFactory { get; }

        public DefaultGameObjectFactory(GameObject explosionPrefab, GameObject particulaPrefab, GameObject granadaPrefab)
        {
            ExplosionFactory = new DefaultExplosionFactory(explosionPrefab);
            ParticulaFactory = new DefaultParticulaFactory(particulaPrefab);
            GranadaFactory = new DefaultGranadaFactory(granadaPrefab);
        }
    }
}