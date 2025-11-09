using UnityEngine;

namespace GameplayAdaptado
{
    // Interface Segregation: Interfaces específicas para cada tipo de fábrica
    public interface IExplosionFactory
    {
        GameObject CrearExplosion(Vector3 posicion, Quaternion rotacion);
    }

    public interface IParticulaFactory
    {
        GameObject EmitirParticula(Vector3 posicion, Quaternion rotacion, float duracion);
    }

    public interface IGranadaFactory
    {
        GameObject CrearGranada(Vector3 posicion, Quaternion rotacion);
    }

    // Abstract Factory: Define una familia de fábricas relacionadas
    public interface IGameObjectFactory
    {
        IExplosionFactory ExplosionFactory { get; }
        IParticulaFactory ParticulaFactory { get; }
        IGranadaFactory GranadaFactory { get; }
    }
}