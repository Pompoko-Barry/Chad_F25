using UnityEngine;
using TMPro;

public class LifeLostPopup : MonoBehaviour
{
    public TextMeshProUGUI lifeLostText;
    public float lifetime = 2f;
    public float floatSpeed = 1.5f;
    public float scaleSpeed = 3f;
    private float timer = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This will make the popup start small and then grow
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Have this popup float up slower than points penalty popup
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        float scale = Mathf.Sin(timer * scaleSpeed) * 0.3f + 1f;
        transform.localScale = Vector3.one * scale * Mathf.Min(timer * 3f, 1f);

        // Fade out in the last half second
        if (timer > lifetime - 0.5f)
        {
            float alpha = (lifetime - timer) / 0.5f;
            lifeLostText.color = new Color(lifeLostText.color.r, lifeLostText.color.g, lifeLostText.color.b, alpha);
        }

        // Destroy when done
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        lifeLostText.text = text;
    }
}
