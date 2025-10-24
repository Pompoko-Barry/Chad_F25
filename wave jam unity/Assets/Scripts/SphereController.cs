using UnityEngine;

public class SphereController : MonoBehaviour
{
    [Header("Sphere Type")]
    public bool isBaby = true;

    [Header("Movement")] //wth is a header??
    public float rollForce = 5f;
    public Vector3 voidPosition; // Where the void is

    private Rigidbody rb;
    private bool hasBeenLaunched = false;
    private bool isBeingDragged = false;
    private Vector3 dragOffset;
    private Camera mainCamera;


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

        //// This will stop physics while sphere is being dragged by mouse
        //if (rb != null)
        //{
        //    rb.linearVelocity = Vector3.zero;
        //    rb.angularVelocity = Vector3.zero;
        //    rb.useGravity = false;
        //    rb.isKinematic = true;
            
        //}

        // Calculate the offset from mouse to sphere : wat is offset ?
        Vector3 mousePos = GetMouseWorldPosition();
        dragOffset = transform.position - mousePos;
    }

    void OnMouseDrag()
    {
        if (isBeingDragged)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            transform.position = mousePos + dragOffset;
        }
    }

    void OnMouseUp()
    {
        // Stop dragging
        isBeingDragged = false;

        // Turn on physics again
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = true;
        }

        // How/where do I add the logic for checking if the sphere got dropped on the right spot?
        // Also, what happens if the sphere gets dropped somewhere there is no sorting zone?? (a blank spot on the playable area)
    }

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
