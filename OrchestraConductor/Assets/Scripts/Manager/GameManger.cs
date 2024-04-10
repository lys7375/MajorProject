using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManger : MonoBehaviour
{
    static public int finalScore = 0;
    static public int maxHitChain = 0;
    static public int miss = 0;


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
    public void SaveData(string levelName, int score, int hitChain, int missHit)
    {
        DataStorage data = new DataStorage(levelName, score, hitChain, missHit);

        data.SaveDataToPlayerPrefs(levelName);
    }

    // 本地数据读取
    public DataStorage LoadData(string levelName)
    {
        DataStorage data = DataStorage.LoadDataFromPlayerPrefs(levelName);

        return data;
    }

    //// 获取UDPCommunicator发送的后端数据后进行拆分
    //private void ExtractData()
    //{
    //    if(udpComm != null)
    //    {
    //        string receivedMessage = udpComm.GetLastReceivedMessage();

    //        if(!string.IsNullOrEmpty(receivedMessage))
    //        {
    //            position = receivedMessage.IndexOf('|');

    //            if(position != -1)
    //            {
    //                leftHandDirection = "L" + receivedMessage.Substring(0, position);
    //                rightHandDirection = "R" + receivedMessage.Substring(position + 1);
    //            }
    //        }
    //        else
    //        {
    //            leftHandDirection = "";
    //            rightHandDirection = "";
    //        }

    //        Debug.Log("Receive left: " + leftHandDirection + " |  right: " + rightHandDirection);
    //        //Debug.Log("udpComm.GetlastReceivedMessage(): " + receivedMessage + "left: " + leftHandDirection + " |  right: " + rightHandDirection);
    //    }
    //}
}
