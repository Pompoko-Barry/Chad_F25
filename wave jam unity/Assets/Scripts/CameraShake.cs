using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPosition;
    private float shakeTimeRemaining;
    private float shakePower;
    private float shakeFadeTime;

    [Header("Screen Flash")]
    public UnityEngine.UI.Image flashImage; // Assign a full-screen red image

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.localPosition = originalPosition + new Vector3(xAmount, yAmount, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    public void StartShake(float duration, float power)
    {
        originalPosition = transform.localPosition;
        shakeTimeRemaining = duration;
        shakePower = power;
        shakeFadeTime = power / duration;
    }

    public void FlashScreen(float duration, Color flashColor)
    {
        if (flashImage != null)
        {
            StartCoroutine(ScreenFlashCoroutine(duration, flashColor));
        }
    }

    System.Collections.IEnumerator ScreenFlashCoroutine(float duration, Color flashColor)
    {
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(true);

            // Flash in
            float elapsed = 0f;
            while (elapsed < duration * 0.3f)
            {
                elapsed += Time.deltaTime;
                float alpha = elapsed / (duration * 0.5f);
                flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha * 0.8f);
                yield return null;
            }

            // Flash out
            elapsed = 0f;
            while (elapsed < duration * 0.7f)
            {
                elapsed += Time.deltaTime;
                float alpha = 1f - (elapsed / (duration * 0.7f));
                flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha * 0.5f);
                yield return null;
            }

            flashImage.gameObject.SetActive(false);
        }
    }
}