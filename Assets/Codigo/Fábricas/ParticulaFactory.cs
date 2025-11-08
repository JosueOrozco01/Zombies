using UnityEngine;

public static class ParticulaFactory
{
    public static GameObject Emitir(GameObject prefab, Vector3 posicion, Quaternion rotacion, float autoDestroySeconds = 2f)
    {
        var go = Object.Instantiate(prefab, posicion, rotacion);
        if (autoDestroySeconds > 0) Object.Destroy(go, autoDestroySeconds);
        return go;
    }
}
