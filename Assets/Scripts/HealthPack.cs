using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healAmount = 20f;  // this is how much health the health pack restores

    private void OnTriggerEnter(Collider other)
    {
        // when the player collides with the health pack, heal them
        if (other.CompareTag("Player")) 
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);  // heal the player
                Debug.Log($"Player healed by {healAmount}");
                Destroy(gameObject);  // this gets rid of the health pack when it's picked up
            }
        }
    }
}
