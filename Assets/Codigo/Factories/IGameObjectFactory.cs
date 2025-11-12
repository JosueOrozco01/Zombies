using UnityEngine;

namespace GameplayAdaptado.Legacy
{
    // Abstract Factory: Define una familia de fábricas relacionadas
    public interface IGameObjectFactory
    {
        IExplosionFactory ExplosionFactory { get; }
        IParticulaFactory ParticulaFactory { get; }
        IGranadaFactory GranadaFactory { get; }
    }
}
