using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QRCodeDetector : Singleton<QRCodeDetector>
{
    //TODO: low fps while the detector is running
    public GameObject QRCodeUI_Prefab;
    private GameObject QRCodeUI;
    private Text QRCodeText;

    private Color32[] data { get; set; }
    private WebCamTexture webCameraTexture;
    private BarcodeReader QRCodeReader;

    private IEnumerator scanQRCodeCoroutine;

    public delegate void DetectedQRCode(string CodeResult);
    public event DetectedQRCode OnDetectedQRCode;

    private void Start()
    {
        scanQRCodeCoroutine = ScanQRCode();
        StartCoroutine(InitWebCamera());

        //GestureManager.Instance.OnDoubleClick += tryOpenQRCodeDetector;
    }

    public void tryOpenQRCodeDetector()
    {
        if (webCameraTexture == null)
            return;

        if (webCameraTexture.isPlaying)
            stopQRCodeDetector();
        else
            openQRCodeDetector();
    }

    private void openQRCodeDetector()
    {
        webCameraTexture.Play();
        QRCodeUI = Instantiate(QRCodeUI_Prefab);
        InitQRCodeUIComponent();
        CursorManager.Instance.HideCursor = true;
        StartCoroutine(scanQRCodeCoroutine);
    }

    private void stopQRCodeDetector()
    {
        webCameraTexture.Stop();
        Destroy(QRCodeUI);
        CursorManager.Instance.HideCursor = false;
        StopCoroutine(scanQRCodeCoroutine);
    }

    private void InitQRCodeUIComponent()
    {
        if (QRCodeUI == null)
            return;

        UIUtils.SetEachZTestMode(QRCodeUI, GUIZTestMode.Always);
        QRCodeUI.GetComponent<Canvas>().worldCamera = Camera.main;
        QRCodeText = QRCodeUI.transform.Find("ScanResult").GetComponent<Text>();
    }

    private IEnumerator ScanQRCode()
    {
        QRCodeReader = new BarcodeReader();
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            data = webCameraTexture.GetPixels32();//相机捕捉到的纹理  
            Result _result = QRCodeReader.Decode(data, webCameraTexture.width, webCameraTexture.height);
            if (_result != null)
                DetectedQRHandle(_result);
        }
    }

    private void DetectedQRHandle(Result _QRResult)
    {
        QRCodeText.text = _QRResult.Text;
        stopQRCodeDetector();

        if (OnDetectedQRCode != null)
            OnDetectedQRCode(_QRResult.Text);
    }


    private IEnumerator InitWebCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
                yield break;
            string devicename = devices[0].name;

            webCameraTexture = new WebCamTexture(devicename, Screen.width, Screen.height);
        }
    }
    private void OnDestroy()
    {
        //GestureManager.Instance.OnDoubleClick -= tryOpenQRCodeDetector;
    }
}
