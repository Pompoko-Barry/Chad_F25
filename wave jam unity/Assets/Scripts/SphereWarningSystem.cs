using UnityEngine;

public class SphereWarningSystem : MonoBehaviour
{
    [Header("Warning Settings")]
    public float warningDistance = 5f; // Distance from edge to trigger warning
    public GameObject warningIndicatorPrefab; // Yellow triangle prefab
    public AudioClip warningSound;

    [Header("Edge Detection")]
    public string edgeTag = "Edge";

    private GameObject currentWarningIndicator;
    private AudioSource audioSource;
    private bool isWarningActive = false;
    private bool hasPlayedWarning = false;
    private Transform edgeTransform;
    private SphereController sphereController;

    void Start()
    {
        sphereController = GetComponent<SphereController>();

        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;

        // Find the edge object
        GameObject edgeObject = GameObject.FindGameObjectWithTag(edgeTag);
        if (edgeObject != null)
        {
            edgeTransform = edgeObject.transform;
        }
    }

    void Update()
    {
        // Don't show warning if being dragged
        if (sphereController != null && sphereController.IsBeingDragged())
        {
            HideWarning();
            hasPlayedWarning = false;
            return;
        }

        CheckDistanceToEdge();

    }

    void CheckDistanceToEdge()
    {
        if (edgeTransform == null) return;

        // Calculate distance to edge (approximate using collider bounds)
        Collider edgeCollider = edgeTransform.GetComponent<Collider>();
        if (edgeCollider != null)
        {
            Vector3 closestPoint = edgeCollider.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);

            if (distance <= warningDistance && !isWarningActive)
            {
                ShowWarning(closestPoint);
            }
            else if (distance > warningDistance && isWarningActive)
            {
                HideWarning();
                hasPlayedWarning = false;
            }
        }
    }

    void ShowWarning(Vector3 edgePoint)
    {
        if (warningIndicatorPrefab == null) return;

        // Create warning indicator if it doesn't exist
        if (currentWarningIndicator == null)
        {
            currentWarningIndicator = Instantiate(warningIndicatorPrefab);
        }

        // Position warning indicator between sphere and edge
        Vector3 direction = (edgePoint - transform.position).normalized;
        Vector3 indicatorPosition = transform.position + direction * 2f; // 2 units ahead
        indicatorPosition.y = transform.position.y + 1f; // Slightly above sphere

        currentWarningIndicator.transform.position = indicatorPosition;

        // Rotate to point toward edge
        currentWarningIndicator.transform.LookAt(edgePoint);
        currentWarningIndicator.transform.Rotate(90, 0, 0); // Adjust rotation for triangle

        currentWarningIndicator.SetActive(true);
        isWarningActive = true;

        // Play warning sound once
        if (!hasPlayedWarning && warningSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(warningSound);
            hasPlayedWarning = true;
        }
    }

    void HideWarning()
    {
        if (currentWarningIndicator != null)
        {
            currentWarningIndicator.SetActive(false);
        }
        isWarningActive = false;
    }

    void OnDestroy()
    {
        // Clean up warning indicator when sphere is destroyed
        if (currentWarningIndicator != null)
        {
            Destroy(currentWarningIndicator);
        }
    }
}