using UnityEngine;

namespace GameplayAdaptado.Factories
{
    public class DefaultGranadaFactory : IGranadaFactory
    {
        private readonly GameObject _prefab;

        public DefaultGranadaFactory(GameObject prefab)
        {
            _prefab = prefab;
        }

        public GameObject CrearGranada(Vector3 posicion, Quaternion rotacion)
        {
            if (_prefab == null) return null;
            return Object.Instantiate(_prefab, posicion, rotacion);
        }
    }
}
