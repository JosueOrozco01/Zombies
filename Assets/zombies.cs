using UnityEngine;

public class Zombie1 : MonoBehaviour
{
    float limiteCaminataIzq;
    float limiteCaminataDer;
    Rigidbody2D rb;
    CircleCollider2D circle;
    public
    float velCaminata = 1f;
    float direccion = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();

         // El centro del movimiento será la posición inicial
        float centro = transform.position.x;

        // Calculamos los límites según el radio del collider
        limiteCaminataIzq = centro - circle.radius;
        limiteCaminataDer = centro + circle.radius;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(velCaminata * direccion, rb.linearVelocity.y);

        // Si llega al borde izquierdo o derecho del círculo, se da la vuelta
        if (transform.position.x <= limiteCaminataIzq)
            direccion = 1f;
        else if (transform.position.x >= limiteCaminataDer)
            direccion = -1f;

        // Girar sprite según la dirección
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direccion, transform.localScale.y, transform.localScale.z);
    }
}
