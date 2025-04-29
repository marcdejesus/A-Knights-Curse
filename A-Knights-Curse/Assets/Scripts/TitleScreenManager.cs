using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public Image fadeImage; // Assign your black image here!
    public float fadeSpeed = 2f;
    private bool isFading = false;

    void Update()
    {
        if (Input.anyKeyDown && !isFading)
        {
            isFading = true;
        }

        if (isFading)
        {
            fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
            if (fadeImage.color.a >= 0.95f) // Once fully faded
            {
                LoadNextScene();
            }
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("CutsceneScene");
    }
}
