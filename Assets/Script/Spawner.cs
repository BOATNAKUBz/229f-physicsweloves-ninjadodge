using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public float spawnRate = 1f;
    public float spawnRangeX = 8f;

    void Start()
    {
        InvokeRepeating("SpawnObject", 1f, spawnRate);
    }

    void SpawnObject()
    {
        int rand = Random.Range(0, objectsToSpawn.Length);

        Vector3 spawnPos = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX),
            transform.position.y,
            0
        );

        Instantiate(objectsToSpawn[rand], spawnPos, Quaternion.identity);
    }
}