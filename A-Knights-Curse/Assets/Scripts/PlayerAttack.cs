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

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check for attack input and cooldown
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            Attack();
        }

        // Reset movement lock and show player after attack animation
        if (isAttacking && Time.time > lastAttackTime + attackDuration)
        {
            isAttacking = false;
            if (lockMovementDuringAttack && playerMovement != null)
            {
                playerMovement.enabled = true;
            }
            if (playerSprite != null)
            {
                playerSprite.enabled = true;
            }
        }
    }

    void Attack()
    {
        // Get mouse position relative to player for direction
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePos - (Vector2)transform.position).normalized;

        // Hide player sprite during attack
        if (playerSprite != null)
        {
            playerSprite.enabled = false;
        }

        // Spawn the sword swing at player's position
        GameObject swing = Instantiate(swordSwingPrefab, transform.position, Quaternion.identity);
        
        // Initialize it with the attack direction (will only use horizontal component)
        SwordSwing swordSwing = swing.GetComponent<SwordSwing>();
        if (swordSwing != null)
        {
            swordSwing.Initialize(attackDirection);
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
}
