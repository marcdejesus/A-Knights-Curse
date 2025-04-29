using UnityEngine;
using UnityEngine.UI;

public class DemonAltar : MonoBehaviour
{
    public GameObject demonPrefab;
    public Transform summonPoint;
    public GameObject altarPromptUI;
    public Text altarText;

    // Boss UI References
    public GameObject demonHPBarUI;
    public Image demonHPFillImage;
    public RectTransform demonEmblemIcon;

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
        GameObject demon = Instantiate(demonPrefab, summonPoint.position, Quaternion.identity);
        
        // Setup the boss health UI
        BossHealth bossHealth = demon.GetComponent<BossHealth>();
        bossHealth.SetupUI(demonHPBarUI, demonHPFillImage, demonEmblemIcon);
        
        hasSummoned = true;
        altarPromptUI.SetActive(false);
    }
}