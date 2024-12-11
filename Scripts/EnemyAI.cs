using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Health for the enemy drone
    public float health = 100f;

    // Movement speeds for chasing
    public float chaseSpeed = 5f;

    // Detection range and attack range for the drone
    public float detectionRange = 90f;
    public float attackRange = 2f;

    // Damage settings for the enemy's attack (either bullets or contact)
    public float damageAmount = 10f; // Bullet damage to the player
    public float damageInterval = 1f; // Time between each bullet attack
    private float nextDamageTime = 0f;

    // Reference to the player and player health script
    private Transform player;
    private PlayerHealth playerHealth;

    // Navmesh obstacle component
    private NavMeshObstacle navObstacle;

    // Flag to check if the drone is attacking
    private bool isAttacking = false;

    // Health pack settings
    public GameObject healthPackPrefab; // Reference to the health pack prefab
    public float dropChance = 10f; // Percentage chance to drop the health pack

    // Bullet attack settings
    public GameObject enemyBulletPrefab; // Enemy bullet prefab
    public float bulletSpeed = 10f; // Bullet speed

    // Explosion settings
    public GameObject explosionPrefab; // Explosion prefab
    public float explosionRadius = 5f; // Explosion radius
    public float explosionDamage = 15f; // Explosion damage to player

    private void Start()
    {
        // Find the player in the scene and get their health script
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();

        // Get the navmesh obstacle component and enable carving
        navObstacle = GetComponent<NavMeshObstacle>();
        if (navObstacle != null)
        {
            navObstacle.carving = true;
        }
    }

    private void Update()
    {
        // Calculate distance between the drone and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within detection range, chase them
        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer(distanceToPlayer);

            // If the player is within attack range, shoot bullets
            if (distanceToPlayer <= attackRange)
            {
                ShootBullet();
            }
        }
        else
        {
            StopChasing();
        }
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        if (!isAttacking)
        {
            // Move towards the player
            Vector3 targetPosition = player.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);

            if (navObstacle != null)
            {
                navObstacle.enabled = true;
            }
        }
    }

    private void StopChasing()
    {
        if (navObstacle != null)
        {
            navObstacle.enabled = false;
        }
    }

    private void ShootBullet()
    {
        if (Time.time >= nextDamageTime)
        {
            if (enemyBulletPrefab != null)
            {
                // Get a target point on the player (e.g., center of mass or head)
                Vector3 targetPoint = player.position + Vector3.up * 1.5f; // Adjust height as needed, or use player's head position directly

                // Use raycasting to check where the bullet should aim
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (targetPoint - transform.position).normalized, out hit))
                {
                    // Use hit point as the new target
                    targetPoint = hit.point;
                }

                // Instantiate the bullet at the enemy's position
                GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);

                // Apply velocity to the bullet towards the target point
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 directionToTarget = (targetPoint - transform.position).normalized;
                    rb.velocity = directionToTarget * bulletSpeed;
                }

                // Pass the damage amount to the enemy bullet behavior
                EnemyBullet bulletBehavior = bullet.GetComponent<EnemyBullet>();
                if (bulletBehavior != null)
                {
                    bulletBehavior.damageAmount = damageAmount;
                }
            }

            // Set cooldown for the next bullet
            nextDamageTime = Time.time + damageInterval;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");

        // Create explosion effect on death
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Apply damage to the player if they are within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                PlayerHealth playerHealth = col.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(explosionDamage);
                }
            }
        }

        // Optionally drop a health pack
        if (healthPackPrefab != null && Random.value * 100f <= dropChance)
        {
            Instantiate(healthPackPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy the drone object
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the drone collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Blow up on contact and deal explosion damage
            Die();
        }
    }
}
