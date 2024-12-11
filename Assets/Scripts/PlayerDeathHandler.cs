using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    // this method will be called when the player dies
    public void HandlePlayerDeath()
    {
        Debug.Log("Player has died!");

        // load the "DeathMenu" scene
        SceneManager.LoadScene("DeathMenu");
    }
}
