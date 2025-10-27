using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // Load specific scenes by name
    public void LoadMainMenu()
    {
        Time.timeScale = 1; // Unpause in case coming from paused game
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void LoadInstructions()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Instructions");
    }

    public void LoadIntroduction()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Introduction");
    }

    // Reload current scene (for retry)
    public void ReloadCurrentScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}