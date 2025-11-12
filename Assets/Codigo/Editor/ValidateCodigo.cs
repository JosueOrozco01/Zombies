using UnityEditor;
using UnityEngine;

public static class ValidateCodigo
{
    [MenuItem("Codigo/Validate Project Setup")]
    public static void RunValidation()
    {
    int bootstrappers = UnityEngine.Object.FindObjectsByType<GameplayAdaptado.Bootstrap.GameBootstrapper>(UnityEngine.FindObjectsSortMode.None).Length;
    int personajes = UnityEngine.Object.FindObjectsByType<GameplayAdaptado.Entities.PersonajeRefactor>(UnityEngine.FindObjectsSortMode.None).Length;
    int zombies = UnityEngine.Object.FindObjectsByType<GameplayAdaptado.Entities.ZombiesRefactor>(UnityEngine.FindObjectsSortMode.None).Length;
    int granadas = UnityEngine.Object.FindObjectsByType<GameplayAdaptado.Weapons.GranadaRefactor>(UnityEngine.FindObjectsSortMode.None).Length;

        Debug.Log($"GameBootstrapper instances: {bootstrappers}\nPersonajeRefactor: {personajes}\nZombiesRefactor: {zombies}\nGranadaRefactor: {granadas}");

        if (bootstrappers == 0)
            Debug.LogWarning("No GameBootstrapper found — add one to your initial scene at Assets/Codigo/Bootstrap/GameBootstrapper.cs");
        else
            Debug.Log("GameBootstrapper exists — check inspector to wire prefabs");
    }
}
