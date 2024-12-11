using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // maximum health
    public float currentHealth; // current health

    public TextMeshProUGUI healthText; // UI element to display health
    public AudioClip healSound; // heal sound clip
    private AudioSource audioSource; // audio source for playing sounds

    private void Start()
    {
        // initialize health to maxHealth at the start
        currentHealth = maxHealth;

        // get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // update the health display
        UpdateHealthDisplay();
    }

    // function to reduce health when taking damage
    public void TakeDamage(float damageAmount)
    {
        // reduce health by the damage amount
        currentHealth -= damageAmount;

        // clamp health to prevent it from going below zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // update the health display
        UpdateHealthDisplay();

        // check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // function to heal the player
    public void Heal(float healAmount)
    {
        // increase health by the heal amount
        currentHealth += healAmount;

        // clamp health to not exceed maxHealth
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // play the heal sound if a heal sound is assigned
        if (healSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(healSound);
        }

        // update the health display
        UpdateHealthDisplay();
    }

    // function to update the health display UI
    private void UpdateHealthDisplay()
    {
        // update the UI text to show current health
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }

    // function to handle player's death
    private void Die()
    {
        Debug.Log("Player has died!");
        // load the DeathScreen scene
        SceneManager.LoadScene("DeathScene");
    }
}