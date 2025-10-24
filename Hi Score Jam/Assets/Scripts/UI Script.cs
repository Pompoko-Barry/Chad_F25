using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    public TextMeshProUGUI scoreCounter;
    public GameObject loseHandle;

    private ScoreManager score;
    private bool gameEnded = false;

    //for distance based score
    public TextMeshProUGUI accuracyScoreText;

    void Start()
    {
        loseHandle.SetActive(false);

        score = FindAnyObjectByType<ScoreManager>();

        if (score == null)
        {
            Debug.Log("ScoreManager script not found in the scene!");
        }
    }

    void Update()
    {
        if (score == null) return;

        scoreCounter.text = $"Times Target Hit: {score.targetsCollided}";

        if (accuracyScoreText != null)
        {
            accuracyScoreText.text = $"Accuracy Grading Not Properly Implemented {score.pointsEarned}";
        }

        // Check for lose condition
        if (!gameEnded && score.spheresUsed >= 3 && AllSpheresGone())
        {
            gameEnded = true;

            if (score.targetsCollided == 0)
            {
                Debug.Log("Lose condition triggered!");
                loseHandle.SetActive(true);
            }
        }
    }

    bool AllSpheresGone()
    {
        GameObject[] spheres = GameObject.FindGameObjectsWithTag("Sphere");

        foreach (GameObject sphere in spheres)
        {
            if (sphere.activeInHierarchy)
            {
                return false; // If even one is still active, return false
            }
        }

        return true; // All spheres are gone
    }
}
