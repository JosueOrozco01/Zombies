using UnityEngine;

namespace GameplayAdaptado.Factories
{
    public interface IGranadaFactory
    {
        GameObject CrearGranada(Vector3 posicion, Quaternion rotacion);
    }
}
