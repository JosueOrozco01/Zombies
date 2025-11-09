using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonBack : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena del menú principal")]
    public string nombreEscenaMenuPrincipal = "MenuPrincipal";

    public void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene(nombreEscenaMenuPrincipal);
    }
}
