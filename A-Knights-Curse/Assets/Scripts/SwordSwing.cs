using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public int damage = 10;  // Default damage value
    public SpriteRenderer spriteRenderer;
    public float offsetX = 1f;  // How far from player center the swing appears

    void Awake()
    {
        // Auto-find sprite renderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void Initialize(Vector2 direction)
    {
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
    }

    void Start()
    {
        // Destroy after animation plays
        Destroy(gameObject, 0.35f);
    }
} 