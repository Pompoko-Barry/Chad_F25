using UnityEngine;
using UnityEngine.AI;

public class ElfSpawnerManager : MonoBehaviour
{
    public static ElfSpawnerManager Instance;

    [Header("Elf Settings")]
    public GameObject elfPrefab;
    public int minElvesPerWave = 1;
    public int maxElvesPerWave = 3;

    [Header("Spawn Timing")]
    public float timeBetweenElves = 8f; // Time between elf spawns
    public float spawnRadius = 10f; // How far from center to spawn

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
            SpawnElf();
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

    void SpawnElf()
    {
        if (elfPrefab == null)
        {
            Debug.LogWarning("Elf prefab not assigned!");
            return;
        }

        //// Spawn at random position around the play area
        //Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;

        //float spawnHeight = 2f;
        //Vector3 spawnPos = new Vector3(randomCircle.x + 17f, spawnHeight, randomCircle.y+ 3f); // Elves are spawning below the play area, tweak this so they are in the right spot
        Vector3 center = new Vector3(19.7f, 1f, 3f);  
        Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = center + new Vector3(circle.x, 1f, circle.y);

        //GameObject elf = Instantiate(elfPrefab, spawnPos, Quaternion.identity, null);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPos, out hit, 10f, NavMesh.AllAreas))
        {
            spawnPos = hit.position; // snap to the navmesh
        }
        else
        {
            Debug.LogWarning("No NavMesh found near " + spawnPos);
        }

        Debug.Log("Elf spawned at: " + spawnPos + " | Should be at Y=2.5");
    }

    public void OnWaveEnd()
    {
        // Destroy all remaining elves when wave ends
        ElfController[] remainingElves = FindObjectsByType<ElfController>(FindObjectsSortMode.None);
        foreach (ElfController elf in remainingElves)
        {
            Destroy(elf.gameObject);
        }
    }
}
