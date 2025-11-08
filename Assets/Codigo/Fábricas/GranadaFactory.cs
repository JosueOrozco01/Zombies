using UnityEngine;

namespace GameplayAdaptado
{
    public static class GranadaFactory
    {
        // Simple factory: instantiate prefab at position/rotation and return instance
        public static GameObject Crear(GameObject prefab, Vector3 posicion, Quaternion rotacion)
        {
            if (prefab == null) return null;
            GameObject go = Object.Instantiate(prefab, posicion, rotacion);
            return go;
        }
    }
}
