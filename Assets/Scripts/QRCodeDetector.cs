using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using UnityEngine.UI;

public class QRCodeDetector : MonoBehaviour {

    private Color32[] data { get; set; }
    public RawImage cameraTexture;
    public Text QRcodeText;
    private WebCamTexture webCameraTexture;
    private BarcodeReader barcodeReader;

    IEnumerator Start()
    {
        Debug.Log("Enter QRCode Start");
        barcodeReader = new BarcodeReader();
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);//请求授权使用摄像头  
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            string devicename = devices[0].name;
            webCameraTexture = new WebCamTexture(devicename, 400, 300);
            cameraTexture.texture = webCameraTexture;
            webCameraTexture.Play();
            StartCoroutine(ScanQRCode());
        }

    }

    private IEnumerator ScanQRCode()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            data = webCameraTexture.GetPixels32();//相机捕捉到的纹理  
            DecodeQR(webCameraTexture.width, webCameraTexture.height);
        }
    }


    private void DecodeQR(int width, int height)
    {
        Result br = barcodeReader.Decode(data, width, height);

        if (br != null)
            QRcodeText.text = br.Text;

    }
}
