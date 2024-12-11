using UnityEngine;

public class DroneSpawnManager : MonoBehaviour
{
    public GameObject Drone;          // Regular drone prefab
    public GameObject StrongerDrone;  // Stronger drone prefab

    public float spawnDistanceMin = 20f; // Minimum distance the drones will spawn from the player
    public float spawnDistanceMax = 30f; // Maximum distance the drones will spawn from the player
    public float spawnHeightMin = 5f;   // Minimum height where the drones will spawn
    public float spawnHeightMax = 10f;  // Maximum height where the drones will spawn

    public float strongerDroneChance = 0.2f; // 20% chance to spawn a stronger drone

    private Transform player;  // Reference to the player's position

    void Start()
    {
        // Find the player in the scene
        player = GameObject.FindWithTag("Player").transform;

        // Start spawning drones repeatedly
        InvokeRepeating(nameof(SpawnDrone), 2f, 3f); // Spawn drones every 3 seconds after a 2-second delay
    }

    // Spawns a drone near the player
    void SpawnDrone()
    {
        // Ensure the player exists
        if (player != null)
        {
            // Calculate random spawn position
            float spawnX = player.position.x + Random.Range(spawnDistanceMin, spawnDistanceMax); // Random X position away from the player
            float spawnY = Random.Range(spawnHeightMin, spawnHeightMax); // Random height within the range
            float spawnZ = player.position.z; // Keep Z the same as the player, or modify if needed

            Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);

            // Determine whether to spawn a regular or stronger drone
            GameObject droneToSpawn = Random.value < strongerDroneChance ? StrongerDrone : Drone;

            // Instantiate the selected drone at the random spawn position
            Instantiate(droneToSpawn, spawnPos, Quaternion.identity);
        }
    }
}
