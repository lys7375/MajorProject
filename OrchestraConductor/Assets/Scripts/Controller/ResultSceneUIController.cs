using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSceneUIController : MonoBehaviour
{
    public GameObject gameManger;
    public TMP_Text musicName;
    public TMP_Text hit;
    public TMP_Text chain;
    public TMP_Text missTxt;
    public Button retry;
    public Button next;

    public float fadeSpeed = 0.5f; // 渐变速度

    private float currentAlpha = 0f; // 当前 alpha 值

    // Start is called before the first frame update
    void Start()
    {
        DisplayData();

        retry.onClick.AddListener(Retry);
        next.onClick.AddListener(Next);
    }

    // Update is called once per frame
    void Update()
    {
        FadeIn();
    }

    void DisplayData()
    {
        string[] dataPieces = gameManger.GetComponent<GameManger>().LoadData();

        if (dataPieces.Length == 4)
        {
            string sceneName = dataPieces[0];
            string finalScore = dataPieces[1];
            string maxHitChain = dataPieces[2];
            string miss = dataPieces[3];

            sceneName = sceneName.Replace("_", " ");

            musicName.text = sceneName;
            hit.text = "Score: " + finalScore;
            chain.text = "Max Chain: " + maxHitChain;
            missTxt.text = "Miss: " + miss;
        }
    }

    //void DisplayData()
    //{
    //    string key = PlayerPrefs.GetString("levelName");

    //    string data = PlayerPrefs.GetString(key);

    //    string[] dataPieces = data.Split('|');


    //    if (dataPieces.Length == 4)
    //    {
    //        string sceneName = dataPieces[0];
    //        string finalScore = dataPieces[1];
    //        string maxHitChain = dataPieces[2];
    //        string miss = dataPieces[3];

    //        sceneName = sceneName.Replace("_", " ");

    //        musicName.text = sceneName;
    //        hit.text = "Score: " + finalScore;
    //        chain.text = "Max Chain: " + maxHitChain;
    //        missTxt.text = "Miss: " + miss;

    //        //Debug.Log("Scene Name: " + sceneName);
    //        //Debug.Log("Final Score: " + finalScore);
    //        //Debug.Log("Max Hit Chain: " + maxHitChain);
    //        //Debug.Log("Miss: " + miss);
    //    }
    //}

    // 重玩
    void Retry()
    {
        GameManger.finalScore = 0;
        GameManger.maxHitChain = 0;
        GameManger.miss = 0;

        SceneManager.LoadScene("GameScene");
    }

    // 回到LevelSelectionScene
    void Next()
    {
        SceneManager.LoadScene("LevelSelectionScene");
    }

    void FadeIn()
    {
        currentAlpha += fadeSpeed * Time.deltaTime;
        currentAlpha = Mathf.Clamp01(currentAlpha);

        musicName.color = new Color(musicName.color.r, musicName.color.g, musicName.color.b, currentAlpha);
        hit.color = new Color(hit.color.r, hit.color.g, hit.color.b, currentAlpha);
        chain.color = new Color(chain.color.r, chain.color.g, chain.color.b, currentAlpha);
        missTxt.color = new Color(missTxt.color.r, missTxt.color.g, missTxt.color.b, currentAlpha);

        if (currentAlpha >= 1f)
        {
            enabled = false;
        }
    }
}
