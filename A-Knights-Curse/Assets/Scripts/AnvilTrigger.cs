using UnityEngine;

public class AnvilTrigger : MonoBehaviour
{
    public int upgradeCost = 5;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetMouseButtonDown(0))
        {
            BloodEssenceManager bloodManager = FindObjectOfType<BloodEssenceManager>();
            if (bloodManager.coins >= upgradeCost)
            {
                bloodManager.coins -= upgradeCost;
                UpgradePlayer();
            }
        }
    }

    void UpgradePlayer()
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        stats.maxHealth += 10;
        stats.moveSpeed += 0.5f;
        stats.damage += 5;
    }
}
