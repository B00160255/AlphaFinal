using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healAmount = 20f;  // Amount of health the pack restores

    private void OnTriggerEnter(Collider other)
    {
        // When the player collides with the health pack, heal them
        if (other.CompareTag("Player"))  // Make sure "Player" tag is correct in Unity
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);  // Heal the player
                Debug.Log($"Player healed by {healAmount}");
                Destroy(gameObject);  // Destroy the health pack after it is collected
            }
        }
    }
}
