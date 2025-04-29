using UnityEngine;
using UnityEngine.UI;

public class BloodEssenceManager : MonoBehaviour
{
    public int coins = 0;
    public Text essenceText;

    void Update()
    {
        essenceText.text = "Blood Essence: " + coins;
    }

    public void AddEssence(int amount)
    {
        coins += amount;
    }
}
