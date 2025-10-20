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

    public void muere(Vector3 direccion)
    {
        GameObject instMuerto = Instantiate(prefabMuerto, transform.position, transform.rotation);

        instMuerto.transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(direccion * magnitudVueloCabeza, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(1).GetComponent<Rigidbody2D>().AddForce(direccion * magnitudVueloCabeza/2, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(0).GetComponent<Rigidbody2D>().AddTorque(10f, ForceMode2D.Impulse);
        instMuerto.transform.GetChild(1).GetComponent<Rigidbody2D>().AddTorque(10f, ForceMode2D.Impulse);
        Destroy(gameObject);
    }
}
    