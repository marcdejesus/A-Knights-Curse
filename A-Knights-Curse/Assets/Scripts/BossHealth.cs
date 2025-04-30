using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth;
    public int contactDamage = 20;  // Damage dealt to player on contact

    public GameObject healthBarUI;    // DemonHPBackground
    public Image healthFill;          // DemonHPFill
    public RectTransform emblemIcon;  // DemonEmblem - Now static, won't move with health

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Take damage from sword
        if (collision.CompareTag("SwordSwing"))
        {
            SwordSwing sword = collision.GetComponent<SwordSwing>();
            if (sword != null)
            {
                TakeDamage(sword.damage);
            }
        }

        // Deal damage to player on contact
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(contactDamage);
            }
        }
    }

    public void SetupUI(GameObject healthBarUI, Image healthFill, RectTransform emblem)
    {
        this.healthBarUI = healthBarUI;
        this.healthFill = healthFill;
        this.emblemIcon = emblem;
        healthBarUI.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        // Only update the health fill amount, emblem stays static
        float healthPercent = (float)currentHealth / maxHealth;
        healthFill.fillAmount = healthPercent;
    }

    void Die()
    {
        if (healthBarUI != null)
            healthBarUI.SetActive(false);

        SceneManager.LoadScene("EndCutsceneScene"); // Replace with your actual end scene
    }
}
