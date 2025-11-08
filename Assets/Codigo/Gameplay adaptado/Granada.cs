using UnityEngine;

public class Granada : MonoBehaviour
{
    public float tiempo = 4f;
    float momentoExplosion;
    public GameObject prefabExplosion;

    void Start()
    {
        momentoExplosion = Time.time + tiempo;
    }

    void Update()
    {
        if (Time.time > momentoExplosion) Explota();
    }

    void Explota()
    {
        // 1. destruir la granada
        Destroy(gameObject);

        // 2. explosion
        GameObject explosion = ExplosionFactory.Crear(prefabExplosion, transform.position, transform.rotation);
        Destroy(explosion, 2f); // se destruye después de 2 segundos

        // 3. daño
        Collider2D[] todosLosObjetos = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (Collider2D obj in todosLosObjetos)
        {
            // Si es zombie que implementa IEntidad → Morir con dirección
            if (obj.gameObject.CompareTag("zombie"))
            {
                if (obj.gameObject.TryGetComponent<IEntidad>(out var entidad) && entidad.EstaViva)
                {
                    entidad.Morir((obj.transform.position - transform.position).normalized);
                }
            }

            // Si es jugador → Recibir mordida (mantengo tu comportamiento)
            if (obj.gameObject.CompareTag("Player"))
            {
                var pj = obj.gameObject.GetComponent<personaje>();
                if (pj != null) pj.RecibirMordida(obj.transform.position);
            }
        }
    }
}
