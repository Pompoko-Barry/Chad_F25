using UnityEngine;

public class ElfController : MonoBehaviour
{
    [Header("Elf Settings")]
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;
    public float eatRadius = 1f;

    [Header("Audio")]
    public AudioClip elfLaughSound;
    public AudioClip elfClickSound;

    [Header("Visual Effects")]
    public ParticleSystem clickParticles;

    private Transform targetSphere;
    private Camera mainCamera;
    private AudioSource audioSource;
    private bool isActive = true;

    void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;

        FindNearestSphere();
    }

    void Update()
    {
        if (!isActive) return;

        // Bouncy animation
        float bounce = Mathf.Sin(Time.time * 5f) * 0.1f;
        transform.position = new Vector3(transform.position.x, 0.5f + bounce, transform.position.z);

        // If no target or target destroyed, find new one
        if (targetSphere == null)
        {
            FindNearestSphere();
        }

        // Move toward target sphere
        if (targetSphere != null)
        {
            Vector3 direction = (targetSphere.position - transform.position).normalized;
            direction.y = 0; // Stay on ground

            transform.position += direction * moveSpeed * Time.deltaTime;

            // Face the target
            transform.LookAt(new Vector3(targetSphere.position.x, transform.position.y, targetSphere.position.z));

            // Check if close enough to eat
            float distanceToSphere = Vector3.Distance(transform.position, targetSphere.position);
            if (distanceToSphere <= eatRadius)
            {
                EatSphere();
            }
        }
    }

    void FindNearestSphere()
    {
        SphereController[] allSpheres = FindObjectsByType<SphereController>(FindObjectsSortMode.None);
        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (SphereController sphere in allSpheres)
        {
            // Don't target spheres being dragged or pork-pines
            if (sphere.IsBeingDragged() || sphere.isPorkPine)
                continue;

            float distance = Vector3.Distance(transform.position, sphere.transform.position);
            if (distance < detectionRadius && distance < closestDistance)
            {
                closestDistance = distance;
                closest = sphere.transform;
            }
        }

        targetSphere = closest;
    }

    void EatSphere()
    {
        if (targetSphere != null)
        {
            Debug.Log("Elf ate a sphere!");

            // Play laugh sound
            if (elfLaughSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(elfLaughSound);
            }

            // Destroy the sphere
            Destroy(targetSphere.gameObject);
            targetSphere = null;

            // Find next target
            FindNearestSphere();
        }
    }

    void OnMouseDown()
    {
        // Player clicked on elf - destroy it!
        if (isActive)
        {
            Debug.Log("Elf clicked and removed!");

            if (elfClickSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(elfClickSound);
            }

            // Spawn particles
            if (clickParticles != null)
            {
                ParticleSystem particles = Instantiate(clickParticles, transform.position, Quaternion.identity);
                Destroy(particles.gameObject, 2f);
            }

            isActive = false;

            // Visual feedback
            StartCoroutine(DestroyElf());
        }
    }

    System.Collections.IEnumerator DestroyElf()
    {
        // Shrink animation
        float elapsed = 0f;
        float duration = 0.3f;
        Vector3 startScale = transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
