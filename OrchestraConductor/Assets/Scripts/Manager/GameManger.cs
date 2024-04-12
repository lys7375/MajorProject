using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManger : MonoBehaviour
{
    static public int finalScore = 0;
    static public int maxHitChain = 0;
    static public int chain = 0;
    static public int miss = 0;
    static public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ExtractData();
    }

    // 数据本地存入
    public void SaveData(string levelName)
    {
        string dataString = $"{levelName}|{GameManger.finalScore}|{GameManger.maxHitChain}|{GameManger.miss}";

        //Debug.Log(dataString);

        PlayerPrefs.SetString("levelName", levelName);
        PlayerPrefs.Save();

        PlayerPrefs.SetString(levelName, dataString);
        PlayerPrefs.Save();

        if (levelName == "Constant_Moderato")
        {
            UpdateRecord("Constant_Moderato_Record", dataString);
        }
        else if (levelName == "A_Familiar_Sight_and_Leisure")
        {
            UpdateRecord("A_Familiar_Sight_and_Leisure_Record", dataString);
        }
        else if (levelName == "Three-Tone_Composition")
        {
            UpdateRecord("Three-Tone_Composition_Record", dataString);
        }
        else
        {
            UpdateRecord("Three-Tone_Composition_Hard_Record", dataString);
        }

        GameManger.finalScore = 0;
        GameManger.maxHitChain = 0;
        GameManger.miss = 0;
        GameManger.chain = 0;
    }

    // 更新游戏记录
    void UpdateRecord(string key, string data)
    {
        Debug.Log("Update Record");

        if (PlayerPrefs.HasKey(key))
        {

            string record = PlayerPrefs.GetString(key);
            string[] dataPieces = record.Split('|');

            if (dataPieces.Length == 4) // 确保数据拆分正确
            {
                string sceneName = dataPieces[0];
                int finalScore = int.Parse(dataPieces[1]);
                int maxHitChain = int.Parse(dataPieces[2]);
                int miss = int.Parse(dataPieces[3]);

                sceneName = sceneName.Replace("_", " ");

                if (finalScore < GameManger.finalScore)
                {
                    PlayerPrefs.SetString(key, data);
                    PlayerPrefs.Save();
                }
            }
        }
        else
        {
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }
    }

    // 本地数据读取
    public string[] LoadData()
    {
        string key = PlayerPrefs.GetString("levelName");
        string data = PlayerPrefs.GetString(key);
        string[] dataPieces = data.Split('|');

        return dataPieces;
    }

    public void Show(string txt)
    {
        Debug.Log(txt);
    }
}
