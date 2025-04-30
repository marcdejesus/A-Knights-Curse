using UnityEngine;
using UnityEngine.UI; // Add this for Text class

// This script helps set up the anvil properly in the Unity Editor
public class AnvilSetup : MonoBehaviour
{
    [Header("Required Components")]
    public GameObject anvilPromptUI;  // Assign your UI prompt here
    public Transform interactionPoint; // Where the player needs to stand
    
    void Reset()
    {
        // This function runs when the component is first added or reset
        Debug.Log("Setting up Anvil...");
        
        // Check for BoxCollider2D
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.size = new Vector2(2f, 1f);  // Reasonable size for anvil interaction
            col.offset = new Vector2(0f, 0.5f);  // Adjust based on your sprite
            col.isTrigger = true;  // Must be a trigger
            Debug.Log("Added BoxCollider2D to anvil");
        }
        else
        {
            col.isTrigger = true;
            Debug.Log("Existing collider set to trigger");
        }
        
        // Check for AnvilTrigger script
        AnvilTrigger trigger = GetComponent<AnvilTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<AnvilTrigger>();
            Debug.Log("Added AnvilTrigger component");
        }
        
        // Create UI prompt if it doesn't exist
        if (anvilPromptUI == null)
        {
            Debug.LogWarning("Please assign anvilPromptUI in the inspector!");
        }
    }
    
    // This function is called when you press the "Set Up Anvil" button in the inspector
    public void SetupAnvil()
    {
        // Ensure we have all required components
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.size = new Vector2(2f, 1f);
            col.offset = new Vector2(0f, 0.5f);
        }
        col.isTrigger = true;
        
        // Set up AnvilTrigger
        AnvilTrigger trigger = GetComponent<AnvilTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<AnvilTrigger>();
        }
        
        // Assign UI prompt if available
        if (anvilPromptUI != null)
        {
            trigger.upgradePromptUI = anvilPromptUI;
            
            // Try to find Text component in children
            Text[] texts = anvilPromptUI.GetComponentsInChildren<Text>();
            if (texts.Length > 0)
            {
                trigger.upgradeText = texts[0];
                Debug.Log("Found and assigned UI Text component");
            }
        }
        
        Debug.Log("Anvil setup complete!");
    }
    
    // Visual debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f);
        
        // Draw interaction range
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Gizmos.color = Color.green;
            // Convert local collider bounds to world space
            Vector3 center = transform.TransformPoint(col.offset);
            Vector3 size = new Vector3(col.size.x, col.size.y, 0.1f);
            
            // Draw the box
            Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, size);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
} 