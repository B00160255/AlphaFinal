using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab
    public float bulletSpeed = 20f; // Bullet speed
    public float shootingDistance = 2f; // Distance in front of the player to spawn the bullet
    public Transform shootingPoint; // Point from which the bullet will shoot (usually the player's gun or camera)
    public float fireRate = 0.5f; // Time between shots (fire rate)
    private float nextFireTime = 0f; // Time for the next shot

    void Update()
    {
        // Handle shooting input (e.g., left mouse button or fire button)
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            ShootBullet();
            nextFireTime = Time.time + fireRate; // Set the next allowed fire time
        }
    }

    private void ShootBullet()
    {
        if (bulletPrefab != null && shootingPoint != null)
        {
            // Calculate the spawn position in front of the player
            // Adjust the spawn position based on shooting distance and the shooting point's rotation
            Vector3 spawnPosition = shootingPoint.position + shootingPoint.forward * shootingDistance;

            // Log to ensure the bullet is being instantiated at the correct position
            Debug.Log("Shooting bullet from: " + spawnPosition);

            // Instantiate the bullet at the calculated position and with the shooting point's rotation
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, shootingPoint.rotation);

            // Get the Rigidbody component of the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply velocity in the forward direction of the shooting point
                rb.velocity = shootingPoint.forward * bulletSpeed;
            }
            else
            {
                Debug.LogError("Bullet does not have a Rigidbody component!");
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab or Shooting Point is not assigned.");
        }
    }
}
