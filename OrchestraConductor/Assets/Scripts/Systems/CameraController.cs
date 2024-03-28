using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public RawImage rawImage; // 在Unity编辑器中将RawImage对象分配给此变量

    private WebCamTexture webCamTexture;

    // Start is called before the first frame update
    void Start()
    {
        // 获取设备上的摄像头
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            // 选择第一个摄像头（通常是后置摄像头）
            webCamTexture = new WebCamTexture(devices[0].name);
            // 将摄像头纹理分配给RawImage对象
            rawImage.texture = webCamTexture;
            // 根据需要水平翻转图像
            rawImage.rectTransform.localScale = new Vector3(-1, 1, 1);
            // 开始捕捉摄像头图像
            webCamTexture.Play();
        }
        else
        {
            Debug.LogError("No webcam found on this device.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
