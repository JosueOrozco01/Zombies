using UnityEngine;

public static class SonidoFactory
{
    public static GameObject Emitir(GameObject prefab, Vector2 posicion, float duracion = 5f)
    {
        var go = Object.Instantiate(prefab, posicion, Quaternion.identity);
        Object.Destroy(go, duracion);
        return go;
    }
}
