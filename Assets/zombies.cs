using System;
using Unity.VisualScripting;
using UnityEngine;

public class zombies : MonoBehaviour
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();

        // Ajustar los límites según el radio y la escala del personaje
        float radioEscalado = circle.radius * transform.localScale.x;
        float centro = transform.position.x;

        limiteCaminataIzq = centro - radioEscalado;
        limiteCaminataDer = centro + radioEscalado;

        anim = transform.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < umbralVelicidad)
            distanciaShaggy = Mathf.Abs(Shaggy.position.x - transform.position.x);
        {
            switch (comportamiento)
            {
                case tipoComportamientoZombie.pasivo:
                    {
                        // Movimiento del zombie
                        rb.linearVelocity = new Vector2(velCaminata * direccion, rb.linearVelocity.y);

                        // Cambio de dirección al alcanzar los límites
                        if (transform.position.x <= limiteCaminataIzq)
                            direccion = 1f;
                        else if (transform.position.x >= limiteCaminataDer)
                            direccion = -1f;

                        anim.speed = 1f;

                        //entrar en zona de persecucion 
                        if (distanciaShaggy < entradaZonaPersecucion) comportamiento = tipoComportamientoZombie.persecucion;
                    }
                    break;

                case tipoComportamientoZombie.persecucion:
                    {
                        // Movimiento del zombie (corriendo)
                        rb.linearVelocity = new Vector2(velCaminata * 1.5f * direccion, rb.linearVelocity.y);

                        // Cambio de dirección al alcanzar los límites
                        if (Shaggy.position.x > transform.position.x)
                            direccion = 1f;
                        else if (Shaggy.position.x < transform.position.x)
                            direccion = -1f;

                        // velocidad del animator
                        anim.speed = 1.5f;

                        // volver a la zona pasiva  
                        if (distanciaShaggy > salidaZonaPersecucion) comportamiento = tipoComportamientoZombie.pasivo;

                        // entrar en zona de ataque
                        if (distanciaShaggy < distaciaAtaque) comportamiento = tipoComportamientoZombie.ataque;
                    }
                    break;

                case tipoComportamientoZombie.ataque:
                    {
                        anim.SetTrigger("atacar");
                        
                        // Cambio de dirección segun la posicion de shaggy
                        if (Shaggy.position.x > transform.position.x)
                            direccion = 1f;
                        else if (Shaggy.position.x < transform.position.x)
                            direccion = -1f;

                        // velocidad del animator
                        anim.speed = 1f;

                        // entrar en zona de ataque
                        if (distanciaShaggy > distaciaAtaque)
                        {
                            comportamiento = tipoComportamientoZombie.persecucion;
                            anim.ResetTrigger("atacar");
                        }
                    }
                    break;
            }

            // Ajusta la escala para girar el sprite
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * Mathf.Sign(direccion);
            transform.localScale = escala;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && mordidaEsValida)
        {
            mordidaEsValida = false;
            Shaggy.GetComponent<personaje>().RecibirMordida();
        }
    }

    public void muere(Vector3 direccion)
    {
        GameObject instMuerto = Instantiate(prefabMuerto, transform.position, transform.rotation);

        instMuerto.transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(direccion * magnitudVueloCabeza, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(1).GetComponent<Rigidbody2D>().AddForce(direccion * magnitudVueloCabeza / 2, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(0).GetComponent<Rigidbody2D>().AddTorque(10f, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(1).GetComponent<Rigidbody2D>().AddTorque(10f, ForceMode2D.Impulse);
        Destroy(gameObject);
    }

    public void mordidaValida_inicio()
    {
        mordidaEsValida = true;
    }
    
    public void mordidaValida_fin()
    {
        mordidaEsValida = false;
    }
}
    