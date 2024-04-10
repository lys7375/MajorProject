using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelselectionSceneUIController : MonoBehaviour
{
    public Button game0;
    public Button game1;
    public Button game2;
    public TMP_Text levelName1;
    public TMP_Text levelName2;
    public TMP_Text levelName3;
    public TMP_Text score1;
    public TMP_Text score2;
    public TMP_Text score3;
    public TMP_Text missHit1;
    public TMP_Text missHit2;
    public TMP_Text missHit3;
    public TMP_Text maxChain1;
    public TMP_Text maxChain2;
    public TMP_Text maxChain3;

    // Start is called before the first frame update
    void Start()
    {
        game0.onClick.AddListener(Game0ButtonOnClick);
        ShowGameInfo("Constant_Moderato", levelName1, score1, missHit1, maxChain1);
        ShowGameInfo("A_Familiar_Sight_and_Leisure", levelName2, score2, missHit2, maxChain2);
        ShowGameInfo("Three-Tone_Composition", levelName3 , score3, missHit3, maxChain3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Game0ButtonOnClick()
    {
        SceneManager.LoadScene("Constant_Moderato");
    }

    void ShowGameInfo(string key, TMP_Text levelName, TMP_Text score, TMP_Text missTxt, TMP_Text maxChain)
    {
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
}
