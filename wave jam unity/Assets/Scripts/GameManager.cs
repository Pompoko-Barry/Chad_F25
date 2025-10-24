using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI waveText;

    [Header("Game Settings")]
    public int score = 0;
    public int lives = 3;

    [Header("Scoring")]
    public int correctSortPoints = 10;
    public int wrongSortPenalty = 5;
    public int edgePenalty = 10;

    private VoidSpawner spawner;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawner = FindFirstObjectByType<VoidSpawner>();
        UpdateUI();
    }

    public void CorrectSort(bool wasBaby)
    {
        score += correctSortPoints;
        UpdateUI();

    }

    public void WrongSort(bool wasBaby)
    {
        score -= wrongSortPenalty;
        lives--;

        if (score < 0) score = 0;

        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void SphereReachedEdge(bool wasBaby)
    {
        // Penalty for letting sphere reach edge
        score -= edgePenalty;
        if (wasBaby)
        {
            lives--; // Extra penalty for letting baby escape?? How should players lose? Penalites??
        }

        if (score < 0) score = 0;

        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (livesText != null)
            livesText.text = "Lives: " + lives;

        if (waveText != null && spawner != null)
            waveText.text = "Wave: " + spawner.currentWave;
    }

    void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);
        Time.timeScale = 0;

        //what should game over UI look like??
    }
}
