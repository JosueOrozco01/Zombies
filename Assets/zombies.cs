using UnityEngine;

public class Zombies : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D circle;
    float limiteCaminataIzq;
    float limiteCaminataDer;
    
    public float velCaminata = 10f;
    float direccion = 1f;
    public float umbralVelicidad;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();

        // Ajustar los límites según el radio y la escala del personaje
        float radioEscalado = circle.radius * transform.localScale.x;
        float centro = transform.position.x;

        limiteCaminataIzq = centro - radioEscalado;
        limiteCaminataDer = centro + radioEscalado;
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < umbralVelicidad)
        {
            // Movimiento del zombie
            rb.linearVelocity = new Vector2(velCaminata * direccion, rb.linearVelocity.y);

            // Cambio de dirección al alcanzar los límites
            if (transform.position.x <= limiteCaminataIzq)
                direccion = 1f;
            else if (transform.position.x >= limiteCaminataDer)
                direccion = -1f;

            // Ajusta la escala para girar el sprite
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * Mathf.Sign(direccion);
            transform.localScale = escala;
        }
    }

    public void muere()
    {
        Destroy(gameObject);
    }
}
    