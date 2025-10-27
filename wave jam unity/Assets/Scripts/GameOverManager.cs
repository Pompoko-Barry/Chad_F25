using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalWaveText;

    [Header("Win UI")]
    public GameObject winPanel;
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI winWaveText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Make sure panels are hidden at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void ShowGameOver(int finalScore, int finalWave)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverTitleText != null)
                gameOverTitleText.text = "GAME OVER";

            if (finalScoreText != null)
                finalScoreText.text = "Final Score: " + finalScore;

            if (finalWaveText != null)
                finalWaveText.text = "Waves Completed: " + (finalWave - 1);

            // Pause the game
            Time.timeScale = 0;
        }
    }

    public void ShowWin(int finalScore, int finalWave)
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);

            if (winScoreText != null)
                winScoreText.text = "Final Score: " + finalScore;

            if (winWaveText != null)
                winWaveText.text = "Waves Completed: " + finalWave;

            // Pause the game
            Time.timeScale = 0;
        }
    }

    //// Button Methods
    //public void RetryGame()
    //{
    //    // Unpause time
    //    Time.timeScale = 1;

    //    // Reload current scene
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    //public void ReturnToMainMenu()
    //{
    //    // Unpause time
    //    Time.timeScale = 1;

    //    // Load main menu scene (change "MainMenu" to your actual scene name)
    //    SceneManager.LoadScene("MainMenu");
    //}
}

