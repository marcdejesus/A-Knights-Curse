using UnityEngine;
using UnityEngine.UI;
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
    
    [Header("UI Elements")]
    public GameObject healthBarUI;    // Main health bar container
    public Image healthFill;          // Health fill image that changes with health
    public Text healthText;           // Optional: text showing numeric health value

    private bool isInvulnerable = false;
    private float lastDamageTime;
    private bool isRegenerating = false;

    void Start()
    {
        currentHealth = maxHealth;
        lastDamageTime = -regenDelay; // Allow immediate regen at start if needed
        
        // Initialize the health bar
        UpdateHealthBar();
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
        
        // Update the health bar
        UpdateHealthBar();
        
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
    
    // Method to update the health bar UI
    public void UpdateHealthBar()
    {
        // Clamp current health to valid range
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        // Update the fill amount if we have a health fill image
        if (healthFill != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            healthFill.fillAmount = healthPercent;
            
            // Color change has been removed - health bar will maintain its original color
        }
        
        // Update the numeric text if we have it
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }
    
    // Add this method to update health display after upgrades
    public void UpdateHealthAfterUpgrade()
    {
        // Make sure current health isn't more than max health
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        // Update the health bar
        UpdateHealthBar();
        
        // Any UI update needed can be triggered here
        Debug.Log("Player health updated: " + currentHealth + "/" + maxHealth);
    }

    IEnumerator RegenerateHealth()
    {
        isRegenerating = true;

        while (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + regenAmount, maxHealth);
            UpdateHealthBar(); // Update the health bar after regeneration
            yield return new WaitForSeconds(regenTickRate);
        }

        isRegenerating = false;
    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        
        // Optional: add visual feedback for invulnerability
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        if (playerSprite != null)
        {
            // Flash the player during invulnerability
            float flashInterval = 0.1f;
            for (float i = 0; i < invulnerabilityTime; i += flashInterval)
            {
                playerSprite.enabled = !playerSprite.enabled;
                yield return new WaitForSeconds(flashInterval);
            }
            playerSprite.enabled = true; // Make sure player is visible after
        }
        else
        {
            // Just wait if no sprite renderer
            yield return new WaitForSeconds(invulnerabilityTime);
        }
        
        isInvulnerable = false;
    }

    void Die()
    {
        // Optional: Play death animation
        Debug.Log("Player died");
        
        // Hide the health bar on death
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }
        
        SceneManager.LoadScene("GameplayScene"); // Respawn on death
    }
    
    // Helper method to setup health bar UI at runtime if not assigned in Inspector
    public void SetupHealthBar(GameObject barUI, Image fillImage, Text textDisplay = null)
    {
        healthBarUI = barUI;
        healthFill = fillImage;
        healthText = textDisplay;
        
        // Configure the fill image properly
        if (healthFill != null)
        {
            healthFill.type = Image.Type.Filled;
            healthFill.fillMethod = Image.FillMethod.Horizontal;
            healthFill.fillOrigin = (int)Image.OriginHorizontal.Left;
            // No color changing - keeps original color
        }
        
        UpdateHealthBar();
    }
}
