using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartManager : MonoBehaviour
{
    public List<GameObject> darts;
    public GameObject startingPromptPanel;
    public Button iUnderstandButton;

    private int currentDart = 0;
    private bool promptInteracted = false;


    //reference the score manager for win condition
    public ScoreManager scoreManager;
    public GameObject winButton;


    void Start()
    {
        foreach (GameObject dart in darts)
        {
            dart.SetActive(false);
            dart.GetComponent<SetFalse>().DartManager = this;
        }

        startingPromptPanel.SetActive(true);
        iUnderstandButton.onClick.AddListener(OnPromptInteracted);

        winButton.SetActive(false);
    }

    private void OnPromptInteracted()
    {
        promptInteracted = true;
        startingPromptPanel.SetActive(false);
        iUnderstandButton.gameObject.SetActive(false);
        darts[currentDart].SetActive(true);
    }

    public void ResetDart()
    {
        if (!promptInteracted) return;

        currentDart++;
        if (currentDart < darts.Count)
        {
            darts[currentDart].SetActive(true);
        }
        else
        {
            if (scoreManager != null && scoreManager.targetsCollided >= 1)
            {
                Debug.Log("Win condition met");
                winButton.SetActive(true);
            }
        }
    }
}