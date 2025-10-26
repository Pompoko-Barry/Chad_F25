using UnityEngine;

public class SphereController : MonoBehaviour
{
    [Header("Sphere Type")]
    public bool isBaby = true;

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


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        // Launch the sphere away from the void after a short delay
        Invoke("LaunchFromVoid", 0.2f);
    }


   void LaunchFromVoid()
    {
        if (rb != null && !hasBeenLaunched)
        {
            // This formula thing will calculate direction away from void
            Vector3 direction = (transform.position - voidPosition).normalized;

            // Keep it on the horizontal plane to prevent jumping movements
            direction.y = 0;

            // Add force to have spheres roll away
            rb.AddForce(direction * rollForce, ForceMode.Impulse);

            hasBeenLaunched = true;
        }
    }

    void OnMouseDown()
    {
        isBeingDragged = true;

        // Store the current rotation
        dragStartRotation = transform.rotation;

        // Stop physics while dragging
        if (rb != null)
        {
            // Only set velocities if NOT kinematic
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Then make kinematic and disable gravity
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        //Calculate the offset from mouse to sphere: wat is offset ?
        Vector3 mousePos = GetMouseWorldPosition();
        dragOffset = transform.position - mousePos;
    }

    void Update()
    {
        // Check if dragging and mouse button is released
        if (isBeingDragged && Input.GetMouseButtonUp(0))
        {
            ReleaseSphere();
        }

        // Handle dragging movement
        if (isBeingDragged)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            transform.position = mousePos + dragOffset;

            // Rotate with scroll
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                transform.Rotate(Vector3.up, scroll * rotationSpeed, Space.World);
            }
        }
    }

    void ReleaseSphere()
    {
        // Stop dragging
        isBeingDragged = false;

        // Re-enable physics
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    //void OnMouseDrag()
    //{
    //    if (isBeingDragged)
    //    {
    //        Vector3 mousePos = GetMouseWorldPosition();
    //        transform.position = mousePos + dragOffset;

    //        // To rotate sphere with scroll wheel
    //        float scroll = Input.GetAxis("Mouse ScrollWheel");
    //        if (scroll != 0f)
    //        {
    //            transform.Rotate(Vector3.up, scroll * rotationSpeed, Space.World); 
    //        }
    //    }
    //}

    //void OnMouseUp()
    //{
    //    // Stop dragging
    //    isBeingDragged = false;

    //    // Turn on physics again so that physics and gravity will cause the sphere to drop
    //    if (rb != null)
    //    {
    //        rb.useGravity = true;
    //        rb.isKinematic = true;

    //        rb.linearVelocity = Vector3.zero;
    //        rb.angularVelocity = Vector3.zero;
    //    }

    //    // How/where do I add the logic for checking if the sphere got dropped on the right spot?
    //    // Also, what happens if the sphere gets dropped somewhere there is no sorting zone?? (a blank spot on the playable area)
    //}

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    // This is called when sphere reaches edgeΩ (tag a collider with "Edge" or it will not work)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Edge"))
        {
            if (isBaby)
            {
                // The Baby escapeddd!!!! Lose points or life????
                Debug.Log("baby escaped!! -1 life");

                GameManager.Instance.SphereReachedEdge(true);
            }
            else
            {
                // Burrito Escaped -- uh oh
                Debug.Log("Burrito escaped! sadge!");

                GameManager.Instance.SphereReachedEdge(false);
            }

            // Maybe have this be hide instead if destoyiong starts to create some prbem
            Destroy(gameObject);
            
        }

        // Check for sorting zones when dropped
        if (!isBeingDragged && other.CompareTag("BabyZone"))
        {
            if (isBaby)
            {
                Debug.Log("Correct! Baby Sorted!");
                GameManager.Instance.CorrectSort(true);
            }
            else
            {
                Debug.Log("Wrong! This a rito!");
                GameManager.Instance.WrongSort(false);
            }
            Destroy(gameObject);
        }

        if (!isBeingDragged && other.CompareTag("BurritoZone"))
        {
            if (!isBaby)
            {
                Debug.Log("Correct! Burrito sorted!");
                GameManager.Instance.CorrectSort(false);
            }

            else
            {
                Debug.Log("Wrong! Baby in burrito zone!");
                GameManager.Instance.WrongSort(true);
            }
            Destroy(gameObject);
        }
    }
 
}
