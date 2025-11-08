using UnityEngine;

public interface IEntidad
{
    bool EstaViva { get; }
    void RecibirDaño(int cantidad, Vector2 posicionImpacto);
    void Morir(Vector3 direccion);
}
