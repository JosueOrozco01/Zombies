using GameplayAdaptado.Factories;
using GameplayAdaptado.Entities;
using UnityEngine;

namespace GameplayAdaptado.Weapons
{
    public class GranadaRefactor : MonoBehaviour
    {
        public float tiempo = 4f;
        float momentoExplosion;

    [Header("Dependencies")]
    public GameplayAdaptado.IGameObjectFactory factories;

        [Header("Fallbacks")]
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
            Destroy(gameObject);

            GameObject explosion = null;
            if (factories != null && factories.ExplosionFactory != null)
            {
                explosion = factories.ExplosionFactory.CrearExplosion(transform.position, transform.rotation);
            }
            else if (prefabExplosion != null)
            {
                explosion = Instantiate(prefabExplosion, transform.position, transform.rotation);
            }

            if (explosion != null) Destroy(explosion, 2f);

            // Apply damage to zombies and player (compatible with original and refactor)
            Collider2D[] todos = Physics2D.OverlapCircleAll(transform.position, 10f);
            foreach (var obj in todos)
            {
                if (obj.gameObject.CompareTag("zombie"))
                {
                    var zRef = obj.GetComponent<ZombiesRefactor>();
                    if (zRef != null) zRef.Muere((obj.transform.position - transform.position).normalized);
                    else
                    {
                        var zOld = obj.GetComponent<zombies>();
                        if (zOld != null) zOld.muere((obj.transform.position - transform.position).normalized);
                    }
                }

                if (obj.gameObject.CompareTag("Player"))
                {
                    var pRef = obj.GetComponent<GameplayAdaptado.Entities.PersonajeRefactor>();
                    if (pRef != null) pRef.RecibirDaño(1, obj.transform.position);
                    else
                    {
                        var pOld = obj.GetComponent<personaje>();
                        if (pOld != null) pOld.RecibirMordida(obj.transform.position);
                    }
                }
            }
        }
    }
}
