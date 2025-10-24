using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DartThrow2 : MonoBehaviour
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

    public void OnDartMoved ()
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

                //start the countdown to turn false now that dart has been activated/tjrown
                OnDartMoved();


                ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.spheresUsed += 1;
                }

                // Reset impulse for later use not part of initla throw
                impulse = 0f;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //Small push forward after throw
                rb.AddRelativeForce(Vector3.forward * thrust, ForceMode.Impulse);
            }
        }

        //upward force with space after throw
        if (Input.GetKeyDown(KeyCode.Space) && hasBeenThrown)
        {
            rb.AddForce(Vector3.up * thrust, ForceMode.Impulse);
        }
    }
}