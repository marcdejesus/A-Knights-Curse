using UnityEngine;
using UnityEngine.UI;

// This is a simplified helper script to set up the anvil's required components
public class AnvilSetupHelper : MonoBehaviour
{
    public GameObject upgradePromptUI;
    public Text upgradeText;
    
    void Start()
    {
        SetupAnvil();
    }
    
    public void SetupAnvil()
    {
        // Make sure we have a collider
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.size = new Vector2(2f, 1f);
            col.offset = new Vector2(0f, 0.5f);
            Debug.Log("Added BoxCollider2D to anvil");
        }
        
        // Make sure it's a trigger
        col.isTrigger = true;
        
        // Get or add AnvilTrigger component
        AnvilTrigger trigger = GetComponent<AnvilTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<AnvilTrigger>();
            Debug.Log("Added AnvilTrigger to anvil");
        }
        
        // Link the UI elements if provided
        if (upgradePromptUI != null)
        {
            trigger.upgradePromptUI = upgradePromptUI;
            Debug.Log("Assigned upgrade prompt UI");
            
            // If text is provided, link it
            if (upgradeText != null)
            {
                trigger.upgradeText = upgradeText;
                Debug.Log("Assigned upgrade text");
            }
            // Otherwise try to find it
            else if (upgradePromptUI != null)
            {
                Text textComponent = upgradePromptUI.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    trigger.upgradeText = textComponent;
                    Debug.Log("Found and assigned text component automatically");
                }
            }
        }
        
        // Hide the UI at start
        if (upgradePromptUI != null)
        {
            upgradePromptUI.SetActive(false);
        }
        
        // Log success
        Debug.Log("Anvil setup complete. UI prompt assigned: " + (upgradePromptUI != null));
    }
    
    void OnDrawGizmos()
    {
        // Visual debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
} 