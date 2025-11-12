using UnityEngine;

namespace GameplayAdaptado.Entities
{
    public abstract class EntidadBase : MonoBehaviour, IEntidad
    {
        protected bool EstaViva = true;

        public virtual void RecibirDaño(int cantidad, Vector2 impacto)
        {
            // default behaviour can be overridden
        }

        public virtual void Morir(Vector3 direccion)
        {
            EstaViva = false;
        }
    }
}
