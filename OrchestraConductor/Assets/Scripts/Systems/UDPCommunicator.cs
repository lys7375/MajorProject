using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPCommunicator : MonoBehaviour
{
    UdpClient client;
    IPEndPoint remoteEndPoint;
    bool isTrue = true;
    private string receivedText = "";

    void Start()
    {
        client = new UdpClient(54321); // 监听的端口
        remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080); // Python监听的端口和地址

        // 启动一个协程来接收数据
        StartCoroutine(ReceiveData());
    }

    void Update()
    {
        // 按需发送数据
        if (Input.GetKeyDown(KeyCode.Space)) // 示例：按空格键发送数据
        {
            string message = isTrue ? "Stop" : "Run";
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);

            Debug.Log("Python: " + message);
            // 切换布尔变量的值
            isTrue = !isTrue;
        }
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
                Debug.Log("Received: " + receivedText);
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
}
