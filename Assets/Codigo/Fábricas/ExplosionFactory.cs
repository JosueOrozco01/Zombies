using UnityEngine;

public static class ExplosionFactory
{
    public static GameObject Crear(GameObject prefab, Vector3 posicion, Quaternion rotacion)
    {
        return Object.Instantiate(prefab, posicion, rotacion);
    }
}
