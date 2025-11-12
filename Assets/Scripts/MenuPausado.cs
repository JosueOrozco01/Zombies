using UnityEngine;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public GameObject fondoPausa;
    public GameObject botonPausa;
    public GameObject botonReanudar;

    private bool enInspeccion = false;
    private bool juegoPausado = false;

    // 🔒 Límites del movimiento de la cámara de inspección
    public Vector2 limiteMin = new Vector2(-10f, -5f);
    public Vector2 limiteMax = new Vector2(10f, 5f);

    void Start()
    {
        fondoPausa.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;

    }

    public void Pausar()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        juegoPausado = true;
        enInspeccion = false;

        fondoPausa.SetActive(true);

    }

    public void Reanudar()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        juegoPausado = false;
        enInspeccion = false;

        fondoPausa.SetActive(false);

        // 🔄 Reactivar scripts del jugador
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            var scripts = jugador.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
            {
                s.enabled = true;
            }
        }
    }

    public void Inspeccionar()
    {
        // Pausamos todo el juego
        Time.timeScale = 0;
        AudioListener.pause = true;
        enInspeccion = true;
        juegoPausado = false;

        fondoPausa.SetActive(false);


        // Marcar que estamos en modo inspección
        enInspeccion = true;

        // 🔒 Desactivar scripts del jugador para que no se mueva
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            var scripts = jugador.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
            {
                if (s != this) s.enabled = false;
            }
        }
    }
}