using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    // Object to be spawned
    [SerializeField] private AnimatedCurveObject objectToSpawnPrefab;

    [SerializeField] private float spawnFrecuency = 0.5f;
    [SerializeField] private int totalObjectsToSpawn = 20;

    private int numberObjectsSpawned = 0;
    void Start()
    {
        numberObjectsSpawned = 0;
        // Coroutine to spawn objects 
        StartCoroutine(SpawnAnimatedObjects());
    }
    private IEnumerator SpawnAnimatedObjects()
    {
        while (numberObjectsSpawned < totalObjectsToSpawn)
        {
            yield return new WaitForSeconds(spawnFrecuency);

            AnimatedCurveObject spawned =
                      Instantiate(objectToSpawnPrefab);
            if (spawned != null)
            {
                spawned.transform.parent = transform;
            }
            numberObjectsSpawned += 1;
        }
    }
}
