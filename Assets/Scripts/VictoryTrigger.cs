using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    // here's the name of the victory scene
    public string victoryScene = "VictoryScene";  

    private void OnTriggerEnter(Collider other)
    {
        // this checks if the player touched the trigger
        if (other.CompareTag("Player"))
        {
            // if they did, this loads the victory scene
            SceneManager.LoadScene(victoryScene);
        }
    }
}
