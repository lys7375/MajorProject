using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public Button playBtn;
    public Button exitBtn;

    public TMP_Text title;
    public TMP_Text playText;
    public TMP_Text exitText;
    public float fadeSpeed = 0.5f; // 渐变速度

    private float currentAlpha = 0f; // 当前 alpha 值

    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(PlayButtonOnClick);
        exitBtn.onClick.AddListener(ExitButtonOnClick);

        playText.color = new Color(playText.color.r, playText.color.g, playText.color.b, 0f);
        exitText.color = new Color(exitText.color.r, exitText.color.g, exitText.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        FadeIn();
    }

    void PlayButtonOnClick()
    {
        SceneManager.LoadScene("LevelSelectionScene");
    }

    void ExitButtonOnClick()
    {
        Application.Quit();
    }

    void FadeIn()
    {
        currentAlpha += fadeSpeed * Time.deltaTime;
        currentAlpha = Mathf.Clamp01(currentAlpha);

        title.color = new Color(playText.color.r, playText.color.g, playText.color.b, currentAlpha);
        playText.color = new Color(playText.color.r, playText.color.g, playText.color.b, currentAlpha);
        exitText.color = new Color(exitText.color.r, exitText.color.g, exitText.color.b, currentAlpha);

        if (currentAlpha >= 1f)
        {
            enabled = false;
        }
    }
}
