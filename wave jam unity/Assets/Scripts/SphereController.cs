using UnityEngine;

public class SphereController : MonoBehaviour
{
    [Header("Sphere Type")]
    public bool isBaby = true;

    public bool isPorkPine = false; // NEW DANGER KIND ODOODODODOD

    [Header("Movement")] //wth is a header??
    public float rollForce = 5f;
    public Vector3 voidPosition; // Where the void is

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;

    private Rigidbody rb;
    private bool hasBeenLaunched = false;
    private bool isBeingDragged = false;
    private Vector3 dragOffset;
    private Camera mainCamera;
    private Quaternion dragStartRotation;

    public AudioClip clickSound;
    private AudioSource audioSource;

    [Header("Correct Sort Effects")]
    public ParticleSystem correctSortParticles;
    public AudioClip correctSortSound;


    [Header("Incorrect Sort Effects")]
    public AudioClip IncorrectSortSound;
    public GameObject penaltyPopupPrefab;

    [Header("Pork-Pine Effects")]
    public GameObject lifeLostPopupPrefab;
    public AudioClip porkPineHurtSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        Invoke("LaunchFromVoid", 0.2f);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void LaunchFromVoid()
    {
        if (rb != null && !hasBeenLaunched)
        {
            Vector3 direction = (transform.position - voidPosition).normalized;
            direction.y = 0;
            rb.AddForce(direction * rollForce, ForceMode.Impulse);
            hasBeenLaunched = true;
        }
    }

    void OnMouseDown()
    {
        if (isPorkPine)
        {
            Debug.Log("OUCH! Touched a pork-pine! -1 life");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.TouchedPorkPine();
            }

            PlayPorkPineEffects();

            // Disable interaction
            isBeingDragged = false;
            GetComponent<Collider>().enabled = false;

            Destroy(gameObject, 0.8f);
            return;
        }

        isBeingDragged = true;
        dragStartRotation = transform.rotation;

        if (rb != null)
        {
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Vector3 mousePos = GetMouseWorldPosition();
        dragOffset = transform.position - mousePos;

        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    void Update()
    {
        if (isBeingDragged && Input.GetMouseButtonUp(0))
        {
            ReleaseSphere();
        }

        if (isBeingDragged)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            transform.position = mousePos + dragOffset;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                transform.Rotate(Vector3.right, scroll * rotationSpeed, Space.World);
            }
        }
    }

    void ReleaseSphere()
    {
        isBeingDragged = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sphere hit: " + other.gameObject.name + " with tag: " + other.tag);

        if (other.CompareTag("Edge"))
        {
            if (isPorkPine)
            {
                Debug.Log("Pork-pine escaped safely!");
                Destroy(gameObject);
                return; // No penalty!
            }

            Debug.Log("Sphere escaped! -30 points");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SphereReachedEdge(isBaby);
                GameManager.Instance.ShowEdgePenalty(); // Use UI instead
            }

            Destroy(gameObject, 0.3f);
        }

        if (!isBeingDragged && other.CompareTag("BabyZone"))
        {

            if (isPorkPine)
            {
                Debug.Log("Can't sort pork-pines!");
                return;
            }

            if (isBaby)
            {
                Debug.Log("Correct! Baby Sorted!");
                GameManager.Instance.CorrectSort(true);
                PlayCorrectSortEffects();
            }
            else
            {
                Debug.Log("Wrong! This a rito!");
                GameManager.Instance.WrongSort(false);
                PlayIncorrectSortEffects();
            }
            Destroy(gameObject, 0.5f);
        }

        if (!isBeingDragged && other.CompareTag("BurritoZone"))
        {
            if (isPorkPine)
            {
                Debug.Log("Can't sort pork-pines!");
                return;
            }

            if (!isBaby)
            {
                Debug.Log("Correct! Burrito sorted!");
                GameManager.Instance.CorrectSort(false);
                PlayCorrectSortEffects();
            }
            else
            {
                Debug.Log("Wrong! Baby in burrito zone!");
                GameManager.Instance.WrongSort(true);
                PlayIncorrectSortEffects();
            }
            Destroy(gameObject, 0.5f);
        }
    }

    public bool IsBeingDragged()
    {
        return isBeingDragged;
    }

    //void ShowEdgePenaltyPopup(int penalty)
    //{

    //    Debug.Log("ShowEdgePenaltyPopup called with penalty: " + penalty);
    //    Debug.Log("penaltyPopupPrefab is null? " + (penaltyPopupPrefab == null));
    //    Debug.Log("Camera.main is null? " + (Camera.main == null));

    //    if (penaltyPopupPrefab != null && Camera.main != null)
    //    {
    //        Vector3 popupPosition = new Vector3(0, 2, 0);

    //        Debug.Log("Spawning popup at: " + popupPosition);



    //        Debug.Log("Spawning popup at: " + popupPosition);

    //        GameObject popup = Instantiate(penaltyPopupPrefab, popupPosition, Quaternion.identity);

    //        // Make it face the camera
    //        Vector3 directionToCamera = Camera.main.transform.position - popup.transform.position;
    //        popup.transform.rotation = Quaternion.LookRotation(-directionToCamera);

    //        PenaltyPopup popupScript = popup.GetComponent<PenaltyPopup>();

    //        if (popupScript != null)
    //        {
    //            popupScript.SetPenalty(penalty);
    //            Debug.Log("SetPenalty called with: " + penalty);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Cannot show popup - prefab or camera is null!");
    //    }
    //}

    void PlayCorrectSortEffects()
    {
        if (correctSortParticles != null)
        {
            ParticleSystem particles = Instantiate(correctSortParticles, transform.position, Quaternion.identity);
            Destroy(particles.gameObject, 2f);
        }

        if (correctSortSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(correctSortSound);
        }
    }

    void PlayIncorrectSortEffects()
    {
        if (IncorrectSortSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(IncorrectSortSound);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.StartShake(0.425f, 1f);
        }

        StartCoroutine(FlashRed());
        ShowPenaltyPopup(-15);
    }

    void ShowPenaltyPopup(int penalty)
    {
        if (penaltyPopupPrefab != null)
        {
            Vector3 popupPosition = transform.position + Vector3.up * 2f;
            GameObject popup = Instantiate(penaltyPopupPrefab, popupPosition, Quaternion.identity);

            if (Camera.main != null)
            {
                popup.transform.LookAt(popup.transform.position + Camera.main.transform.rotation * Vector3.forward,
                                       Camera.main.transform.rotation * Vector3.up);
            }

            PenaltyPopup popupScript = popup.GetComponent<PenaltyPopup>();
            if (popupScript != null)
            {
                popupScript.SetPenalty(penalty);
            }
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            renderer.material.color = Color.red;

            yield return new WaitForSeconds(0.3f);

            float elapsed = 0f;
            float duration = 0.2f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                renderer.material.color = Color.Lerp(Color.red, originalColor, elapsed / duration);
                yield return null;
            }
        }
    }

    void PlayPorkPineEffects()
    {
        if (porkPineHurtSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(porkPineHurtSound);
        }
        else if (IncorrectSortSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(IncorrectSortSound);
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.StartShake(0.9f, 1.2f);
            CameraShake.Instance.FlashScreen(0.8f, Color.red);
        }

        ShowLifeLostPopup();
    }

    void ShowLifeLostPopup()
    {
        if (lifeLostPopupPrefab != null)
        {
            Vector3 popupPosition = transform.position + Vector3.up * 2f;
            GameObject popup = Instantiate(lifeLostPopupPrefab, popupPosition, Quaternion.identity);

            if (Camera.main != null)
            {
                popup.transform.LookAt(popup.transform.position + Camera.main.transform.rotation * Vector3.forward,
                                       Camera.main.transform.rotation * Vector3.up);
            }

            LifeLostPopup popupScript = popup.GetComponent<LifeLostPopup>();
            if (popupScript != null)
            {
                popupScript.SetText("-1 LIFE!");
            }
        }
    }
}