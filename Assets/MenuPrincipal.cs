using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Botón Play -> carga el primer nivel (por nombre)
    public void Jugar()
    {
        SceneManager.LoadScene("Scenes/Escena");
    }

    // Botón Niveles -> abrir pantalla de selección de niveles
    public void AbrirNiveles()
    {
        SceneManager.LoadScene("Scenes/MenuLevels");
    }

    // Botón Settings -> abrir escena de ajustes
    public void AbrirSettings()
    {
        SceneManager.LoadScene("Scenes/Settings");
    }

    // Botón Exit -> salir del juego (en editor para pruebas solo detiene Play)
    public void Salir()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
