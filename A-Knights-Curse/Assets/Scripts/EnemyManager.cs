using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public GameObject batPrefab;
    public GameObject slimePrefab;

    public Transform[] spawnPoints;

    void Start()
    {
        InvokeRepeating("SpawnEnemies", 2f, 5f); // every 5 sec
    }

    void SpawnEnemies()
    {
        int randomEnemy = Random.Range(0, 3);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        if (randomEnemy == 0)
            Instantiate(skeletonPrefab, spawnPoint.position, Quaternion.identity);
        else if (randomEnemy == 1)
            Instantiate(batPrefab, spawnPoint.position, Quaternion.identity);
        else
            Instantiate(slimePrefab, spawnPoint.position, Quaternion.identity);
    }
}
