using GameplayAdaptado.Factories;
using GameplayAdaptado.Services;
using UnityEngine;

namespace GameplayAdaptado.Entities
{
    // A refactored, dependency-injectable version of the player logic.
    public class PersonajeRefactor : EntidadBase
    {
    [Header("Dependencies (set from GameBootstrapper)")]
    public GameplayAdaptado.IGameObjectFactory factories;
    public GameplayAdaptado.IAudioService audioService;

        [Header("Gameplay fields")]
        public int energiaMax = 5;
        public int energiaActual;

        private void Awake()
        {
            energiaActual = energiaMax;
        }

        public override void RecibirDaño(int cantidad, Vector2 impacto)
        {
            if (!EstaViva) return;
            energiaActual -= cantidad;
            if (energiaActual <= 0)
            {
                Morir(Vector3.zero);
            }
        }

        public override void Morir(Vector3 direccion)
        {
            base.Morir(direccion);
            // TODO: trigger animations, notify systems
        }
    }
}
