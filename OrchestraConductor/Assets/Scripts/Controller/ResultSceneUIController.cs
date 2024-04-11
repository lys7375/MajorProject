using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneUIController : MonoBehaviour
{
    public TMP_Text musicName;
    public TMP_Text hit;
    public TMP_Text chain;
    public TMP_Text missTxt;
    private

    //public DataStorage data;

    // Start is called before the first frame update
    void Start()
    {
        string data = PlayerPrefs.GetString("Three-Tone_Composition");

        //data = DataStorage.LoadDataFromPlayerPrefs(levelName);

        //Debug.Log(data);

        //string str = data.levelName.Replace('_', ' ');

        //musicName.text = "Score: " + str;
        //hit.text = "Hit: " + data.finalScore;
        //chain.text = "Hit Chain: " + data.maxHitChain;
        //miss.text = "Miss Hit: " + data.missHit;

        string[] dataPieces = data.Split('|');

        if (dataPieces.Length == 4) // 确保数据拆分正确
        {
            string sceneName = dataPieces[0];
            int finalScore = int.Parse(dataPieces[1]);
            int maxHitChain = int.Parse(dataPieces[2]);
            int miss = int.Parse(dataPieces[3]);

            sceneName = sceneName.Replace("_", " ");

            musicName.text = sceneName;
            hit.text = "Score: " + finalScore.ToString();
            chain.text = "Max Chain: " + maxHitChain.ToString();
            missTxt.text = "Miss: " + miss.ToString();

            // 现在你可以使用这些值来做任何你需要的操作
            Debug.Log("Scene Name: " + sceneName);
            Debug.Log("Final Score: " + finalScore);
            Debug.Log("Max Hit Chain: " + maxHitChain);
            Debug.Log("Miss: " + miss);

        }
    }

    // Update is called once per frame
    void Update()
    {
        //    string str = data.levelName.Replace('_', ' ');

        //    musicName.text = "Score: " + str;
        //    hit.text = "Hit: " + data.finalScore;
        //    chain.text = "Hit Chain: " + data.maxHitChain;
        //    miss.text = "Miss Hit: " + data.missHit;
    }
}
