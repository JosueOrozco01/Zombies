using UnityEngine;

public abstract class EntidadBase : MonoBehaviour, IEntidad
{
    public bool EstaViva { get; protected set; } = true;

    public abstract void RecibirDaño(int cantidad, Vector2 posicionImpacto);
    public abstract void Morir(Vector3 direccion);

    // Permite que código externo (p. ej. loaders) sincronice estado de vida sin modificar el setter protegido
    public void SetEstaViva(bool valor)
    {
        EstaViva = valor;
    }
}
