using UnityEngine;

namespace GameplayAdaptado.Factories
{
    public interface IExplosionFactory
    {
        GameObject CrearExplosion(Vector3 posicion, Quaternion rotacion);
    }
}
namespace GameplayAdaptado.Legacy
{
    public interface IExplosionFactory
    {
        GameObject CrearExplosion(Vector3 posicion, Quaternion rotacion);
    }
}
