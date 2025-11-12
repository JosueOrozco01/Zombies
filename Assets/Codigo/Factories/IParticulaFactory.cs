using UnityEngine;

namespace GameplayAdaptado.Factories
{
    public interface IParticulaFactory
    {
        GameObject EmitirParticula(Vector3 posicion, Quaternion rotacion, float duracion = 2f);
    }
}
