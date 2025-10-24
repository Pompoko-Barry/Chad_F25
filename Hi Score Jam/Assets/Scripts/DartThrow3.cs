using System.Collections;
using UnityEngine;

public class DartThrow3 : MonoBehaviour
{
    private Rigidbody rb;
    public float impulse = 0f;
    public float maxImpulse = 20f;
    public float impulseRate = 10f;

    public float torque = 0.1f;

    public float thrust = 5f;

    private bool hasBeenThrown = false;

    private SetFalse setFalseScript;

    private void Awake()
    {
        setFalseScript = GetComponent<SetFalse>();
    }

    public void OnDartMoved()
    {
        if (setFalseScript != null)
        {
            setFalseScript.StartCountdown();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (!hasBeenThrown)
        {
            if (Input.GetButton("Fire1"))
            {
                impulse += impulseRate * Time.deltaTime;
                impulse = Mathf.Clamp(impulse, 0, maxImpulse);
            }

            if (Input.GetButtonUp("Fire1"))
            {
                rb.isKinematic = false;
                rb.AddRelativeForce(Vector3.forward * impulse, ForceMode.Impulse);
                hasBeenThrown = true;

                OnDartMoved();

                ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.spheresUsed += 1;
                }

                impulse = 0f;
            }
        }
        else
        {
            //directional force after left click aka initial throw
            if (Input.GetKeyDown(KeyCode.A))
            {
                rb.AddForce(Vector3.left * thrust, ForceMode.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                rb.AddForce(Vector3.right * thrust, ForceMode.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                rb.AddForce(Vector3.up * thrust, ForceMode.Impulse);
            }

            //stop all motion on Space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
