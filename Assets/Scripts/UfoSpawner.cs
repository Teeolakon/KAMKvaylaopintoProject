using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpawner : MonoBehaviour
{
    public GameObject ufoPrefab;
    public Transform[] spawnPoints;
    public float initialSpawnInterval = 10f;
    public float minSpawnInterval = 2f;
    public float spawnRateIncrease = 0.1f;
    public int maxActiveUFOs = 3;

    private bool spawningActive = false;
    private float currentSpawnInterval;
    private List<GameObject> activeUFOs = new List<GameObject>();

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
    }

    public void ActivateSpawning()
    {
        if (!spawningActive)
        {
            spawningActive = true;
            StartCoroutine(SpawnLoop());
        }
    }

    IEnumerator SpawnLoop()
    {
        while (spawningActive)
        {
            TrySpawnUfo();
            yield return new WaitForSeconds(currentSpawnInterval);
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnRateIncrease);
        }
    }

    void Update()
    {
        // Continuously clean up destroyed UFOs, not just when spawning
        CleanupDestroyedUFOs();
    }

    void CleanupDestroyedUFOs()
    {
        int beforeCount = activeUFOs.Count;
        activeUFOs.RemoveAll(ufo => ufo == null);
        int afterCount = activeUFOs.Count;

        if (beforeCount != afterCount)
        {
            Debug.Log($"Cleaned up {beforeCount - afterCount} destroyed UFOs. Active UFOs: {afterCount}");
        }
    }

    void TrySpawnUfo()
    {
        CleanupDestroyedUFOs(); // Ensure cleanup before checking count

        if (activeUFOs.Count >= maxActiveUFOs) return;

        // Check if there are any available targets before spawning
        if (!HasAvailableTargets()) return;

        SpawnUfo();
    }

    bool HasAvailableTargets()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Abductable");
        int availableCount = 0;

        foreach (GameObject obj in potentialTargets)
        {
            Abductable abductable = obj.GetComponent<Abductable>();
            if (abductable != null && !abductable.isClaimed)
            {
                availableCount++;
            }
        }

        Debug.Log($"Available targets: {availableCount}, Active UFOs: {activeUFOs.Count}");
        return availableCount > 0;
    }

    void SpawnUfo()
    {
        if (spawnPoints.Length == 0 || ufoPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newUfo = Instantiate(ufoPrefab, spawnPoint.position, spawnPoint.rotation);
        activeUFOs.Add(newUfo);

        // Let the UFO find its own target using the claiming system
        UfoBehaviour behaviour = newUfo.GetComponent<UfoBehaviour>();
        if (behaviour == null)
        {
            Debug.LogWarning("UfoBehaviour component not found on the ufo prefab.");
        }
    }
}