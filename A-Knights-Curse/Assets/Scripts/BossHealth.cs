using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth;
    public int contactDamage = 20;  // Damage dealt to player on contact

    public GameObject healthBarUI;    // DemonHPBackground
    public Image healthFill;          // DemonHPFill
    public RectTransform emblemIcon;  // DemonEmblem - Now static, won't move with health
    
    private bool uiInitialized = false;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Boss health initialized with " + maxHealth + " health.");
        
        // Debug collider information
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            Debug.Log("Boss has a " + collider.GetType().Name + ", isTrigger: " + collider.isTrigger);
        }
        else
        {
            Debug.LogError("Boss is missing a Collider2D component! Add a collider in the inspector.");
            // Add a default collider
            BoxCollider2D newCollider = gameObject.AddComponent<BoxCollider2D>();
            newCollider.isTrigger = true;
            Debug.Log("Added a BoxCollider2D component to boss");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            return;
            
        Debug.Log("Boss collision with: " + collision.gameObject.name + " (Tag: " + collision.tag + ")");
            
        // Take damage from sword
        if (collision.CompareTag("SwordSwing"))
        {
            SwordSwing sword = collision.GetComponent<SwordSwing>();
            if (sword != null)
            {
                Debug.Log("Boss taking " + sword.damage + " damage from sword");
                TakeDamage(sword.damage);
            }
            else
            {
                // If somehow the sword swing doesn't have the component, still take some damage
                Debug.LogWarning("Sword without SwordSwing component hit boss - using default damage");
                TakeDamage(10);
            }
        }

        // Deal damage to player on contact
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(contactDamage);
            }
        }
    }
    
    // Add this method to catch physics collisions as well as triggers
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.gameObject == null)
            return;
            
        Debug.Log("Boss collision (non-trigger) with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");
        
        // Check if it's a sword swing
        if (collision.gameObject.CompareTag("SwordSwing"))
        {
            SwordSwing sword = collision.gameObject.GetComponent<SwordSwing>();
            if (sword != null)
            {
                Debug.Log("Boss taking " + sword.damage + " damage from sword (collision)");
                TakeDamage(sword.damage);
            }
            else
            {
                // Default damage
                TakeDamage(10);
            }
        }
        
        // Check for player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(contactDamage);
            }
        }
    }

    public void SetupUI(GameObject healthBarUI, Image healthFill, RectTransform emblem)
    {
        if (healthBarUI == null || healthFill == null || emblem == null)
        {
            Debug.LogError("Cannot set up Boss health UI - some UI elements are missing!");
            return;
        }
        
        this.healthBarUI = healthBarUI;
        this.healthFill = healthFill;
        this.emblemIcon = emblem;
        
        // Ensure the UI is active
        healthBarUI.SetActive(true);
        
        // Set the initial health bar fill
        if (healthFill != null)
        {
            healthFill.fillAmount = 1.0f;
        }
        
        uiInitialized = true;
        Debug.Log("Boss health UI initialized successfully");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss health: " + currentHealth + "/" + maxHealth);
        
        // Clamp health to prevent negative values
        currentHealth = Mathf.Max(0, currentHealth);
        
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        // Skip if UI is not initialized
        if (!uiInitialized)
        {
            Debug.LogWarning("Cannot update boss health bar - UI not initialized");
            return;
        }
        
        // Check if health fill reference exists
        if (healthFill == null)
        {
            Debug.LogError("Cannot update boss health bar - healthFill is null");
            return;
        }
        
        // Only update the health fill amount, emblem stays static
        float healthPercent = (float)currentHealth / maxHealth;
        healthFill.fillAmount = healthPercent;
        
        Debug.Log("Updated boss health bar: " + (healthPercent * 100) + "%");
    }

    void Die()
    {
        Debug.Log("Boss has been defeated!");
        
        if (healthBarUI != null)
            healthBarUI.SetActive(false);

        // Optional: Add death effect or animation before changing scene
        
        SceneManager.LoadScene("EndCutsceneScene"); // Replace with your actual end scene
    }
}
