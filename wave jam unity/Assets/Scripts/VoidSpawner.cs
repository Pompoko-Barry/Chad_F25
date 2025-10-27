using UnityEngine;

public class VoidSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject spherePrefab;
    public Material[] babyMaterials;
    public Material[] burritoMaterials;


    [Header("Material Progression")]
    public int materialsPerWave = 2;
    public int newMaterialEveryXWaves = 2;

    private int currentBabyMaterialCount = 2;
    private int currentBurritoMaterialCount = 2;


    [Header("Spawn Timing")]
    public float spawnInterval = 2f; // Time inbetween spawns
    public int spheresPerWave = 10;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public float babySpawnChance = 0.5f; // 50% chance for baby1 spawn
    public float waveTimer = 30f;  //wave duration in seconds
    public float wavePauseTime = 3f;  // To pause in between waves

    private float spawnTimer = 0f;
    private int spheresSpawnedThisWave = 0;
    private float currentWaveTimeRemaining;
    private bool isWavePaused = false;


    void Start()
    {
        currentWaveTimeRemaining = waveTimer;
    }

    void Update()
    {

        if (isWavePaused)
            return;  // All gameplay is paused during wave transition period

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
        bool isBaby = Random.value < babySpawnChance;

        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        SphereController controller = sphere.GetComponent<SphereController>();

        if (sphereRenderer != null)
        {
            if (isBaby && babyMaterials.Length > 0)
            {
                // Only pick from unlocked materials
                int maxIndex = Mathf.Min(currentBabyMaterialCount, babyMaterials.Length);
                Material randomBabyMat = babyMaterials[Random.Range(0, maxIndex)];
                sphereRenderer.material = randomBabyMat;
            }
            else if (!isBaby && burritoMaterials.Length > 0)
            {
                int maxIndex = Mathf.Min(currentBurritoMaterialCount, burritoMaterials.Length);
                Material randomBurritoMat = burritoMaterials[Random.Range(0, maxIndex)];
                sphereRenderer.material = randomBurritoMat;
            }
        }

        if (controller != null)
        {
            controller.isBaby = isBaby;
            controller.voidPosition = transform.position;
        }

        Debug.Log("Spawned " + (isBaby ? "Baby" : "Burrito"));
    }

    void EndWave()
    {

        //Destroy all remaining spheres 
        SphereController[] remainingSpheres = FindObjectsByType<SphereController>(FindObjectsSortMode.None);
        foreach (SphereController sphere in remainingSpheres)
        {
            Destroy(sphere.gameObject);
        }

        isWavePaused = true;
        Invoke("StartNextWave", wavePauseTime);

        //StartNextWave();
    }

    public void StartNextWave()
    {
        isWavePaused = false;
        currentWave++;
        spheresSpawnedThisWave = 0;
        currentWaveTimeRemaining = waveTimer;

        // Unlock new materials every few waves
        if (currentWave % newMaterialEveryXWaves == 0)
            if (currentBabyMaterialCount < babyMaterials.Length)
            {
                currentBabyMaterialCount++;
            }
            if (currentBurritoMaterialCount < burritoMaterials.Length)
        {
            currentBurritoMaterialCount++;
        }
      

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

