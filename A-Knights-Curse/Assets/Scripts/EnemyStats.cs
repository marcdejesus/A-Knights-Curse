using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 30;
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure collision is valid
        if (collision == null)
            return;
        
        // Handle taking damage from sword
        if (collision.CompareTag("SwordSwing"))
        {
            SwordSwing swordSwing = collision.GetComponent<SwordSwing>();
            if (swordSwing != null)
            {
                TakeDamage(swordSwing.damage);
            }
            else
            {
                TakeDamage(10);
            }
        }
        
        // Deal damage to player on contact
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }

    void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        BloodEssenceManager bloodManager = FindObjectOfType<BloodEssenceManager>();
        if (bloodManager != null)
        {
            bloodManager.AddEssence(1); // +1 Blood Essence
        }
        Destroy(gameObject);
    }
}
