using System.Collections;
using System.Collections.Generic;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float movX;
        movX = Input.GetAxis("Horizontal");
        anim.SetFloat("absMovX", Mathf.Abs(movX));
        rb.linearVelocity = new Vector2(velX * movX, rb.linearVelocity.y);

        enPiso = Physics2D.OverlapCircle(refPie.position, 1f, 1 << 6); // Cuando el pie esta cerca del suelo
        anim.SetBool("enPiso", enPiso);

        if (Input.GetButtonDown("Jump") && enPiso)
        {
            rb.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        }


        // Girar el personaje
        if (movX < 0)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        if (movX > 0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void FixedUpdate()
    {
        // Movimiento de camara
        Vector3 dondeEstoy = Camera.main.transform.position;
        Vector3 dondeQuieroIr = transform.position + new Vector3(0, 0, -20);

        Camera.main.transform.position = Vector3.Lerp(dondeEstoy, dondeQuieroIr, 0.5f);
    }
}
