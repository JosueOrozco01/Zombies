using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameplayAdaptado
{
    public class zombies : EntidadBase
    {
    Rigidbody2D rb;
    CircleCollider2D circle;
    float limiteCaminataIzq;
    float limiteCaminataDer;

    public float velCaminata = 10f;
    float direccion = 1f;
    public float umbralVelicidad;
    public GameObject prefabMuerto;
    public float magnitudVueloCabeza = 300f;
    enum tipoComportamientoZombie { pasivo, persecucion, ataque }
    tipoComportamientoZombie comportamiento = tipoComportamientoZombie.pasivo;
    float entradaZonaPersecucion = 40f;
    float salidaZonaPersecucion = 80f;
    float distaciaAtaque = 6f;
    float distanciaShaggy;
    public Transform Shaggy;
    Animator anim;
    bool mordidaEsValida = false;

    public Transform refPiso;

    // Se mantiene la bandera, pero sincronizada con EntidadBase
    public bool vivo = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();

        float radioEscalado = circle.radius * transform.localScale.x;
        float centro = transform.position.x;

        limiteCaminataIzq = centro - radioEscalado;
        limiteCaminataDer = centro + radioEscalado;

        anim = transform.GetComponent<Animator>();
    }

    void Update()
    {
        // Sincroniza con EntidadBase
        vivo = EstaViva;
        if (!vivo) return;

        if (rb.linearVelocity.magnitude < umbralVelicidad)
        {
            bool enPiso = Physics2D.OverlapCircle(refPiso.position, 0.1f, 1 << 6);

            distanciaShaggy = Mathf.Abs(Shaggy.position.x - transform.position.x);
            bool pocaDistanciaVertical = Mathf.Abs(Shaggy.position.y - transform.position.y) < 5f;

            switch (comportamiento)
            {
                case tipoComportamientoZombie.pasivo:
                    rb.linearVelocity = new Vector2(velCaminata * direccion, rb.linearVelocity.y);

                    if (transform.position.x <= limiteCaminataIzq)
                        direccion = 1f;
                    else if (transform.position.x >= limiteCaminataDer)
                        direccion = -1f;

                    anim.speed = 1f;

                    if (distanciaShaggy < entradaZonaPersecucion && pocaDistanciaVertical)
                        comportamiento = tipoComportamientoZombie.persecucion;
                    break;

                case tipoComportamientoZombie.persecucion:
                    rb.linearVelocity = new Vector2(velCaminata * 1.5f * direccion, rb.linearVelocity.y);

                    if (Shaggy.position.x > transform.position.x)
                        direccion = 1f;
                    else if (Shaggy.position.x < transform.position.x)
                        direccion = -1f;

                    anim.speed = 1.5f;

                    if (distanciaShaggy > salidaZonaPersecucion || !pocaDistanciaVertical)
                        comportamiento = tipoComportamientoZombie.pasivo;

                    if (distanciaShaggy < distaciaAtaque)
                        comportamiento = tipoComportamientoZombie.ataque;

                    break;

                case tipoComportamientoZombie.ataque:
                    anim.SetTrigger("atacar");

                    if (Shaggy.position.x > transform.position.x)
                        direccion = 1f;
                    else if (Shaggy.position.x < transform.position.x)
                        direccion = -1f;

                    anim.speed = 1f;

                    if (distanciaShaggy > distaciaAtaque)
                    {
                        comportamiento = tipoComportamientoZombie.persecucion;
                        anim.ResetTrigger("atacar");
                    }

                    break;
            }

            if (!enPiso) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Mathf.Sign(direccion);
        transform.localScale = escala;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!EstaViva) return;

        if (collision.gameObject.CompareTag("Player") && mordidaEsValida)
        {
            mordidaEsValida = false;
            Shaggy.GetComponent<personaje>().RecibirMordida(collision.contacts[0].point);
        }
    }

    // Abstract Method - implementación de EntidadBase
    public override void RecibirDaño(int cantidad, Vector2 posicionImpacto)
    {
        // Si quisieras agregar HP, aquí se procesaría; por ahora tu zombie muere por disparo a cabeza.
        // Se deja para compatibilidad futura.
    }

    public override void Morir(Vector3 direccion)
    {
        if (!EstaViva) return;

        GameObject instMuerto = Instantiate(prefabMuerto, transform.position, transform.rotation);

        instMuerto.transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(direccion * magnitudVueloCabeza, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(1).GetComponent<Rigidbody2D>().AddForce(direccion * magnitudVueloCabeza / 2, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(0).GetComponent<Rigidbody2D>().AddTorque(10f, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(1).GetComponent<Rigidbody2D>().AddTorque(10f, ForceMode2D.Impulse);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<AudioSource>().enabled = false;
        vivo = false;
        EstaViva = false;
    }

    public void mordidaValida_inicio()
    {
        mordidaEsValida = true;
    }

        public void mordidaValida_fin()
        {
            mordidaEsValida = false;
        }

    // Compatibilidad con código que usa el método antiguo 'muere'
    public void muere(Vector3 direccion)
    {
        Morir(direccion);
    }

    
    }
}
