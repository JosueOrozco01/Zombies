using UnityEngine;
using UnityEngine.UI;

namespace GameplayAdaptado
{
    // Single Responsibility Principle: Esta clase solo maneja la UI de configuración de audio
    public class SettingsMenuAdaptado : MonoBehaviour
    {
        [Header("Controladores de Volumen")]
        [Tooltip("Slider para el volumen del disparo")]
        public Slider disparo;
        [Tooltip("Slider para el sonido cuando no hay balas")]
        public Slider sinBalas;
        [Tooltip("Slider para el sonido al recoger municiones")]
        public Slider municiones;
        [Tooltip("Slider para el sonido al alcanzar checkpoint")]
        public Slider checkpoint;
        [Tooltip("Slider para el sonido de salto")]
        public Slider salta;
        [Tooltip("Slider para el sonido de muerte del zombie")]
        public Slider zombieMuere;
        [Tooltip("Slider para el sonido de daño")]
        public Slider duele;
        [Tooltip("Slider para la música de fondo")]
        public Slider fondo;

        private IAudioVolumeControl audioControl;

        private void Start()
        {
            // Dependency Inversion: Dependemos de la abstracción IAudioVolumeControl
            audioControl = AudioService.instance as IAudioVolumeControl;
            if (audioControl == null)
            {
                Debug.LogWarning("IAudioVolumeControl no encontrado - los sliders no funcionarán.");
                return;
            }

            InitializeSlider(disparo, "disparo");
            InitializeSlider(sinBalas, "sinBalas");
            InitializeSlider(municiones, "municiones");
            InitializeSlider(checkpoint, "checkpoint");
            InitializeSlider(salta, "salta");
            InitializeSlider(zombieMuere, "zombieMuere");
            InitializeSlider(duele, "duele");
            InitializeSlider(fondo, "fondo");
        }

        private void InitializeSlider(Slider slider, string key)
        {
            if (slider == null) return;

            // Inicializar valor actual desde el servicio
            slider.value = audioControl.GetVolume(key);

            // Conectar el evento de cambio
            slider.onValueChanged.AddListener((float value) => 
            {
                audioControl.SetVolume(key, value);
            });
        }
    }
}