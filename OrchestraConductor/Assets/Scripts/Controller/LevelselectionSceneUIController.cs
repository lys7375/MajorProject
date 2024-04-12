using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelselectionSceneUIController : MonoBehaviour
{
    public static string levelName;
    public Button game0;
    public Button game1;
    public Button game2;
    public Button game3;
    public TMP_Text levelName1;
    public TMP_Text levelName2;
    public TMP_Text levelName3;
    public TMP_Text levelName4;
    public TMP_Text score1;
    public TMP_Text score2;
    public TMP_Text score3;
    public TMP_Text score4;
    public TMP_Text missHit1;
    public TMP_Text missHit2;
    public TMP_Text missHit3;
    public TMP_Text missHit4;
    public TMP_Text maxChain1;
    public TMP_Text maxChain2;
    public TMP_Text maxChain3;
    public TMP_Text maxChain4;
    public Button backBtn;
    public GameObject go;
    public GameObject go1;

    // Start is called before the first frame update
    void Start()
    {
        game0.onClick.AddListener(Game0ButtonOnClick);
        game1.onClick.AddListener(Game1ButtonOnClick);
        game2.onClick.AddListener(Game2ButtonOnClick);
        game3.onClick.AddListener(Game3ButtonOnClick);
        backBtn.onClick.AddListener(BackBtnOnClick);

        ShowGameInfo("Constant_Moderato_Record", levelName1, score1, missHit1, maxChain1);
        ShowGameInfo("A_Familiar_Sight_and_Leisure_Record", levelName2, score2, missHit2, maxChain2);
        ShowGameInfo("Three-Tone_Composition_Record", levelName3 , score3, missHit3, maxChain3);
        ShowGameInfo("Three-Tone_Composition_Hard_Record", levelName4 , score4, missHit4, maxChain4);

        EnableHardLeve();
        EnableNewLeve();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Game0ButtonOnClick()
    {
        SceneManager.LoadScene("GameScene");
        levelName = "Constant_Moderato";
    }

    void Game3ButtonOnClick()
    {
        SceneManager.LoadScene("GameScene");
        levelName = "Three-Tone_Composition_Hard";
    }

    void Game1ButtonOnClick()
    {
        SceneManager.LoadScene("GameScene");
        levelName = "A_Familiar_Sight_and_Leisure";
    }

    void Game2ButtonOnClick()
    {
        SceneManager.LoadScene("GameScene");
        levelName = "Three-Tone_Composition";
    }

    void BackBtnOnClick()
    {
        DeleteAllPlayerPrefsKeys();
        SceneManager.LoadScene("MainMenu");
    }

    void ShowGameInfo(string key, TMP_Text levelName, TMP_Text score, TMP_Text missTxt, TMP_Text maxChain)
    {
        //Debug.Log("ShowGameInfo");
        if (PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);
            string[] dataPieces = data.Split('|');

            if (dataPieces.Length == 4)
            {
                string sceneName = dataPieces[0];
                sceneName = sceneName.Replace("_", " ");
                int finalScore = int.Parse(dataPieces[1]);
                int maxHitChain = int.Parse(dataPieces[2]);
                int miss = int.Parse(dataPieces[3]);

                levelName.text = sceneName;
                score.text = "Score: " + finalScore.ToString();
                missTxt.text = "Max Chain: " + maxHitChain.ToString();
                maxChain.text = "Miss: " + miss.ToString();
            }
        }
        else
        {
            score.text = "Score: 0";
            missTxt.text = "Max Chain: 0";
            maxChain.text = "Miss: 0";
        }
    }

    //void CheckInfo(string key)
    //{
    //    if (PlayerPrefs.HasKey(key))
    //    {
    //        string data = PlayerPrefs.GetString(key);
    //        string[] dataPieces = data.Split('|');

    //        if (dataPieces.Length == 4)
    //        {
    //            int finalScore = int.Parse(dataPieces[1]);

    //        }
    //    }
    //}

    void EnableHardLeve()
    {
        if (PlayerPrefs.HasKey("Three-Tone_Composition_Record"))
        {
            string data = PlayerPrefs.GetString("Three-Tone_Composition_Record");
            string[] dataPieces = data.Split('|');

            if (dataPieces.Length == 4)
            {
                if (int.Parse(dataPieces[1]) >= 17)
                {
                    go.SetActive(true);
                }
            }
        }
    }

    void EnableNewLeve()
    {        
        if (PlayerPrefs.HasKey("Three-Tone_Composition_Hard_Record"))
        {
            string data = PlayerPrefs.GetString("Three-Tone_Composition_Hard_Record");
            string[] dataPieces = data.Split('|');

            if (dataPieces.Length == 4)
            {
                if (int.Parse(dataPieces[1]) >= 17)
                {
                    go1.SetActive(true);
                }
            }
        }
    }

    void DeleteAllPlayerPrefsKeys()
    {
        PlayerPrefs.DeleteKey("Constant_Moderato_Record");
        PlayerPrefs.DeleteKey("A_Familiar_Sight_and_Leisure_Record");
        PlayerPrefs.DeleteKey("Three-Tone_Composition_Record");
        PlayerPrefs.DeleteKey("Three-Tone_Composition_Hard_Record");
    }
}
