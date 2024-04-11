using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSceneUIController : MonoBehaviour
{
    public TMP_Text musicName;
    public TMP_Text hit;
    public TMP_Text chain;
    public TMP_Text missTxt;
    public Button retry;
    public Button next;

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
        
    }

    void DisplayData()
    {
        string data = PlayerPrefs.GetString("Three-Tone_Composition");

        string[] dataPieces = data.Split('|');


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

            //Debug.Log("Scene Name: " + sceneName);
            //Debug.Log("Final Score: " + finalScore);
            //Debug.Log("Max Hit Chain: " + maxHitChain);
            //Debug.Log("Miss: " + miss);
        }
    }

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
}
