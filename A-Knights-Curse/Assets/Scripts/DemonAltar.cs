using UnityEngine;
using UnityEngine.UI;

public class DemonAltar : MonoBehaviour
{
    public GameObject demonPrefab;
    public Transform summonPoint;
    public GameObject altarPromptUI;
    public Text altarText;
    private bool inRange = false;
    private bool hasSummoned = false;

    void Update()
    {
        if (inRange && !hasSummoned && Input.GetKeyDown(KeyCode.E))
        {
            SummonDemon();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasSummoned)
        {
            inRange = true;
            altarText.text = "Press E to summon the Demon";
            altarPromptUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            altarPromptUI.SetActive(false);
        }
    }

    void SummonDemon()
    {
        Instantiate(demonPrefab, summonPoint.position, Quaternion.identity);
        hasSummoned = true;
        altarPromptUI.SetActive(false);
    }
}