using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManger : MonoBehaviour
{
    public UDPCommunicator udpComm;

    private string leftHandDirection = "";
    private string rightHandDirection = "";
    private int position = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ExtractData();
    }

    // 获取UDPCommunicator发送的后端数据后进行拆分
    private void ExtractData()
    {
        if(udpComm != null)
        {
            string receivedMessage = udpComm.GetlastReceivedMessage();

            if(!string.IsNullOrEmpty(receivedMessage))
            {
                position = receivedMessage.IndexOf('|');

                if(position != -1)
                {
                    leftHandDirection = receivedMessage.Substring(0, position);
                    rightHandDirection = receivedMessage.Substring(position + 1);
                }
            }
            else
            {
                leftHandDirection = "";
                rightHandDirection = "";
            }

            Debug.Log("udpComm.GetlastReceivedMessage(): " + receivedMessage + "left: " + leftHandDirection + " |  right: " + rightHandDirection);
        }
    }
}
