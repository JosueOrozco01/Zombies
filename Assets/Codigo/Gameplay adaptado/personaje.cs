using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace GameplayAdaptado
{
public class personaje : EntidadBase, IPersistible
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
    bool miraValida;

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

    public TMPro.TextMeshProUGUI textoContBalas;

    int cantBalas = 0;

    public GameObject granada;
    public float fuerzaArrojarGranada = 50f;

    [Header("sonidos")]
    public GameObject disparo;
    public GameObject sinBalas;
    public GameObject agarrarMuniciones;
    public GameObject agarrarCheckpoint;
    public GameObject ShaggySalta;
    public GameObject ZombieMuere;
    public GameObject ShaggyDuele;
    public GameObject FondoSonido;

    [Header("Mobile Controls")]
    public GameObject btnJump;
    public GameObject btnFire;
    public GameObject btnGrenade;
    public GameObject btnLeft;
    public GameObject btnRight;

    [Header("Controles móviles")]
    private bool presionandoIzquierda = false;
    private bool presionandoDerecha = false;

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

        if (infoPartidaGuardada.hayPartidaGuardada) CargarEstado();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // Fondo sonoro
        GameObject bgm = Instantiate(FondoSonido, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(bgm);
        bgm.GetComponent<AudioSource>().loop = true;
        bgm.GetComponent<AudioSource>().Play();

#if UNITY_ANDROID || UNITY_IOS
            btnJump.SetActive(true);
            btnFire.SetActive(true);
            btnGrenade.SetActive(true);
            btnLeft.SetActive(true);
            btnRight.SetActive(true);
#elif UNITY_EDITOR
        btnJump.SetActive(true);
        btnFire.SetActive(true);
        btnGrenade.SetActive(true);
        btnLeft.SetActive(true);
        btnRight.SetActive(true);
#else
            btnJump.SetActive(false);
            btnFire.SetActive(false);
            btnGrenade.SetActive(false);
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
#endif

#if UNITY_EDITOR
        UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;
#endif
    }

    void Update()
    {
        if (energiaActual <= 0) return;

        float movX = 0;

#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        if (presionandoIzquierda) movX = -1f;
        else if (presionandoDerecha) movX = 1f;
#else
        movX = Input.GetAxis("Horizontal");
#endif

        anim.SetFloat("absMovX", Mathf.Abs(movX));
        rb.linearVelocity = new Vector2(velX * movX, rb.linearVelocity.y);

        enPiso = Physics2D.OverlapCircle(refPie.position, 1f, 1 << 6);
        anim.SetBool("enPiso", enPiso);

#if !UNITY_ANDROID && !UNITY_IOS
        if (Input.GetButtonDown("Jump") && enPiso)
            Saltar();
#endif

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
#if !UNITY_ANDROID && !UNITY_IOS
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = -30f;
            mira.position = mouseWorld;
            refManoArma.position = mira.position;

            Vector3 distancia = transform.position - mira.position;
            miraValida = (distancia.magnitude > 10f);
            mira.gameObject.SetActive(miraValida);

            bool clickSobreUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

            if (!clickSobreUI)
            {
                if (Input.GetButtonDown("Fire1") && miraValida)
                {
                    if (cantBalas > 0) disparar();
                    else
                    {
                        textoContBalas.color = Color.red;
                        textoContBalas.fontSize = 50;
                        SonidoFactory.Emitir(sinBalas, refManoArma.position, 1f);
                    }
                }

                if (Input.GetButtonDown("Fire2") && miraValida)
                {
                    ArrojarGranada();
                }
            }
#endif
        }

        if (Input.GetKeyDown(KeyCode.P)) CargarEstado();
        if (Input.GetKeyDown(KeyCode.O)) GuardarEstado();

        textoContBalas.text = cantBalas.ToString();
    }

    public void PresionarIzquierda()
    {
        presionandoIzquierda = true;
        Debug.Log("← Presionando izquierda");
    }

    public void SoltarIzquierda()
    {
        presionandoIzquierda = false;
        Debug.Log("← Soltó izquierda");
    }

    public void PresionarDerecha()
    {
        presionandoDerecha = true;
        Debug.Log("→ Presionando derecha");
    }

    public void SoltarDerecha()
    {
        presionandoDerecha = false;
        Debug.Log("→ Soltó derecha");
    }

    public void Saltar()
    {
        Debug.Log("Botón SALTO presionado");
        if (!enPiso) return;
        rb.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        SonidoFactory.Emitir(ShaggySalta, transform.position, 1f);
    }

    public void BotonDisparar()
    {
        if (!enPiso) return;
        Debug.Log("Botón DISPARO presionado");
        if (miraValida && cantBalas > 0) disparar();
        else if (cantBalas <= 0) SonidoFactory.Emitir(sinBalas, refManoArma.position, 1f);
    }

    public void BotonGranada()
    {
        Debug.Log("Botón GRANADA presionado");
        if (miraValida) ArrojarGranada();
    }

    void actualizarDisplay()
    {
        float valorAlfa = 1f / energiaMax * (energiaMax - energiaActual);
        mascaraDaño.color = new Color(1f, 1f, 1f, valorAlfa);

        textoEnergia.text = energiaActual.ToString();

        float valorDeseado = (float)energiaActual / energiaMax;
        if (valorDeseado == 0) barraLlena.fillAmount = 0;
        barraLlena.fillAmount = Mathf.Lerp(barraLlena.fillAmount, valorDeseado, 0.1f);
    }

    private void FixedUpdate()
    {
        Vector3 dondeEstoy = Camera.main.transform.position;
        Vector3 dondeQuieroIr = transform.position + new Vector3(0, 0, -20);

        Camera.main.transform.position = Vector3.Lerp(dondeEstoy, dondeQuieroIr, 0.5f);

        actualizarDisplay();

        float valorAlfa = Mathf.Lerp(telaNegra.color.a, valorAlfaDeseadoTelaNegra, 0.05f);
        telaNegra.color = new Color(0, 0, 0, valorAlfa);

        if (valorAlfa > 0.9f && valorAlfaDeseadoTelaNegra == 1) SceneManager.LoadScene("Scenes/Escena");

        textoContBalas.color = Color.Lerp(textoContBalas.color, Color.white, 0.1f);
        textoContBalas.fontSize = Mathf.Lerp(textoContBalas.fontSize, 36, 0.1f);
    }

    private void LateUpdate()
    {
        if (energiaActual <= 0) return;

        if (magnitudSacudida > 0.05f)
        {
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

            magnitudSacudida *= 0.93f;
        }
        else
        {
            camaraSacudir.localRotation = Quaternion.identity;
            camaraSacudir.localPosition = new Vector3(0, 0, camaraSacudir.localPosition.z);
        }

        if (tieneArma && miraValida)
        {
            refCabeza.up = refOjos.position - mira.position;
            contArma.up = contArma.position - mira.position;
            refManoArma.position = mira.position;
        }
    }

    void ArrojarGranada()
    {
        GameObject nuevaGranada = GranadaFactory.Crear(
            granada, transform.position + transform.forward, Quaternion.identity);

        Vector2 dir = (mira.position - transform.position).normalized;

        nuevaGranada.GetComponent<Rigidbody2D>().AddForce(fuerzaArrojarGranada * dir, ForceMode2D.Impulse);
    }

    // Persitencia (SOLID: IPersistible)
    public void GuardarEstado()
    {
        infoPartidaGuardada.infoShaggy.cantBalas = cantBalas;
        infoPartidaGuardada.infoShaggy.energiaActual = energiaActual;
        infoPartidaGuardada.infoShaggy.posicion = transform.position;

        infoPartidaGuardada.hayPartidaGuardada = true;

        infoPartidaGuardada.infoPaqueteBalas.Clear();
        Transform todosLosPaquetes = GameObject.Find("Paquete de balas").transform;
        foreach (Transform paq in todosLosPaquetes)
        {
            infoPartidaGuardada.TipoInfoPaqueteBalas itemPaq = new infoPartidaGuardada.TipoInfoPaqueteBalas
            {
                activo = paq.gameObject.activeSelf
            };
            infoPartidaGuardada.infoPaqueteBalas.Add(itemPaq);
        }

        infoPartidaGuardada.infoZombies.Clear();
        Transform todosLosZombies = GameObject.Find("Zombies").transform;
        foreach (Transform zombie in todosLosZombies)
        {
            infoPartidaGuardada.TipoInfoZombie itemZombie = new infoPartidaGuardada.TipoInfoZombie
            {
                activo = zombie.gameObject.activeSelf,
                posicion = zombie.position
            };
            infoPartidaGuardada.infoZombies.Add(itemZombie);
        }
    }

    public void CargarEstado()
    {
        cantBalas = infoPartidaGuardada.infoShaggy.cantBalas;
        energiaActual = infoPartidaGuardada.infoShaggy.energiaActual;
        transform.position = infoPartidaGuardada.infoShaggy.posicion;

        Transform todosLosPaquetes = GameObject.Find("Paquete de balas").transform;
        int i = 0;
        foreach (Transform paq in todosLosPaquetes)
        {
            paq.gameObject.SetActive(infoPartidaGuardada.infoPaqueteBalas[i++].activo);
        }

        Transform todosLosZombies = GameObject.Find("Zombies").transform;
        i = 0;
        foreach (Transform zombie in todosLosZombies)
        {
            zombie.GetComponent<SpriteRenderer>().enabled = infoPartidaGuardada.infoZombies[i].activo;
            var audio = zombie.GetComponent<AudioSource>();
            if (audio) audio.enabled = infoPartidaGuardada.infoZombies[i].activo;

            var zz = zombie.GetComponent<zombies>();
            if (zz != null)
            {
                zz.vivo = infoPartidaGuardada.infoZombies[i].activo;
                // usar helper para sincronizar el estado protegido
                zz.SetEstaViva(infoPartidaGuardada.infoZombies[i].activo);
            }

            zombie.position = infoPartidaGuardada.infoZombies[i++].posicion;
        }
    }

    void disparar()
    {
        Vector3 direccion = (mira.position - contArma.position).normalized;

        Vector3 direccionRetroceso = -direccion;
        direccionRetroceso.y *= 0.2f;
        rb.AddForce(magnitudPateoArma * direccionRetroceso, ForceMode2D.Impulse);

        ParticulaFactory.Emitir(particulasArma, refPuntaArma.position, Quaternion.identity, 2f);

        sacudirCamara(1f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(contArma.position, direccion, 100f, ~(1 << 8));

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("cabezaZombie"))
                {
                    var zComp = hit.transform.GetComponentInParent<zombies>();
                    if (zComp != null && zComp.vivo)
                    {
                        zComp.muere(direccion);

                        ParticulaFactory.Emitir(particulasMuchaSangreVerde, hit.point, Quaternion.identity, 2f);
                        SonidoFactory.Emitir(ZombieMuere, transform.position, 1f);
                        break;
                    }
                }
                else if (hit.collider.CompareTag("zombie"))
                {
                    if (hit.rigidbody != null)
                        hit.rigidbody.AddForce(magnitudReaccionDisparo * direccion, ForceMode2D.Impulse);

                    ParticulaFactory.Emitir(particulasSangreVerde, hit.point, Quaternion.identity, 2f);
                }
            }
        }

        cantBalas -= 1;
        SonidoFactory.Emitir(disparo, refManoArma.position, 1f);
    }

    void NuevoSonido(GameObject prefab, Vector2 posicion, float duracion = 5f)
    {
        // Reencaminado por compatibilidad a la fábrica (mantiene firma original)
        SonidoFactory.Emitir(prefab, posicion, duracion);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("arma"))
        {
            tieneArma = true;
            contArma.gameObject.SetActive(true);
            cantBalas += 8;

            textoContBalas.color = Color.green;
            textoContBalas.fontSize = 50;

            SonidoFactory.Emitir(agarrarMuniciones, collision.transform.position, 1f);

            Destroy(collision.gameObject);
            mira.gameObject.SetActive(true);
        }

        if (collision.gameObject.CompareTag("balas"))
        {
            collision.gameObject.SetActive(false);
            cantBalas += 8;
            textoContBalas.color = Color.green;
            textoContBalas.fontSize = 50;
            SonidoFactory.Emitir(agarrarMuniciones, collision.transform.position, 1f);
        }

        if (collision.gameObject.CompareTag("checkpoint"))
        {
            GuardarEstado();
            SonidoFactory.Emitir(agarrarCheckpoint, collision.transform.position, 1f);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("mar"))
        {
            MorirEnMar();
        }
    }

    void MorirEnMar()
    {
        Debug.Log("☠️ El personaje cayó al mar");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void sacudirCamara(float maximo)
    {
        magnitudSacudida = maximo;
    }

    public void RecibirMordida(Vector2 posicion)
    {
        if (energiaActual <= 0) return;

        energiaActual -= 1;

        if (energiaActual <= 0)
        {
            Debug.Log("Adios mundo cruel");
            actualizarDisplay();
            anim.SetTrigger("muere");
        }
        else
        {
            Debug.Log("auch! Ahora tengo" + energiaActual + " de " + energiaMax);

            ParticulaFactory.Emitir(particulasSanngreShaggy, posicion, Quaternion.identity, 2f);

            anim.SetTrigger("auch");

            SonidoFactory.Emitir(ShaggyDuele, transform.position, 1f);
        }
    }

    public void iniciarFadeOut()
    {
        valorAlfaDeseadoTelaNegra = 1; // fade out
    }
    // Abstract Method - implementación de EntidadBase
    public override void RecibirDaño(int cantidad, Vector2 posicionImpacto)
    {
        // Reusa tu mecánica de mordida como daño
        energiaActual -= cantidad;
        if (energiaActual <= 0) Morir(Vector3.zero);
    }

    public override void Morir(Vector3 direccion)
    {
        EstaViva = false;
        valorAlfaDeseadoTelaNegra = 1;
        anim.SetTrigger("muere");
    }
}
}
