using UnityEngine;
using UnityEngine.UI;

public class AnvilTrigger : MonoBehaviour
{
    public int[] upgradeCosts = { 5, 10, 20 };
    public GameObject upgradePromptUI;
    public Text upgradeText;
    private bool inRange = false;
    private int currentUpgradeIndex = 0;

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            TryUpgrade();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            if (currentUpgradeIndex < upgradeCosts.Length)
                upgradeText.text = "Press E to upgrade gear (Cost: " + upgradeCosts[currentUpgradeIndex] + ")";
            else
                upgradeText.text = "Max upgrades reached";
            upgradePromptUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            upgradePromptUI.SetActive(false);
        }
    }

    void TryUpgrade()
    {
        if (currentUpgradeIndex >= upgradeCosts.Length) return;

        var blood = FindObjectOfType<BloodEssenceManager>();
        if (blood.coins >= upgradeCosts[currentUpgradeIndex])
        {
            blood.coins -= upgradeCosts[currentUpgradeIndex];
            UpgradePlayer();
            currentUpgradeIndex++;
            if (currentUpgradeIndex < upgradeCosts.Length)
                upgradeText.text = "Press E to upgrade gear (Cost: " + upgradeCosts[currentUpgradeIndex] + ")";
            else
                upgradeText.text = "Max upgrades reached";
        }
    }

    void UpgradePlayer()
    {
        var stats = FindObjectOfType<PlayerStats>();
        stats.maxHealth += 10;
        stats.moveSpeed += 0.5f;
        stats.damage += 5;
    }
}