using UnityEngine;

public class VoidSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject spherePrefab;
    public Material[] babyMaterials;
    public Material[] burritoMaterials;

    [Header("Spawn Timing")]
    public float spawnInterval = 2f; // Time inbetween spawns
    public int spheresPerWave = 10;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public float babySpawnChance = 0.5f; // 50% chance for baby1 spawn
    public float waveTimer = 30f;  //wave duration in seconds

    private float spawnTimer = 0f;
    private int spheresSpawnedThisWave = 0;
    private float currentWaveTimeRemaining;


    void Start()
    {
        currentWaveTimeRemaining = waveTimer;
    }

    void Update()
    {
        // Countdown wave timer
        currentWaveTimeRemaining -= Time.deltaTime;

        // Update UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateWaveTimer(currentWaveTimeRemaining);
        }

        // Check if wave time is up
        if (currentWaveTimeRemaining <= 0)
        {
            EndWave();
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && spheresSpawnedThisWave < spheresPerWave)
        {
            SpawnSphere();
            spawnTimer = 0f;
            spheresSpawnedThisWave++;
        }
    }

    void SpawnSphere()
    {
        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-0.5f, 0.5f),
            1f,
            Random.Range(-0.5f, 0.5f)
            );

        GameObject sphere = Instantiate(spherePrefab, spawnPos, Random.rotation);

        // This will determine type of sphere
        bool isBaby = Random.value < babySpawnChance;

        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        SphereController controller = sphere.GetComponent<SphereController>();

        // Pick a random material from the array
        if (sphereRenderer != null)
        {
            if (isBaby && babyMaterials.Length > 0)
            {
                Material randomBabyMat = babyMaterials[Random.Range(0, babyMaterials.Length)];
                sphereRenderer.material = randomBabyMat;
            }

            else if (!isBaby && burritoMaterials.Length > 0)
            {
                Material randomBurritoMat = burritoMaterials[Random.Range(0, burritoMaterials.Length)];
                sphereRenderer.material = randomBurritoMat;
            }
        }

        if (controller != null)
        {
            controller.isBaby = isBaby;
            controller.voidPosition = transform.position;
        }

        Debug.Log("Spawned " + (isBaby ? "Baby" : "Burrito"));
        //// Spawn at the position of void/hole but with a slight random offset
        //Vector3 spawnPos = transform.position + new Vector3(
        //    Random.Range (-0.5f, 0.5f),
        //    1f, // This will cause it to spawn slightly above
        //    Random.Range(-0.5f, 0.5f)
        //    );

        //// Create the sphere
        //GameObject sphere = Instantiate(spherePrefab, spawnPos, Random.rotation);

        //// Determine if baby or burrito
        //bool isBaby = Random.value < baby1SpawnChance;

        //// Get the sphere's components
        //Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        //SphereController controller = sphere.GetComponent<SphereController>();

        //// Assign material and type
        //if (sphereRenderer != null)
        //{
        //    Material materialToUse = isBaby ? baby1Material : burrito1Material;
        //    sphereRenderer.material = materialToUse;
        //    Debug.Log("Assigned material: " + (materialToUse != null ? materialToUse.name : "NULL"));
        //}
        //else
        //{
        //    Debug.LogWarning("No Renderer found on sphere!");
        //}


        //if (controller != null)
        //{
        //    controller.isBaby = isBaby;
        //    controller.voidPosition = transform.position;
        //}

        //Debug.Log("Spawned " + (isBaby ? "Baby" : "Burrito") + " sphere");

    }

    void EndWave()
    {
        SphereController[] remainingSpheres = FindObjectsByType<SphereController>(FindObjectsSortMode.None);
        foreach (SphereController sphere in remainingSpheres)
        {
            Destroy(sphere.gameObject);
        }

        StartNextWave();
    }

    public void StartNextWave()
    {
        currentWave++;
        spheresSpawnedThisWave = 0;
        currentWaveTimeRemaining = waveTimer;

        // Make waves harder
        spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.1f); // Spawn faster
        spheresPerWave += 5; // More spheres per wave

        Debug.Log("Wave " + currentWave + " started!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateWaveDisplay(currentWave);
        }
    }
}

