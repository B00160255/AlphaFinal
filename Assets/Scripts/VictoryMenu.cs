using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    // this restarts the game to level 1
    public void RestartGame()
    {
        Time.timeScale = 1f; // this makes sure the game is not paused
        SceneManager.LoadScene("Level1");
    }

    // this sends you to the main menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // makes sure the game's not paused
        SceneManager.LoadScene("Menu"); 
    }
}
