using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public float maxLifetime = 5f;
    //EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

    void Start()
    {
        // Se destruye automáticamente si vuela sin golpear nada
        Destroy(gameObject, maxLifetime);
    }

    void Update()
    {
        // Avance constante en la dirección a la que apunta
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameTags.Enemy))
        {
            

            /*if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, transform.position);
            }*/

            Destroy(gameObject);
        }
        else if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}