using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public int damage = 10;  // Default damage value
    public SpriteRenderer spriteRenderer;
    public float offsetX = 1f;  // How far from player center the swing appears
    public float lifetime = 0.35f;  // How long the swing lasts
    
    // Reference back to the player's attack script to notify when done
    [HideInInspector]
    public PlayerAttack parentAttackScript;
    
    private float spawnTime;
    private bool hasDamagedBoss = false;

    void Awake()
    {
        // Auto-find sprite renderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer missing on SwordSwing prefab!");
            }
        }
        
        // Ensure this object has the correct tag
        gameObject.tag = "SwordSwing";
        
        // Record spawn time
        spawnTime = Time.time;
        
        // Add a collider if missing
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector2(1.5f, 1.5f);
            Debug.Log("Added missing collider to SwordSwing");
        }
    }

    public void Initialize(Vector2 direction)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("Cannot initialize SwordSwing: SpriteRenderer is missing!");
            return;
        }
        
        // Only care about horizontal direction for flipping
        bool isRightSide = direction.x > 0;
        
        // Flip sprite and position if attacking right
        spriteRenderer.flipX = isRightSide;
        
        // Position the swing on the correct side of the player
        Vector3 position = transform.position;
        position.x += isRightSide ? offsetX : -offsetX;
        transform.position = position;

        // Get damage value from player if possible
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            damage = playerStats.damage;
        }
        
        Debug.Log("SwordSwing initialized with damage: " + damage);
    }

    void Update()
    {
        // Check if lifetime has expired
        if (Time.time >= spawnTime + lifetime)
        {
            // Notify the parent attack script that we're done
            if (parentAttackScript != null)
            {
                parentAttackScript.EndAttack();
            }
            
            // Destroy this game object
            Destroy(gameObject);
        }
        
        // Actively check for boss collisions (in case trigger doesn't work)
        if (!hasDamagedBoss)
        {
            CheckForBoss();
        }
    }
    
    void CheckForBoss()
    {
        // Look for the boss within a small radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D hit in hits)
        {
            // If we found the boss
            BossHealth bossHealth = hit.GetComponent<BossHealth>();
            if (bossHealth != null && !hasDamagedBoss)
            {
                Debug.Log("SwordSwing directly detected boss via overlap check!");
                bossHealth.TakeDamage(damage);
                hasDamagedBoss = true;
                break;
            }
        }
    }
    
    // Collision detection with trigger colliders
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SwordSwing collided with: " + collision.gameObject.name + " (Tag: " + collision.tag + ")");
        
        // Check if we hit the boss
        BossHealth bossHealth = collision.GetComponent<BossHealth>();
        if (bossHealth != null && !hasDamagedBoss)
        {
            Debug.Log("SwordSwing detected boss collision via trigger!");
            bossHealth.TakeDamage(damage);
            hasDamagedBoss = true;
        }
    }
    
    // Collision detection with non-trigger colliders
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("SwordSwing collision with: " + collision.gameObject.name);
        
        // Check if we hit the boss
        BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
        if (bossHealth != null && !hasDamagedBoss)
        {
            Debug.Log("SwordSwing detected boss via physical collision!");
            bossHealth.TakeDamage(damage);
            hasDamagedBoss = true;
        }
    }
    
    // Ensure we destroy this object even if something goes wrong
    void OnDestroy()
    {
        if (parentAttackScript != null)
        {
            parentAttackScript.EndAttack();
        }
    }
} 