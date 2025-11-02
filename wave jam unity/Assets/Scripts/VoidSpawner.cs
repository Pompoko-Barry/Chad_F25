using UnityEngine;

public class VoidSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject spherePrefab;
    public Material[] babyMaterials;
    public Material[] burritoMaterials;
    public Material[] porkPineMaterials;


    [Header("Material Progression")]
    public int materialsPerWave = 2;
    public int newMaterialEveryXWaves = 2;

    private int currentBabyMaterialCount = 2;
    private int currentBurritoMaterialCount = 2;
    private int currentPorkPineMaterialCount = 2;


    [Header("Spawn Timing")]
    public float spawnInterval = 2f; // Time inbetween spawns
    public int spheresPerWave = 10;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public float babySpawnChance = 0.45f; // Reduced to make room for pork-pine
    public float porkPineSpawnChance = 0.1f;
    public float waveTimer = 1f;  //wave duration in seconds
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
         Random.Range(-2f, 0.5f), // These are the values that determine where the spheres will spawn within the void zone
         1f,
         Random.Range(-2f, 0.5f)
     );

        GameObject sphere = Instantiate(spherePrefab, spawnPos, Random.rotation);

        // Determine type with pork-pine option
        float roll = Random.value;
        bool isPorkPine = roll < porkPineSpawnChance;
        bool isBaby = !isPorkPine && roll < (porkPineSpawnChance + babySpawnChance);

        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        SphereController controller = sphere.GetComponent<SphereController>();

        if (sphereRenderer != null)
        {
            if (isPorkPine && porkPineMaterials.Length > 0)
            {
                int maxIndex = Mathf.Min(currentPorkPineMaterialCount, porkPineMaterials.Length);
                Material randomPorkPineMat = porkPineMaterials[Random.Range(0, maxIndex)];
                sphereRenderer.material = randomPorkPineMat;
            }
            else if (isBaby && babyMaterials.Length > 0)
            {
                int maxIndex = Mathf.Min(currentBabyMaterialCount, babyMaterials.Length);
                Material randomBabyMat = babyMaterials[Random.Range(0, maxIndex)];
                sphereRenderer.material = randomBabyMat;
            }
            else if (burritoMaterials.Length > 0)
            {
                int maxIndex = Mathf.Min(currentBurritoMaterialCount, burritoMaterials.Length);
                Material randomBurritoMat = burritoMaterials[Random.Range(0, maxIndex)];
                sphereRenderer.material = randomBurritoMat;
            }
        }

        if (controller != null)
        {
            controller.isBaby = isBaby;
            controller.isPorkPine = isPorkPine;
            controller.voidPosition = transform.position;
        }

        Debug.Log("Spawned " + (isPorkPine ? "PORK-PINE!" : (isBaby ? "Baby" : "Burrito")));
    }

    void EndWave()
    {

        // Clean up elves
        if (ElfSpawnerManager.Instance != null)
        {
            ElfSpawnerManager.Instance.OnWaveEnd();
        }

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

        // Notify elf spawner about new wave
        if (ElfSpawnerManager.Instance != null)
        {
            ElfSpawnerManager.Instance.OnWaveStart(currentWave);
        }

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
        spawnInterval = Mathf.Max(0.7f, spawnInterval - 0.1f); // Spawn faster
        spheresPerWave += 5; // More spheres per wave

        Debug.Log("Wave " + currentWave + " started!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateWaveDisplay(currentWave);
        }
    }
}

