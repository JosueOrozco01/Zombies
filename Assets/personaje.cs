using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class personaje : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    public float fuerzaSalto;
    public bool enPiso;
    public Transform refPie;
    public float velX = 10f;
    public Transform contArma;
    bool tieneArma;
    public Transform mira;
    public Transform refManoArma;
    public Transform refOjos;
    public Transform refCabeza;
    public float magnitudPateoArma = 300f;
    public Transform refPuntaArma;
    public GameObject particulasArma;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Ocultar la mira al inicio
        mira.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // movernos horizontalmente
        float movX;
        movX = Input.GetAxis("Horizontal");
        anim.SetFloat("absMovX", Mathf.Abs(movX));
        rb.linearVelocity = new Vector2(velX * movX, rb.linearVelocity.y);

        // detecccion si estamos en el piso 
        enPiso = Physics2D.OverlapCircle(refPie.position, 1f, 1 << 6); // Cuando el pie esta cerca del suelo
        anim.SetBool("enPiso", enPiso);

        // saltar
        if (Input.GetButtonDown("Jump") && enPiso)
        {
            rb.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        }

        // Girar el personaje
        if (tieneArma)
        {
            if (mira.transform.position.x < transform.position.x) transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            if (mira.transform.position.x > transform.position.x) transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            if (movX < 0) transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            if (movX > 0) transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (tieneArma)
        {
            //detectar el mouse y colocar ahi la mira
            mira.position = Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                -Camera.main.transform.position.z));

            refManoArma.position = mira.position;

            if (Input.GetButtonDown("Fire1")) disparar();
        }
    }

    private void FixedUpdate()
    {
        // Movimiento de camara
        Vector3 dondeEstoy = Camera.main.transform.position;
        Vector3 dondeQuieroIr = transform.position + new Vector3(0, 0, -20);

        Camera.main.transform.position = Vector3.Lerp(dondeEstoy, dondeQuieroIr, 0.5f);
    }

    private void LateUpdate()
    {
        if (tieneArma)
        {
            // que la mano apunte a la mira
            refCabeza.up = refOjos.position - mira.position;

            // que el arma tambien mire el mouse 
            contArma.up = contArma.position - mira.position;
        }
    }

    void disparar()
    {
        Vector3 direccion = (mira.position - contArma.position).normalized;

        // 1 arma patea 
        rb.AddForce(magnitudPateoArma * -direccion, ForceMode2D.Impulse);

        // 2 particulas
        Instantiate(particulasArma, refPuntaArma.position, Quaternion.identity);
        RaycastHit2D hit = Physics2D.Raycast(contArma.position, direccion, 100f, ~(1 << 10));
       if (hit.collider != null)
        {
            // le dio a algo
            if (hit.collider.gameObject.CompareTag("zombie"))
            {
                // le dio a a un zombie
                Destroy(hit.collider.gameObject);
            } 
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("arma"))
        {
            tieneArma = true;
            Destroy(collision.gameObject);
            contArma.gameObject.SetActive(true);

            // Mostrar la mira ahora que tenemos arma
            mira.gameObject.SetActive(true);
        }
    }
}