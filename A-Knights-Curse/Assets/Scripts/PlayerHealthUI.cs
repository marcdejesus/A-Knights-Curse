using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject playerHPBackground;  // Optional, for hiding on death
    public Image playerHPFill;            // Health bar fill image
    public Image heartIcon;               // Static heart icon at bottom of health bar

    private PlayerStats playerStats;       // Reference to player stats

    void Start()
    {
        // Find and store reference to PlayerStats
        playerStats = FindObjectOfType<PlayerStats>();
        
        if (playerStats == null)
        {
            Debug.LogError("PlayerHealthUI: Could not find PlayerStats component!");
            return;
        }

        // Do initial UI update
        UpdateHealthUI();
    }

    void Update()
    {
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (playerStats == null || playerHPFill == null) return;

        // Calculate health percentage
        float healthPercent = (float)playerStats.currentHealth / playerStats.maxHealth;
        
        // Update fill amount
        playerHPFill.fillAmount = healthPercent;

        // Hide UI on death if background is assigned
        if (playerHPBackground != null && playerStats.currentHealth <= 0)
        {
            playerHPBackground.SetActive(false);
        }
    }
} 