using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelselectionSceneUIController : MonoBehaviour
{
    //public static string levelName;

    public GameObject gameManger;

    public GameObject gObj1;
    public GameObject gObj2;
    public GameObject gObj3;
    public GameObject gObj4;

    public Button backBtn;

    // Start is called before the first frame update
    void Start()
    {
        gObj1.GetComponent<Button>().onClick.AddListener(() => ButtonClick(gObj1.name));
        gObj2.GetComponent<Button>().onClick.AddListener(() => ButtonClick(gObj2.name));
        gObj3.GetComponent<Button>().onClick.AddListener(() => ButtonClick(gObj3.name));
        gObj4.GetComponent<Button>().onClick.AddListener(() => ButtonClick(gObj4.name));

        backBtn.onClick.AddListener(BackBtnOnClick);

        ShowGameInfo("Three-Tone_Composition_Record", gObj1);
        ShowGameInfo("Three-Tone_Composition_Hard_Record", gObj2);
        ShowGameInfo("Constant_Moderato_Record", gObj3);
        ShowGameInfo("A_Familiar_Sight_and_Leisure_Record", gObj4);


        EnableHardLeve();
        EnableNewLeve();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ButtonClick(string str)
    {
        Debug.Log(str);
        GameManger.levelName = str;
        SceneManager.LoadScene("GameScene");
    }

    void BackBtnOnClick()
    {
        DeleteAllPlayerPrefsKeys();
        SceneManager.LoadScene("MainMenu");
    }

    void ShowGameInfo(string key, GameObject gameObject)
    {
        Transform parentTransform = gameObject.transform;

        TMP_Text levelTxt = parentTransform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        TMP_Text score = parentTransform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        TMP_Text missTxt = parentTransform.GetChild(2).gameObject.GetComponent<TMP_Text>();
        TMP_Text maxChain = parentTransform.GetChild(3).gameObject.GetComponent<TMP_Text>();

        string[] dataPieces = gameManger.GetComponent<GameManger>().LoadDataWithKey(key);

        if (dataPieces.Length == 4)
        {
            string sceneName = dataPieces[0];
            sceneName = sceneName.Replace("_", " ");
            int finalScore = int.Parse(dataPieces[1]);
            int maxHitChain = int.Parse(dataPieces[2]);
            int miss = int.Parse(dataPieces[3]);

            levelTxt.text = sceneName;
            score.text = "Score: " + finalScore.ToString();
            missTxt.text = "Max Chain: " + maxHitChain.ToString();
            maxChain.text = "Miss: " + miss.ToString();
        }
        else
        {
            score.text = "Score: 0";
            missTxt.text = "Max Chain: 0";
            maxChain.text = "Miss: 0";
        }
    }

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
                    gObj2.SetActive(true);
                }
                else
                {
                    gObj2.SetActive(false);
                }
            }
        }
        else
        {
            gObj2.SetActive(false);
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
                    gObj3.SetActive(true);
                }
            }
        }
        else
        {
            gObj3.SetActive(false);
        }
    }

    void DeleteAllPlayerPrefsKeys()
    {
        PlayerPrefs.DeleteKey("Three-Tone_Composition_Record");
        PlayerPrefs.DeleteKey("Three-Tone_Composition_Hard_Record");
        PlayerPrefs.DeleteKey("Constant_Moderato_Record");
        PlayerPrefs.DeleteKey("A_Familiar_Sight_and_Leisure_Record");
    }
}
