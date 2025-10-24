using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DistanceDisplay : MonoBehaviour
{
    public Transform transformB;

    public Transform transformA1;
    public Transform transformA2;
    public Transform transformA3;

    public TextMeshProUGUI textComponentForDistance;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        if (textComponentForDistance == null)

            textComponentForDistance = GetComponent<TextMeshProUGUI>();

        textComponentForDistance.text = "Distance from Target for T-Sphere 1/3: Unknown";

        //for line renderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.grey;
        lineRenderer.endColor = Color.red;
        lineRenderer.enabled = false;
    }

    private void Update()
    {

        // //check which object is active, and then show its distance from object B
        // if (transformA1 != null && transformA1.gameObject.activeInHierarchy)
        // {
        //     textComponentForDistance.text =
        //             "Distance from Target for T-Sphere 1/3: " + Vector3.Distance(transformA1.position, transformB.position).ToString("F2");
        //     //Debug.DrawLine(transformA1.position, transformB.position, Color.red);
        // }
        //else if (transformA2 != null && transformA2.gameObject.activeInHierarchy)
        // {
        //     textComponentForDistance.text =
        //         "Distance from Target for T-Sphere 2/3: " + Vector3.Distance(transformA2.position, transformB.position).ToString("F2");
        // }
        // else if (transformA3 != null && transformA3.gameObject.activeInHierarchy)
        // {
        //     textComponentForDistance.text =
        //         "Distance from Target for T-Sphere 3/3: " + Vector3.Distance(transformA3.position, transformB.position).ToString("F2");
        // }
        // else
        // {
        //     textComponentForDistance.text = "No currently avaliable T-Spheres.";
        // }
        Transform activeT_Sphere = null;
        string label = "";

        //determine active sphere
        if (transformA1 != null && transformA1.gameObject.activeInHierarchy)
        {
            activeT_Sphere = transformA1;
            label = "T-Sphere 1/3";
        }
        else if (transformA2 != null && transformA2.gameObject.activeInHierarchy)
        {
            activeT_Sphere = transformA2;
            label = "T-Sphere 2/3";
        }
        else if (transformA3 != null && transformA3.gameObject.activeInHierarchy)
        {
            activeT_Sphere = transformA3;
            label = "T-Sphere 3/3";
        }

        if (activeT_Sphere != null && transformB != null)
        {
            float distance = Vector3.Distance(activeT_Sphere.position, transformB.position);
            textComponentForDistance.text = $"Distance from Target for {label}: {distance:F2}";

            //update LineRenderer to draw between T-Sphere and Target
            lineRenderer.SetPosition(0, activeT_Sphere.position);
            lineRenderer.SetPosition(1, transformB.position);
            lineRenderer.enabled = true;
        }
        else
        {
            textComponentForDistance.text = "No currently available T-Spheres.";
            lineRenderer.enabled = false;
        }
    }

}
