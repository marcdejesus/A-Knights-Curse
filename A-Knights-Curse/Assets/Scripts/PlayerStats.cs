using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;  // Added for IEnumerator

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 10;
    public float moveSpeed = 5f;
    public float invulnerabilityTime = 1.5f;
    private bool isInvulnerable = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        if (isInvulnerable) return;

        currentHealth -= dmg;
        StartCoroutine(Invulnerability());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        // Optional: add flash animation or transparency effect
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    void Die()
    {
        SceneManager.LoadScene("GameplayScene"); // Respawn on death
    }
}
