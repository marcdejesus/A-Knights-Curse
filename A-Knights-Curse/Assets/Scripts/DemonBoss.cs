using UnityEngine;

public class DemonBoss : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float shootInterval = 3f;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    private float fireTimer;
    private float summonTimer;

    void Start()
    {
        fireTimer = shootInterval;
        summonTimer = 5f;
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;
        summonTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            ShootFireball();
            fireTimer = shootInterval;
        }

        if (summonTimer <= 0f)
        {
            SummonEnemies();
            summonTimer = 5f;
        }
    }

    void ShootFireball()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            fireball.GetComponent<Rigidbody2D>().velocity = direction * 6f;
        }
    }

    void SummonEnemies()
    {
        for (int i = 0; i < 2; i++)
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
