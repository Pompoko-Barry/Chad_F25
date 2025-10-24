using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    // Apple counter aka Score Counter
    public int targetsCollided = 0;
    public int spheresUsed = 0;

    public int pointsEarned = 0;


    //renderer to change colors
    public Renderer myRenderer;
    public Color hitColor = Color.blue;
    public float colorChangeDuration = 0.5f;

    private Color originalColor;



    //tag so that only tagged objects will color change
    //public GameObject targetObject;

    //reference to the sphere collider that is wihtin the cylinder collider
    public SphereCollider scoringZone;

    private void Start()
    {
        if (myRenderer != null)
        {
            originalColor = myRenderer.material.color;
        }
        else
        {
            Debug.Log("myRenderer is not assigned");
        }
    }

    //internal void RegisterHit()
    //{
    //    //throw new NotImplementedException();
    //}

    public void RegisterHit()
    {
        targetsCollided += 1;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Target"))
    //    {
    //        //add one to score
    //        targetsCollided += 1;
    //        Debug.Log($"Target hit! Total hits: {targetsCollided}");

    //        if (myRenderer != null)
    //        {
    //            StopAllCoroutines();
    //            myRenderer.material.color = hitColor;
    //            StartCoroutine(ResetColorAfterDelay());
    //        }
    //    }


    //}

    //private System.Collections.IEnumerator ResetColorAfterDelay()
    //{
    //    yield return new WaitForSeconds(colorChangeDuration);
    //    myRenderer.material.color = originalColor;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            {
                targetsCollided += 1;

                //Vector3 hitPoint = collision.contacts[0].point;

                //Vector3 sphereCenter = scoringZone.transform.TransformPoint(scoringZone.center);

                //float distance = Vector3.Distance(hitPoint, sphereCenter);
                //float radius = scoringZone.radius * scoringZone.transform.localScale.x;


                ////this is some function that turns closer distance into score value
                //float scoreFactor = Mathf.Clamp01(1 - (distance / radius));
                ////to scale score to 10 points
                //pointsEarned += Mathf.CeilToInt(scoreFactor * 10);


            }
        }
    }

    //public void RegisterHit()
    //{
    //    targetsCollided += 1;
    //}
}