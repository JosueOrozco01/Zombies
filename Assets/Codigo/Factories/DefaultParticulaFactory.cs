using System.Collections.Generic;
using UnityEngine;

namespace GameplayAdaptado.Factories
{
    public class DefaultParticulaFactory : IParticulaFactory
    {
        private readonly GameObject _prefab;
        private readonly List<GameObject> _active = new List<GameObject>();

        public DefaultParticulaFactory(GameObject prefab)
        {
            _prefab = prefab;
        }

        public GameObject EmitirParticula(Vector3 posicion, Quaternion rotacion, float duracion = 2f)
        {
            if (_prefab == null) return null;
            var go = Object.Instantiate(_prefab, posicion, rotacion);
            _active.Add(go);
            Object.Destroy(go, duracion);
            return go;
        }
    }
}
