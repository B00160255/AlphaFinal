using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 5f; // attack range
    public float attackDamage = 25f; // how much damage the player does
    public LayerMask enemyLayer; // this checks for enemies by seeing if the target has the enemy layer

    public GameObject empEffectPrefab; // reference to the EMP effect prefab
    public AudioClip electricSound; // reference to the electric sound effect
    private AudioSource zapSound; // reference to the AudioSource component which is called zapSound for this one

    public float attackCooldown = 1f; // cooldown duration between attacks
    private float lastAttackTime = 0f; // time when the player last attacked

    private void Start()
    {
        // this gets the audio source for the player
        zapSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // this checks if enough time has passed between the attacks
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (Input.GetButtonDown("Fire1")) 
            {
                Attack();
                lastAttackTime = Time.time; // this records the time of the attack for the cooldown
            }
        }
    }

    private void Attack()
    {
        // check for enemies within attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>()?.TakeDamage(attackDamage);
        }

        // spawn the EMP effect at the player's position
        if (empEffectPrefab != null)
        {
            Instantiate(empEffectPrefab, transform.position, Quaternion.identity);
        }

        // play the zap sound
        if (electricSound != null && zapSound != null)
        {
            zapSound.PlayOneShot(electricSound);
        }
    }
}
