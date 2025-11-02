using UnityEngine;
using UnityEngine.AI;

public class SpawnElf : MonoBehaviour
{
    public static SpawnElf Instance;

    [Header("Elf Settings")]
    public GameObject elfPrefab;
    public int minElvesPerWave = 1;
    public int maxElvesPerWave = 3;

    [Header("Spawn Timing")]
    public float timeBetweenElves = 8f; // Time between elf spawns
    public float spawnRadius = 10f;     // How far from center to spawn
    public Vector3 playAreaCenter = new Vector3(19.7f, 1f, 3f); // Center of playable area

    [Header("Wave Progression")]
    public int startElfWave = 3; // Elves start appearing at wave 3

    private int currentWave = 1;
    private bool canSpawnElves = false;
    private float elfSpawnTimer = 0f;
    private int elvesSpawnedThisWave = 0;
    private int elvesToSpawnThisWave = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!canSpawnElves || elvesSpawnedThisWave >= elvesToSpawnThisWave)
            return;

        elfSpawnTimer += Time.deltaTime;

        if (elfSpawnTimer >= timeBetweenElves)
        {
            SpawnElves(); // call the function here
            elfSpawnTimer = 0f;
            elvesSpawnedThisWave++;
        }
    }

    public void OnWaveStart(int waveNumber)
    {
        currentWave = waveNumber;
        elvesSpawnedThisWave = 0;
        elfSpawnTimer = 0f;

        // Check if this wave should have elves
        if (currentWave >= startElfWave)
        {
            canSpawnElves = true;

            // Calculate how many elves for this wave
            int baseElves = currentWave - startElfWave + 1;
            elvesToSpawnThisWave = Mathf.Clamp(baseElves, minElvesPerWave, maxElvesPerWave);

            Debug.Log($"Wave {currentWave}: Spawning {elvesToSpawnThisWave} elves!");
        }
        else
        {
            canSpawnElves = false;
            elvesToSpawnThisWave = 0;
        }
    }

    public void OnWaveEnd()
    {
        canSpawnElves = false;
        elvesToSpawnThisWave = 0;

        // Destroy all remaining elves
        ElfController[] remainingElves = FindObjectsByType<ElfController>(FindObjectsSortMode.None);
        foreach (ElfController elf in remainingElves)
        {
            Destroy(elf.gameObject);
        }
    }

    void SpawnElves()
    {
        if (elfPrefab == null)
        {
            Debug.LogWarning("Elf prefab not assigned!");
            return;
        }

        // === 1. Pick a random position within the radius around the play area ===
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = playAreaCenter + new Vector3(randomCircle.x, 0.1f, randomCircle.y); // slight Y offset

        // === 2. Snap to NavMesh ===
        NavMeshHit hit;
        float sampleRadius = 20f;
        if (NavMesh.SamplePosition(spawnPos, out hit, sampleRadius, NavMesh.AllAreas))
        {
            spawnPos = hit.position;
        }
        else
        {
            Debug.LogWarning("No NavMesh found near " + spawnPos + ". Using play area center as fallback.");
            spawnPos = playAreaCenter;
        }

        // === 3. Instantiate the elf ===
        GameObject elf = Instantiate(elfPrefab, spawnPos, Quaternion.identity);

        // Optional: random Y rotation
        elf.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        // === 4. Ensure NavMeshAgent is correctly placed ===
        NavMeshAgent agent = elf.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(spawnPos); // snap agent exactly to NavMesh
        }

        Debug.Log("Elf spawned on NavMesh at: " + spawnPos);
    }
}