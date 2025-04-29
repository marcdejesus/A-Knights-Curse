using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public Sprite[] cutsceneImages; // Slideshow images
    public string[] cutsceneTexts;  // Story text
    public Image displayImage;      // UI Image for background
    public Text displayText;        // UI Text for story
    public Image fadeOverlay;       // UI Image for fade (black panel over everything)
    public Text continueText;       // "Press Any Key" UI Text

    public float typingSpeed = 0.03f;
    public float fadeSpeed = 2f;

    private int currentIndex = 0;
    private bool isReadyForNext = false;

    void Start()
    {
        continueText.gameObject.SetActive(false);
        fadeOverlay.color = new Color(0, 0, 0, 1); // Start black
        StartCoroutine(FadeIn());
        ShowCurrentSlide();
    }

    void Update()
    {
        if (Input.anyKeyDown && isReadyForNext)
        {
            currentIndex++;
            if (currentIndex >= cutsceneImages.Length)
            {
                EndCutscene();
            }
            else
            {
                continueText.gameObject.SetActive(false);
                StartCoroutine(FadeSlide());
            }
        }
    }

    void ShowCurrentSlide()
    {
        displayImage.sprite = cutsceneImages[currentIndex];
        StartCoroutine(TypeText(cutsceneTexts[currentIndex]));
    }

    IEnumerator TypeText(string fullText)
    {
        displayText.text = "";
        isReadyForNext = false;

        foreach (char letter in fullText.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(0.5f); // Small pause after typing
        continueText.gameObject.SetActive(true); // Now show "Press Any Key"
        isReadyForNext = true;
    }

    IEnumerator FadeSlide()
    {
        // Fade to black
        while (fadeOverlay.color.a < 1)
        {
            fadeOverlay.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        ShowCurrentSlide();

        // Fade from black
        while (fadeOverlay.color.a > 0)
        {
            fadeOverlay.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        // Start scene by fading from black
        while (fadeOverlay.color.a > 0)
        {
            fadeOverlay.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void EndCutscene()
    {
        SceneManager.LoadScene("GameplayScene"); // Replace with your actual next scene name
    }
}
