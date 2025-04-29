using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth;

    public GameObject healthBarUI;    // DemonHPBackground
    public Image healthFill;          // DemonHPFill
    public RectTransform emblemIcon;  // DemonEmblem

    private float healthBarFullWidth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetupUI(GameObject healthBarUI, Image healthFill, RectTransform emblem)
    {
        this.healthBarUI = healthBarUI;
        this.healthFill = healthFill;
        this.emblemIcon = emblem;

        healthBarUI.SetActive(true);
        healthBarFullWidth = healthFill.rectTransform.sizeDelta.x;
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
        float healthPercent = (float)currentHealth / maxHealth;
        healthFill.fillAmount = healthPercent;

        // Move DemonEmblem to match the end of the health bar
        float newEmblemX = healthBarFullWidth * healthPercent;
        emblemIcon.anchoredPosition = new Vector2(newEmblemX, emblemIcon.anchoredPosition.y);
    }

    void Die()
    {
        if (healthBarUI != null)
            healthBarUI.SetActive(false);

        SceneManager.LoadScene("EndCutsceneScene"); // Replace with your actual end scene
    }
}
