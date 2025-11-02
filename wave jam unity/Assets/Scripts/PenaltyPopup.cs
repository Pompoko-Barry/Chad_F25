using UnityEngine;
using TMPro;

public class PenaltyPopup : MonoBehaviour
{
    public TextMeshProUGUI penaltyText;
    public float lifetime = 1.5f;
    public float floatSpeed = 2f;
    private float timer = 0f;

    void Update()
    {
        // Float upward
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade out
        timer += Time.deltaTime;
        float alpha = 1 - (timer / lifetime);
        penaltyText.color = new Color(penaltyText.color.r, penaltyText.color.g, penaltyText.color.b, alpha);

        // Destroy when done
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetPenalty(int penalty)
    {
        penaltyText.text = penalty.ToString();
    }

    public void SetPenalty(string text)
    {
        penaltyText.text = text;
    }
}
