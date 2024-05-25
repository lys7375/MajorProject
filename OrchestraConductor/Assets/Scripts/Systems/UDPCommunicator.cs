using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class UDPCommunicator : MonoBehaviour
{
    UdpClient client;
    IPEndPoint remoteEndPoint;
    //bool isTrue = true;
    private string receivedText = "";

    static public string leftHandDirection = "";
    static public string rightHandDirection = "";
    static public bool gestureStopSignal = true;
    private int position = 0;

    //private string signal;

    void Start()
    {
        client = new UdpClient(54321); // 监听的端口
        remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080); // Python监听的端口和地址

        //if(gestureStopSignal == true )
        //{
        //    //signal = "Stop";

        //    byte[] data = Encoding.UTF8.GetBytes("Stop");
        //    client.Send(data, data.Length, remoteEndPoint);
        //}
        //else
        //{
        //    //signal = "Run";

        //    byte[] data = Encoding.UTF8.GetBytes("Run");
        //    client.Send(data, data.Length, remoteEndPoint);
        //}

        // 启动一个协程来接收数据
        StartCoroutine(ReceiveData());
    }

    void Update()
    {
        //发送数据
        // 按空格停止python进行手势识别
        //if (Input.GetKeyDown(KeyCode.Space) || gestureStopSignal == true)
        //{
        //    string message = isTrue ? "Stop" : "Run";
        //    byte[] data = Encoding.UTF8.GetBytes(message);
        //    client.Send(data, data.Length, remoteEndPoint);

        //    Debug.Log("Python: " + message);

        //    isTrue = !isTrue;
        //}
    }

    // 获取Python手势识别结果
    private IEnumerator ReceiveData()
    {
        while (true)
        {
            if (client.Available > 0)
            {
                byte[] receivedData = client.Receive(ref remoteEndPoint);
                string receivedText = Encoding.UTF8.GetString(receivedData);
                Debug.Log("Received: " + receivedText + " | " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));

                if (!string.IsNullOrEmpty(receivedText))
                {
                    position = receivedText.IndexOf('|');

                    if (position != -1)
                    {
                        leftHandDirection = receivedText.Substring(0, position);
                        rightHandDirection = receivedText.Substring(position + 1);
                    }
                    else
                    {
                        leftHandDirection = "Null";
                        rightHandDirection = "Null";
                    }
                }
            }
            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        client.Close();
    }

    // 返还Python手势识别数据
    public string GetLastReceivedMessage()
    {
        return receivedText;
    }

    public void DisableGesturerecognition()
    {
        byte[] data = Encoding.UTF8.GetBytes("Stop");
        client.Send(data, data.Length, remoteEndPoint);
    }

    public void EnableStopGesturerecognition()
    {
        byte[] data = Encoding.UTF8.GetBytes("Run");
        client.Send(data, data.Length, remoteEndPoint);
    }
}
