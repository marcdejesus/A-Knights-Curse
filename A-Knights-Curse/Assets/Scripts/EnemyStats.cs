using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 30;
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle taking damage from sword
        if (collision.CompareTag("SwordSwing"))
        {
            TakeDamage(collision.GetComponent<SwordSwing>().damage);
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
        FindObjectOfType<BloodEssenceManager>().AddEssence(1); // +1 Blood Essence
        Destroy(gameObject);
    }
}
