using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 30;
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SwordSwing"))
        {
            TakeDamage(collision.GetComponent<SwordSwing>().damage);
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
