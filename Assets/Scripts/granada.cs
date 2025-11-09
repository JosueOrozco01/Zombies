using UnityEngine;

public class granada : MonoBehaviour
{
    public float tiempo = 4f;
    float momentoExplosion;
    public GameObject prefabExplosion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        momentoExplosion = Time.time + tiempo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > momentoExplosion) Explota();
    }

    void Explota()
    {
        // 1. destruir la grandad
        Destroy(gameObject);

        // 2. explosion
        GameObject explosion = Instantiate(prefabExplosion, transform.position, transform.rotation);
        Destroy(explosion, 2f); // se destruye después de 2 segundos

        // 3. daño
        // zombies
        Collider2D[] todosLosObjetos = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (Collider2D obj in todosLosObjetos)
        {
            if (obj.gameObject.CompareTag("zombie"))
            {
                if (obj.gameObject.GetComponent<zombies>().vivo)
                {
                    obj.gameObject.GetComponent<zombies>().muere(
                        (obj.transform.position - transform.position).normalized
                    );
                }
            }

            if (obj.gameObject.CompareTag("Player"))
            {
                obj.gameObject.GetComponent<personaje>().RecibirMordida(obj.transform.position);
            }
        }
    }
}
