using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DemonAltar : MonoBehaviour
{
    public GameObject demonPrefab;
    public Transform summonPoint;
    public GameObject altarPromptUI;
    public Text altarText;

    // Boss UI References (now optional)
    [Header("Boss UI - Optional")]
    public GameObject demonHPBarUI;
    public Image demonHPFillImage;
    public RectTransform demonEmblemIcon;

    private bool inRange = false;
    private bool hasSummoned = false;
    private bool showedWarning = false;

    void Start()
    {
        // Validate required references
        if (demonPrefab == null)
        {
            Debug.LogError("Demon prefab not assigned in DemonAltar!");
        }
        
        if (summonPoint == null)
        {
            Debug.LogError("Summon point not assigned in DemonAltar!");
        }
        
        // Check UI components
        if (demonHPBarUI != null && demonHPFillImage != null && demonEmblemIcon != null)
        {
            Debug.Log("Boss UI components are all assigned correctly");
            
            // Make sure the UI is initially hidden
            demonHPBarUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Some Boss UI components not assigned in DemonAltar. Boss health bar will be disabled.");
        }
    }

    void Update()
    {
        if (inRange && !hasSummoned && Input.GetKeyDown(KeyCode.E))
        {
            SummonDemon();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasSummoned)
        {
            inRange = true;
            
            // Add null check for UI elements
            if (altarPromptUI != null && altarText != null)
            {
                altarText.text = "Press E to summon the Demon";
                altarPromptUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            
            // Add null check
            if (altarPromptUI != null)
            {
                altarPromptUI.SetActive(false);
            }
        }
    }

    void SummonDemon()
    {
        // Check required components
        if (demonPrefab == null || summonPoint == null)
        {
            Debug.LogError("Cannot summon demon: missing prefab or summon point!");
            return;
        }
        
        Debug.Log("Summoning demon...");
        GameObject demon = Instantiate(demonPrefab, summonPoint.position, Quaternion.identity);
        
        // Setup the boss health UI if all components are available
        BossHealth bossHealth = demon.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            Debug.Log("BossHealth component found on summoned demon");
            
            // Only try to set up UI if all components exist
            if (demonHPBarUI != null && demonHPFillImage != null && demonEmblemIcon != null)
            {
                Debug.Log("Setting up boss health UI");
                
                // Ensure HP bar is active before setting up
                demonHPBarUI.SetActive(true);
                
                // Configure the fill image to ensure it's set up properly
                if (demonHPFillImage != null)
                {
                    demonHPFillImage.type = Image.Type.Filled;
                    demonHPFillImage.fillMethod = Image.FillMethod.Horizontal;
                    demonHPFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
                    demonHPFillImage.fillAmount = 1.0f;
                }
                
                bossHealth.SetupUI(demonHPBarUI, demonHPFillImage, demonEmblemIcon);
            }
            else if (!showedWarning)
            {
                Debug.LogWarning("Boss health UI not displayed - missing UI components");
                showedWarning = true;
            }
        }
        else
        {
            Debug.LogError("Summoned demon is missing BossHealth component!");
        }
        
        hasSummoned = true;
        
        // Add null check
        if (altarPromptUI != null)
        {
            altarPromptUI.SetActive(false);
        }
    }
}