using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    // sacudir camara
    public Transform camaraSacudir;
    float magnitudSacudida;

    public float magnitudReaccionDisparo = 300f;
    public GameObject particulasSangreVerde;
    public GameObject particulasMuchaSangreVerde;

    public GameObject particulasSanngreShaggy;
    public UnityEngine.UI.Image mascaraDaño;

    // energia
    int energiaMax = 5;
    int energiaActual;
    public TMPro.TextMeshProUGUI textoEnergia;
    public UnityEngine.UI.Image barraLlena;

    //muerte
    public UnityEngine.UI.Image telaNegra;
    float valorAlfaDeseadoTelaNegra;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Ocultar la mira al inicio
        mira.gameObject.SetActive(false);
        energiaActual = energiaMax;

        // fade in inicial
        telaNegra.color = new Color(0, 0, 0, 1); // negro
        valorAlfaDeseadoTelaNegra = 0; // transparente
    }

    // Update is called once per frame
    void Update()
    {
        if (energiaActual <= 0) return;
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

    void actualizarDisplay()
    {
        // mascara roja de daño
        float valorAlfa = 1f / energiaMax * (energiaMax - energiaActual);
        mascaraDaño.color = new Color(1f, 1f, 1f, valorAlfa);

        // metodo 1: texto
        textoEnergia.text = energiaActual.ToString();

        // metodo 2: barra
        float valorDeseado = (float)energiaActual / energiaMax;
        if (valorDeseado == 0) barraLlena.fillAmount = 0;
        barraLlena.fillAmount = Mathf.Lerp(barraLlena.fillAmount, valorDeseado, 0.1f);


    }

    private void FixedUpdate()
    {
        // Movimiento de camara
        Vector3 dondeEstoy = Camera.main.transform.position;
        Vector3 dondeQuieroIr = transform.position + new Vector3(0, 0, -20);

        Camera.main.transform.position = Vector3.Lerp(dondeEstoy, dondeQuieroIr, 0.5f);

        actualizarDisplay();

        // manejar la tela negra
        float valorAlfa = Mathf.Lerp(telaNegra.color.a, valorAlfaDeseadoTelaNegra, 0.05f);
        telaNegra.color = new Color(0, 0, 0, valorAlfa);

        // reiniciar escena cuando se complete fade out 
        if (valorAlfa > 0.9f &&  valorAlfaDeseadoTelaNegra == 1) SceneManager.LoadScene("Scenes/Escena");
    }

    private void LateUpdate()
    {
        if (energiaActual <= 0) return;
        // Efecto de sacudida en la cámara hija
        if (magnitudSacudida > 0.05f)
        {
            // Movimiento aleatorio suave en rotación y posición
            camaraSacudir.localRotation = Quaternion.Euler(
                Random.Range(-magnitudSacudida * 2f, magnitudSacudida * 2f),
                Random.Range(-magnitudSacudida * 2f, magnitudSacudida * 2f),
                Random.Range(-magnitudSacudida * 2f, magnitudSacudida * 2f)
            );

            camaraSacudir.localPosition = new Vector3(
                Random.Range(-magnitudSacudida * 0.05f, magnitudSacudida * 0.05f),
                Random.Range(-magnitudSacudida * 0.05f, magnitudSacudida * 0.05f),
                camaraSacudir.localPosition.z
            );

            // Disminuye más lentamente
            magnitudSacudida *= 0.93f;
        }
        else
        {
            camaraSacudir.localRotation = Quaternion.identity;
            camaraSacudir.localPosition = new Vector3(0, 0, camaraSacudir.localPosition.z);
        }

        if (tieneArma)
        {
            // que la cabeza apunte a la mira
            refCabeza.up = refOjos.position - mira.position;

            // que el arma mire el mouse
            contArma.up = contArma.position - mira.position;
        }
    }
        
    void disparar()
    {
        Vector3 direccion = (mira.position - contArma.position).normalized;

        // Retroceso del arma
        Vector3 direccionRetroceso = -direccion;
        direccionRetroceso.y *= 0.2f; 
        rb.AddForce(magnitudPateoArma * direccionRetroceso, ForceMode2D.Impulse);

        // Partículas del arma (se destruyen automáticamente después de 2s)
        GameObject particulaArma = Instantiate(particulasArma, refPuntaArma.position, Quaternion.identity);
        Destroy(particulaArma, 2f);

        // Sacudir cámara
        sacudirCamara(1.5f);

        // Raycast para detectar todo lo que toque
        RaycastHit2D[] hits = Physics2D.RaycastAll(contArma.position, direccion, 100f, ~(1 << 8));
        
        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("cabezaZombie"))
                {
                    // Matar zombie al golpear la cabeza
                    zombies z = hit.transform.GetComponentInParent<zombies>();
                    if (z != null)
                    {
                        z.muere(direccion);

                        // Partículas de sangre verde
                        GameObject particulaSangre = Instantiate(particulasMuchaSangreVerde, hit.point, Quaternion.identity);
                        Destroy(particulaSangre, 2f);

                        break; // Detenemos el loop, ya mató al zombie
                    }
                }
                else if (hit.collider.CompareTag("zombie"))
                {
                    // Solo golpe al cuerpo, sin matar
                    if (hit.rigidbody != null)
                        hit.rigidbody.AddForce(magnitudReaccionDisparo * direccion, ForceMode2D.Impulse);

                    GameObject particulaSangre = Instantiate(particulasSangreVerde, hit.point, Quaternion.identity);
                    Destroy(particulaSangre, 2f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("arma"))
        {
            // agarrar el arma
            tieneArma = true;
            Destroy(collision.gameObject);
            contArma.gameObject.SetActive(true);

            // Mostrar la mira ahora que tenemos arma
            mira.gameObject.SetActive(true);
        }
    }

    void sacudirCamara(float maximo)
    {
        magnitudSacudida = maximo;
    }

    public void RecibirMordida(Vector2 posicion)
    {
        if (energiaActual <= 0) return;

        //reducir la energia
        energiaActual -= 1;

        if (energiaActual <= 0)
        {
            Debug.Log("Adios mundo cruel");
            actualizarDisplay();
            // Destroy(gameObject);

            // comienza el proceso de la muerte 
            anim.SetTrigger("muere");
        }
        else
        {
            Debug.Log("auch! Ahora tengo" + energiaActual + " de " + energiaMax);

            // Partículas de sangre en Shaggy
            Instantiate(particulasSanngreShaggy, posicion, Quaternion.identity);

            // Disparar la animacion 
            anim.SetTrigger("auch");
        }
    }

    public void iniciarFadeOut()
    {
        valorAlfaDeseadoTelaNegra = 1; // fade out
    }
}