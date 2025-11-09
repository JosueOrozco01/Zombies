using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles : MonoBehaviour
{
    // Botón Nivel 1
    public void AbrirNivel1()
    {
        infoPartidaGuardada.hayPartidaGuardada = false; // ❌ No cargar partida previa
        SceneManager.LoadScene("Scenes/Escena"); 
    }

    // Botón Nivel 2
    public void AbrirNivel2()
    {
        infoPartidaGuardada.hayPartidaGuardada = false; // ❌ No cargar partida previa
        SceneManager.LoadScene("Scenes/Escena 1"); 
    }

    // Botón Nivel 3
    public void AbrirNivel3()
    {
        infoPartidaGuardada.hayPartidaGuardada = false; // ❌ No cargar partida previa
        SceneManager.LoadScene("Scenes/Escena 2"); 
    }

    // Botón Back (regresar al menú principal)
    public void RegresarMenuPrincipal()
    {
        SceneManager.LoadScene("Scenes/MenuPrincipal");
    }
}
