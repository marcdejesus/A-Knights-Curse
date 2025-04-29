using UnityEngine;

public class DemonBoss : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballInterval = 1f;
    public float summonInterval = 5f;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    private float fireballTimer;
    private float summonTimer;

    void Start()
    {
        fireballTimer = fireballInterval;
        summonTimer = summonInterval;
    }

    void Update()
    {
        fireballTimer -= Time.deltaTime;
        summonTimer -= Time.deltaTime;

        if (fireballTimer <= 0f)
        {
            ShootFireball();
            fireballTimer = fireballInterval; // reset timer
        }

        if (summonTimer <= 0f)
        {
            SummonEnemies();
            summonTimer = summonInterval; // reset timer
        }
    }

    void ShootFireball()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Vector2 direction = (player.transform.position - firePoint.position).normalized;
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            fireball.GetComponent<Fireball>().SetDirection(direction);
        }
    }

    void SummonEnemies()
    {
        for (int i = 0; i < 2; i++) // spawn 2 enemies at random points
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
