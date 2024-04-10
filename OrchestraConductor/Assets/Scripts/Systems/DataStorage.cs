using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public string levelName;
    public int finalScore;
    public int maxHitChain;
    public int missHit;

    public DataStorage(string levelName, int finalScore, int maxHitChain, int missHit)
    {
        this.levelName = levelName;
        this.finalScore = finalScore;
        this.maxHitChain = maxHitChain;
        this.missHit = missHit;
    }

    public void SaveDataToPlayerPrefs(string key)
    {
        string dataString = $"{levelName}|{finalScore}|{maxHitChain}|{missHit}";
        PlayerPrefs.SetString(key, dataString);
        PlayerPrefs.Save();
    }

    public static DataStorage LoadDataFromPlayerPrefs(string key)
    {
        string dataString = PlayerPrefs.GetString(key);

        if (string.IsNullOrEmpty(dataString))
        {
            Debug.Log("No data found in PlayerPrefs for key: " + key);
            return null;
        }

        string[] values = dataString.Split('|');
        if (values.Length != 4)
        {
            Debug.Log("Invalid data format for key: " + key);
            return null;
        }

        string levelName = values[0];
        int finalScore = int.Parse(values[1]);
        int maxHitChain = int.Parse(values[2]);
        int missHit = int.Parse(values[3]);

        return new DataStorage(levelName, finalScore, maxHitChain, missHit);
    }
}
