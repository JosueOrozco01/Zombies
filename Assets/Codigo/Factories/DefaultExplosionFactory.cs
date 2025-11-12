using UnityEngine;

namespace GameplayAdaptado.Legacy
{
    public class DefaultExplosionFactory : IExplosionFactory
    {
        private readonly GameObject _prefab;

        public DefaultExplosionFactory(GameObject prefab)
        {
            _prefab = prefab;
        }

        public GameObject CrearExplosion(Vector3 posicion, Quaternion rotacion)
        {
            if (_prefab == null) return null;
            return Object.Instantiate(_prefab, posicion, rotacion);
        }
    }
}
