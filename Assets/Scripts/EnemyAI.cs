using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // health for the enemy drone
    public float health = 100f;

    // movement speeds for chasing
    public float chaseSpeed = 5f;

    // detection range and attack range for the drone
    public float detectionRange = 90f;
    public float attackRange = 2f;

    // damage settings for the enemy's attack (either bullets or contact)
    public float damageAmount = 10f; // bullet damage to the player
    public float damageInterval = 1f; //time between each bullet attack
    private float nextDamageTime = 0f;

    // reference to the player and player health script
    private Transform player;
    private PlayerHealth playerHealth;

    // navmesh obstacle component
    private NavMeshObstacle navObstacle;

    // flag to check if the drone is attacking
    private bool isAttacking = false;

    // health pack settings
    public GameObject healthPackPrefab; // reference to the health pack prefab
    public float dropChance = 10f; // percentage chance to drop the health pack

    // bullet attack settings
    public GameObject enemyBulletPrefab; // enemy bullet prefab
    public float bulletSpeed = 10f; // bullet speed

    // explosion settings
    public GameObject explosionPrefab; // explosion prefab
    public float explosionRadius = 5f; // explosion radius
    public float explosionDamage = 15f; // explosion damage to player

    private void Start()
    {
        // find the player and get their health script
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();

        // this gets the nevmash obstacle component and enables carving
        navObstacle = GetComponent<NavMeshObstacle>();
        if (navObstacle != null)
        {
            navObstacle.carving = true;
        }
    }

    private void Update()
    {
        // this calculates the distance between the drone and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // this makes the drone chase the player if they're in detection range
        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer(distanceToPlayer);

            // if the player is within attack range, shoot bullets
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
            // move towards the player
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
                // this targets the player
                Vector3 targetPoint = player.position + Vector3.up * 1.5f; // this adjusts the aim height because otherwise it aims at your feet

                // this uses raycasting to determine where to aim
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (targetPoint - transform.position).normalized, out hit))
                {
                    // this uses the hit point as the new target 
                    targetPoint = hit.point;
                }

                // this spawns a bullet where the enemy is
                GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);

                // this applies velocity to the bullets
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 directionToTarget = (targetPoint - transform.position).normalized;
                    rb.velocity = directionToTarget * bulletSpeed;
                }

                // this passes the damage amount to the bullet behaviour script
                EnemyBullet bulletBehavior = bullet.GetComponent<EnemyBullet>();
                if (bulletBehavior != null)
                {
                    bulletBehavior.damageAmount = damageAmount;
                }
            }

            // cooldown for the next bullet
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

        // this creates an explosion when the drone dies
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // this hurts the player if they're within the explosion radius
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

        // this drops a health pack sometimes, it's random
        if (healthPackPrefab != null && Random.value * 100f <= dropChance)
        {
            Instantiate(healthPackPrefab, transform.position, Quaternion.identity);
        }

        // instantly destroy the enemy object
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // this checks if the drone touched the player, and explodes if it does
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Die();
        }
    }
}
