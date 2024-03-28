using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UDPCommunicator : MonoBehaviour
{
    public string videoTargetIP = "localhost";
    public int videoTargetPort = 8080;
    public int messageListenPort = 55555;

    [Space]
    public float delay = 0.5f; // 设定发送数据的频率，例如每0.1s发送一次    

    [Space]
    public RawImage rawImage;

    private UdpClient udpSendClient; // 用于发送数据的UDP客户端
    private UdpClient udpListenClient; // 用于接收数据的UDP客户端
    private Thread listenThread; // 用于监听的线程

    private float elapsedTime = 0.0f; // 用于计时    
    private string lastReceivedMessage = string.Empty;

    private WebCamTexture cam; // Web摄像头
    private Texture2D texture; // 视频帧

    private void Start()
    {
        udpSendClient = new UdpClient();

        // 初始化视频处理
        cam = new WebCamTexture();
        rawImage.texture = cam; // 二维材质
        cam.Play();
        texture = new Texture2D(cam.width, cam.height, TextureFormat.RGB24, false);

        // 初始化信息处理
        udpListenClient = new UdpClient(messageListenPort);
        listenThread = new Thread(ListenThreadMethod);
        listenThread.Start();
    }

    /// <summary>
    ///经过一段间隔通过UDP发送图像信息
    /// </summary>
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > delay)
        {
            SendFrame(cam);
            elapsedTime = 0.0f;
        }
    }

    private void OnDestroy()
    {
        listenThread.Abort();
        udpListenClient.Close();
    }

    private void ListenThreadMethod()
    {
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), messageListenPort);

        while (true)
        {
            byte[] data = udpListenClient.Receive(ref anyIP);
            Debug.Log("Data received: " + data.Length + " bytes");
            lastReceivedMessage = Encoding.ASCII.GetString(data); // 得到消息
            Debug.Log("Data received: " + lastReceivedMessage);
        }
    }

    private void SendFrame(WebCamTexture cam)
    {
        texture.SetPixels(cam.GetPixels());
        texture.Apply();
        byte[] frameData = texture.EncodeToJPG();
        udpSendClient.Send(frameData, frameData.Length, videoTargetIP, videoTargetPort);
    }
}