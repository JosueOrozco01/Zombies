using UnityEngine;

namespace GameplayAdaptado.Entities
{
    public interface IEntidad
    {
        void RecibirDaño(int cantidad, Vector2 impacto);
        void Morir(Vector3 direccion);
    }
}
