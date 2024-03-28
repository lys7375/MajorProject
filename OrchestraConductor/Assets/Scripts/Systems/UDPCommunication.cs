using UnityEngine;
using UnityEngine.UI;

public class UDPCommunication : MonoBehaviour
{    
    public RawImage rawImage;
    private WebCamTexture webCamTexture;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("UDPCommunication Run");

        WebCamDevice[] devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture(devices[0].name);
        webCamTexture.Play();

        rawImage.texture = webCamTexture;
        rawImage.rectTransform.localScale = new Vector3(-1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
