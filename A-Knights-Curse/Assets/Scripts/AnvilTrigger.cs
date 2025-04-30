using UnityEngine;
using UnityEngine.UI;

public class AnvilTrigger : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int[] upgradeCosts = { 5, 10, 20 };
    public int healthIncreasePerUpgrade = 10;
    public float speedIncreasePerUpgrade = 0.5f;
    public int damageIncreasePerUpgrade = 5;
    
    [Header("UI Elements")]
    public GameObject upgradePromptUI;
    public Text upgradeText;
    
    [Header("Detection Settings")]
    public float detectionRadius = 2f;
    public float checkInterval = 0.5f; // How often to check if player is still in range
    
    private bool inRange = false;
    private int currentUpgradeIndex = 0;
    private BloodEssenceManager bloodManager;
    private PlayerStats playerStats;
    private float lastRangeCheck = 0f;
    
    // For debug visualization
    [HideInInspector]
    public GameObject lastPlayerInRange;

    void Awake()
    {
        // Make sure we have a collider that's set as a trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log("Set anvil collider to trigger mode");
        }
    }

    void Start()
    {
        // Check if UI components are assigned
        if (upgradePromptUI == null)
        {
            Debug.LogError("upgradePromptUI is not assigned on AnvilTrigger! Please assign it in Inspector.");
        }
        
        if (upgradeText == null)
        {
            Debug.LogError("upgradeText is not assigned on AnvilTrigger! Please assign it in Inspector.");
        }
        
        // Hide prompt at start
        HidePrompt();
        
        // Find references
        bloodManager = FindObjectOfType<BloodEssenceManager>();
        playerStats = FindObjectOfType<PlayerStats>();
        
        if (bloodManager == null)
        {
            Debug.LogError("BloodEssenceManager not found in scene!");
        }
        
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found in scene!");
        }
        
        Debug.Log("AnvilTrigger initialized. Has UI: " + (upgradePromptUI != null));
    }

    void Update()
    {
        // Periodically check if player is still in range
        if (Time.time > lastRangeCheck + checkInterval)
        {
            lastRangeCheck = Time.time;
            
            // If we think player is in range, verify
            if (inRange)
            {
                VerifyPlayerStillInRange();
            }
            // If not in range, check for nearby players
            else
            {
                CheckForNearbyPlayer();
            }
        }
        
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            TryUpgrade();
        }
    }
    
    // Verify player is still in range
    void VerifyPlayerStillInRange()
    {
        bool playerFound = false;
        
        // Check around the anvil for the player
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                playerFound = true;
                lastPlayerInRange = col.gameObject;
                break;
            }
        }
        
        // If player left the area, hide the prompt
        if (!playerFound && inRange)
        {
            Debug.Log("Player left anvil range (verified by distance check)");
            inRange = false;
            HidePrompt();
            lastPlayerInRange = null;
        }
    }
    
    // Hide the upgrade prompt UI
    void HidePrompt()
    {
        if (upgradePromptUI != null)
        {
            upgradePromptUI.SetActive(false);
            Debug.Log("Hiding anvil prompt UI");
        }
    }
    
    // Show the upgrade prompt UI
    void ShowPrompt()
    {
        if (upgradePromptUI != null)
        {
            upgradePromptUI.SetActive(true);
            Debug.Log("Showing anvil prompt UI");
        }
        else
        {
            Debug.LogError("AnvilPromptUI is missing!");
        }
    }
    
    // Manually check for nearby players in case trigger doesn't work
    void CheckForNearbyPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                // Player found nearby
                lastPlayerInRange = col.gameObject;
                HandlePlayerEntered(col);
                break;
            }
        }
    }
    
    void HandlePlayerEntered(Collider2D other)
    {
        Debug.Log("Player entered anvil range via " + 
                 (other.isTrigger ? "trigger" : "manual check"));
        inRange = true;
        UpdatePromptText();
        ShowPrompt();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D with: " + other.name + ", tag: " + other.tag);
        if (other.CompareTag("Player"))
        {
            lastPlayerInRange = other.gameObject;
            HandlePlayerEntered(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D with: " + other.name);
        if (other.CompareTag("Player"))
        {
            inRange = false;
            HidePrompt();
            lastPlayerInRange = null;
        }
    }
    
    void UpdatePromptText()
    {
        if (upgradeText == null) return;
        
        if (currentUpgradeIndex < upgradeCosts.Length)
        {
            string stats = string.Format("\nHP +{0}, Speed +{1}, Damage +{2}", 
                healthIncreasePerUpgrade, 
                speedIncreasePerUpgrade, 
                damageIncreasePerUpgrade);
                
            upgradeText.text = "Press E to upgrade gear (Cost: " + upgradeCosts[currentUpgradeIndex] + ")" + stats;
            
            // Add indication if player has enough essence
            if (bloodManager != null && bloodManager.coins >= upgradeCosts[currentUpgradeIndex])
            {
                upgradeText.text += "\n<color=green>Enough essence!</color>";
            }
            else
            {
                upgradeText.text += "\n<color=red>Not enough essence</color>";
            }
        }
        else
        {
            upgradeText.text = "Max upgrades reached";
        }
    }

    void TryUpgrade()
    {
        // Check for max upgrades
        if (currentUpgradeIndex >= upgradeCosts.Length)
        {
            Debug.Log("Max upgrades reached");
            return;
        }
        
        // Check for necessary components
        if (bloodManager == null)
        {
            bloodManager = FindObjectOfType<BloodEssenceManager>();
            if (bloodManager == null)
            {
                Debug.LogError("Cannot find BloodEssenceManager!");
                return;
            }
        }
        
        // Check if player has enough blood essence
        if (bloodManager.coins >= upgradeCosts[currentUpgradeIndex])
        {
            Debug.Log("Upgrading player stats. Cost: " + upgradeCosts[currentUpgradeIndex]);
            
            // Deduct cost
            bloodManager.coins -= upgradeCosts[currentUpgradeIndex];
            
            // Apply upgrade
            UpgradePlayer();
            
            // Move to next upgrade tier
            currentUpgradeIndex++;
            
            // Update prompt text
            UpdatePromptText();
            
            // Play sound effect (if you have one)
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            Debug.Log("Not enough blood essence. Have: " + bloodManager.coins + ", Need: " + upgradeCosts[currentUpgradeIndex]);
            
            // Optionally, show a "not enough essence" message
            if (upgradeText != null)
            {
                string originalText = upgradeText.text;
                upgradeText.text = "<color=red>Not enough blood essence!</color>";
                
                // Reset text after a delay
                Invoke("UpdatePromptText", 1.5f);
            }
        }
    }

    void UpgradePlayer()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("Cannot find PlayerStats!");
                return;
            }
        }
        
        playerStats.maxHealth += healthIncreasePerUpgrade;
        playerStats.currentHealth += healthIncreasePerUpgrade; // Also heal the player
        playerStats.moveSpeed += speedIncreasePerUpgrade;
        playerStats.damage += damageIncreasePerUpgrade;
        
        // Update player's health display if needed
        playerStats.UpdateHealthAfterUpgrade();
        
        Debug.Log("Player upgraded! New stats: HP=" + playerStats.maxHealth + 
                 ", Speed=" + playerStats.moveSpeed + 
                 ", Damage=" + playerStats.damage);
    }
    
    // Visual debugging
    void OnDrawGizmos()
    {
        // Draw interaction range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Show connection to UI if assigned
        if (upgradePromptUI != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, upgradePromptUI.transform.position);
        }
        
        // Show connection to last player in range
        if (lastPlayerInRange != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, lastPlayerInRange.transform.position);
        }
    }
}