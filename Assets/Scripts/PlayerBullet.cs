using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float damageAmount = 25f; // Damage dealt by the bullet
    public float lifeTime = 5f; // Time before the bullet is destroyed

    private void Start()
    {
        // Destroy the bullet after its lifetime ends to prevent clutter
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits an enemy
        if (collision.collider.CompareTag("Enemy"))
        {
            // If the enemy is hit, deal damage
            EnemyAI enemyAI = collision.collider.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damageAmount);
            }

            // Destroy the bullet after hitting an enemy
            Destroy(gameObject);
        }
        else
        {
            // Destroy the bullet if it hits anything else (like the environment)
            Destroy(gameObject);
        }
    }
}
