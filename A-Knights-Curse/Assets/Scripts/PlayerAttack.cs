using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject swordSwingPrefab;
    public float attackCooldown = 0.5f;
    
    [Header("Optional Settings")]
    public bool lockMovementDuringAttack = true;  // Optional: lock movement during swing
    public float attackDuration = 0.35f;          // Should match animation duration

    private float lastAttackTime = 0f;
    private bool isAttacking = false;
    private PlayerMovement playerMovement;
    private SpriteRenderer playerSprite;
    private GameObject currentSwing;  // Track the current sword swing instance

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerSprite = GetComponent<SpriteRenderer>();
        
        // Ensure the sword swing prefab is assigned
        if (swordSwingPrefab == null)
        {
            Debug.LogError("SwordSwingPrefab is not assigned in PlayerAttack script. Please assign it in the Inspector.");
        }
        else
        {
            // Check if the prefab has the SwordSwing component
            SwordSwing testSwing = swordSwingPrefab.GetComponent<SwordSwing>();
            if (testSwing == null)
            {
                Debug.LogError("The assigned SwordSwingPrefab does not have a SwordSwing component! Please add it in the prefab.");
            }
        }
    }

    void Update()
    {
        // Only allow attack if not already attacking and cooldown has passed
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
        
        // Safety check - ensure swing is cleaned up if it's been too long
        if (isAttacking && Time.time > lastAttackTime + attackDuration + 0.1f)
        {
            // Force cleanup if attack has been active too long
            Debug.LogWarning("Attack took too long - forcing cleanup");
            EndAttack();
        }
    }

    void Attack()
    {
        // Check if prefab is assigned
        if (swordSwingPrefab == null)
        {
            Debug.LogError("SwordSwingPrefab is not assigned!");
            return;
        }

        // If somehow we still have a swing object, destroy it
        if (currentSwing != null)
        {
            Destroy(currentSwing);
            currentSwing = null;
        }

        // Get mouse position relative to player for direction
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePos - (Vector2)transform.position).normalized;

        // Hide player sprite during attack
        if (playerSprite != null)
        {
            playerSprite.enabled = false;
        }

        // Spawn the sword swing at player's position
        currentSwing = Instantiate(swordSwingPrefab, transform.position, Quaternion.identity);
        
        // Make sure the sword swing has the correct tag
        currentSwing.tag = "SwordSwing";
        
        // Ensure the sword swing has a collider
        Collider2D swingCollider = currentSwing.GetComponent<Collider2D>();
        if (swingCollider == null)
        {
            // Add a collider if missing
            BoxCollider2D newCollider = currentSwing.AddComponent<BoxCollider2D>();
            newCollider.isTrigger = true;
            newCollider.size = new Vector2(1.5f, 1.5f); // Set appropriate size
            Debug.Log("Added missing collider to sword swing");
        }
        else
        {
            // Make sure existing collider is a trigger
            swingCollider.isTrigger = true;
        }
        
        // Initialize it with the attack direction (will only use horizontal component)
        SwordSwing swordSwing = currentSwing.GetComponent<SwordSwing>();
        if (swordSwing != null)
        {
            swordSwing.Initialize(attackDirection);
            
            // Set this attack script as the parent so the sword swing can notify us when it's done
            swordSwing.parentAttackScript = this;
            
            Debug.Log("Sword swing initialized successfully with direction: " + attackDirection);
        }
        else
        {
            Debug.LogError("SwordSwing component not found on the instantiated prefab!");
            
            // Try to add the component at runtime if it's missing
            swordSwing = currentSwing.AddComponent<SwordSwing>();
            if (swordSwing != null)
            {
                Debug.Log("Added missing SwordSwing component to attack");
                swordSwing.damage = 10; // Set default damage
                swordSwing.Initialize(attackDirection);
                swordSwing.parentAttackScript = this;
            }
            else
            {
                // If we can't add the component, still set up our timer
                Invoke("EndAttack", attackDuration);
            }
        }

        // Update attack state
        lastAttackTime = Time.time;
        isAttacking = true;

        // Optional: Lock movement during attack
        if (lockMovementDuringAttack && playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }
    
    // Called when the attack animation is finished (can be called by the sword swing or by timer)
    public void EndAttack()
    {
        // Only do this if we're currently attacking
        if (!isAttacking)
            return;
            
        isAttacking = false;
        
        // Show player sprite after attack
        if (playerSprite != null)
        {
            playerSprite.enabled = true;
        }
        
        // Re-enable movement
        if (lockMovementDuringAttack && playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        
        // Clean up the current swing if it still exists
        if (currentSwing != null)
        {
            Destroy(currentSwing);
            currentSwing = null;
        }
    }
    
    // Make sure we clean up on destroy
    void OnDestroy()
    {
        if (currentSwing != null)
        {
            Destroy(currentSwing);
        }
    }
}
