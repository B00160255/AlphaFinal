using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damageAmount = 15f; // Damage dealt by the bullet
    public float lifeTime = 5f; // Time before the bullet is destroyed

    private void Start()
    {
        // Destroy the bullet after its lifetime ends to prevent clutter
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits the player
        if (collision.collider.CompareTag("Player"))
        {
            // If the player is hit, apply damage
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            // Destroy the bullet after hitting the player
            Destroy(gameObject);
        }
        else
        {
            // Destroy the bullet if it hits anything else (like the environment)
            Destroy(gameObject);
        }
    }
}
