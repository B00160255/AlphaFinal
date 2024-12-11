using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenuCanvas;      // reference to the Main Menu Canvas
    public GameObject settingsMenuCanvas;  // reference to the Settings Menu Canvas

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1); 
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    // open the Settings menu and hide the main menu
    public void OpenSettings()
    {
        mainMenuCanvas.SetActive(false);          // hide the main menu
        settingsMenuCanvas.SetActive(true);       // show the settings menu
    }

    // close the Settings menu and show the main menu
    public void CloseSettings()
    {
        settingsMenuCanvas.SetActive(false);      // hide the settings menu
        mainMenuCanvas.SetActive(true);           // show the main menu
    }
}

