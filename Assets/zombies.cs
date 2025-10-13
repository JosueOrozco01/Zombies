using UnityEngine;

public class Zombie1 : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D circle;
    float limiteCaminataIzq;
    float limiteCaminataDer;
    
    public float velCaminata = 10f;
    float direccion = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(velCaminata * direccion, rb.linearVelocity.y);

    if (transform.position.x <= limiteCaminataIzq)
        direccion = 1f;
    else if (transform.position.x >= limiteCaminataDer)
        direccion = -1f;

    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direccion, transform.localScale.y, transform.localScale.z);
    }
}
