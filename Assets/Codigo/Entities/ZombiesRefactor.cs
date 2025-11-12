using GameplayAdaptado.Entities;
using GameplayAdaptado.Factories;
using UnityEngine;

namespace GameplayAdaptado.Entities
{
    public class ZombiesRefactor : EntidadBase
    {
    [Header("Dependencies")]
    public GameplayAdaptado.IGameObjectFactory factories;

        [Header("Parameters")]
        public float velCaminata = 10f;
        public GameObject prefabMuerto;
        public float magnitudVueloCabeza = 300f;
        public Transform Shaggy;

        public bool vivo = true;

        Rigidbody2D rb;
        Animator anim;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        public void Muere(Vector3 direccion)
        {
            if (!vivo) return;
            vivo = false;
            if (prefabMuerto != null)
            {
                GameObject inst = Instantiate(prefabMuerto, transform.position, transform.rotation);
                var headRb = inst.transform.GetChild(0).GetComponent<Rigidbody2D>();
                if (headRb != null) headRb.AddForce(direccion * magnitudVueloCabeza, ForceMode2D.Impulse);
            }

            // Disable visuals and audio on the original
            var sr = GetComponent<SpriteRenderer>(); if (sr != null) sr.enabled = false;
            var aud = GetComponent<AudioSource>(); if (aud != null) aud.enabled = false;
        }
    }
}
