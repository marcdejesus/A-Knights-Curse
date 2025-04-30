using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;  // Added for IEnumerator

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public float invulnerabilityTime = 1.5f;

    [Header("Regeneration Settings")]
    public float regenDelay = 3f;        // Time to wait before regeneration starts
    public int regenAmount = 2;          // Amount of health regenerated per tick
    public float regenTickRate = 0.1f;   // How often regeneration occurs (in seconds)

    [Header("Combat Stats")]
    public int damage = 10;
    public float moveSpeed = 5f;

    private bool isInvulnerable = false;
    private float lastDamageTime;
    private bool isRegenerating = false;

    void Start()
    {
        currentHealth = maxHealth;
        lastDamageTime = -regenDelay; // Allow immediate regen at start if needed
    }

    void Update()
    {
        // Check if we should start regenerating
        if (!isRegenerating && Time.time >= lastDamageTime + regenDelay)
        {
            StartCoroutine(RegenerateHealth());
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isInvulnerable) return;

        currentHealth -= dmg;
        lastDamageTime = Time.time;
        
        // Stop any current regeneration
        StopAllCoroutines();
        isRegenerating = false;
        
        // Start invulnerability
        StartCoroutine(Invulnerability());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator RegenerateHealth()
    {
        isRegenerating = true;

        while (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + regenAmount, maxHealth);
            yield return new WaitForSeconds(regenTickRate);
        }

        isRegenerating = false;
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
