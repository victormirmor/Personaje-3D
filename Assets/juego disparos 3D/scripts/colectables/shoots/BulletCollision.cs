using UnityEngine;

namespace CompleteProject
{
    [RequireComponent(typeof(BulletDamage))]
    public class BulletCollision : MonoBehaviour
    {
        private BulletDamage bulletDamage;

        private void Awake()
        {
            bulletDamage = GetComponent<BulletDamage>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameTags.Enemy))
            {
                if (bulletDamage != null)
                {
                    bulletDamage.ApplyDamageToTarget(other.gameObject, transform.position);
                }

                Destroy(gameObject);
            }
            else if (other.CompareTag(GameTags.Environment))
            {
                Destroy(gameObject);
            }
        }
    }
}