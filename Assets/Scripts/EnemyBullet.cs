using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damageAmount = 15f; // damage dealt by the bullet
    public float lifeTime = 5f; // time before the bullet is destroyed
    public AudioClip bulletSound; // bullet sound to play when the bullet is spawned
    private AudioSource audioSource; // audio source component

    private void Start()
    {
        // this gets the audio source for the bullet
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // this plays the bullet sound
        if (bulletSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bulletSound);
        }

        // destroy the bullet after its lifetime ends to prevent clutter
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if the bullet hits the player, and apply damage if it does
        if (collision.collider.CompareTag("Player"))
        {

            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            // dstroy the bullet after hitting the player
            Destroy(gameObject);
        }
        else
        {
            // this destroys the bullet if it hits something else, like a building or another drone
            Destroy(gameObject);
        }
    }
}

