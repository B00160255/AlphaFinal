using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    // restart level
    public void RestartLevel()
    {
        // reloads the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // This function loads the main menu scene
    public void GoToMainMenu()
    {
        //return to main menu
        SceneManager.LoadScene("Menu");
    }

    // This function quits the game
    public void QuitGame()
    {
        // quit app
        Application.Quit();

        // log for quit
        Debug.Log("Game is quitting...");
    }
}
