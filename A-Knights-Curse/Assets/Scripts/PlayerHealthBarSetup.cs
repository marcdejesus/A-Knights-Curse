using UnityEngine;
using UnityEngine.UI;

// This script sets up the player health bar UI and connects it to the PlayerStats component
public class PlayerHealthBarSetup : MonoBehaviour
{
    [Header("Health Bar Elements")]
    public GameObject healthBarContainer; // Parent container for the health bar UI
    public Image healthFillImage;         // The fill image that shows current health
    public Text healthValueText;          // Optional: displays numeric health values
    
    [Header("Stats Display Elements")]
    public Text damageStatText;           // Text element to display damage stat
    public Text speedStatText;            // Text element to display speed stat
    public Text maxHealthStatText;        // Text element to display max health stat
    
    [Header("UI Settings")]
    public Color healthBarColor = Color.red; // Default color if you want to set it once at start
    public bool useCustomColor = false;      // Whether to apply the color above
    
    [Header("References")]
    public PlayerStats playerStats;       // Reference to the player's stats component
    
    void Start()
    {
        // If no player stats reference provided, try to find it
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("Could not find PlayerStats component! Health bar will not work.");
                return;
            }
            Debug.Log("Found PlayerStats component automatically");
        }
        
        // Verify we have the required components
        if (healthBarContainer == null)
        {
            Debug.LogError("Health Bar Container is not assigned!");
            return;
        }
        
        if (healthFillImage == null)
        {
            Debug.LogError("Health Fill Image is not assigned!");
            return;
        }
        
        // Ensure the health fill has the right settings
        healthFillImage.type = Image.Type.Filled;
        healthFillImage.fillMethod = Image.FillMethod.Horizontal;
        healthFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        
        // Set a custom color only if specified
        if (useCustomColor)
        {
            healthFillImage.color = healthBarColor;
        }
        
        // Connect the health UI to the player stats component
        playerStats.healthBarUI = healthBarContainer;
        playerStats.healthFill = healthFillImage;
        playerStats.healthText = healthValueText;
        
        // Connect the stats display UI elements
        playerStats.damageStatText = damageStatText;
        playerStats.speedStatText = speedStatText;
        playerStats.maxHealthStatText = maxHealthStatText;
        
        // Make sure the health bar starts with the correct value
        playerStats.UpdateHealthBar();
        
        // Initialize the stats display
        playerStats.UpdateStatsDisplay();
        
        Debug.Log("Player health bar and stats display successfully connected to PlayerStats");
    }
} 