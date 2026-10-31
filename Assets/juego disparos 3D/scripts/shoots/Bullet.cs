using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public float maxLifetime = 5f;
    EnemyHealth enemyHealth;

    void Start(){
    
        Destroy(gameObject, maxLifetime);
    }

    void Update(){
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other){
        
        if (other.CompareTag("Enemy")){
            enemyHealth = other.GetComponent<EnemyHealth>();
            Debug.Log ("Es un enemigo");

            enemyHealth.TakeDamage(damage, transform.position);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}