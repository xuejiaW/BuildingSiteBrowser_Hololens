using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUIManager : Singleton<GeneralUIManager>
{
    private void Start()
    {

        GestureManager.Instance.OnDoubleClick += SwitchGameObjectActive;

        UIIcon[] UIIcons = GetComponentsInChildren<UIIcon>();
        foreach (var icon in UIIcons)
        {
            icon._OnClick += (()=> { gameObject.SetActive(false); });
        }


        transform.Find("TakePhote").GetComponent<UIIcon>()._OnClick +=(()=> 
        {
            Debug.Log("Click the take Photo");
            PictureVideoCapture.Instance.TakePhoto();
        });

        transform.Find("StartVideo").GetComponent<UIIcon>()._OnClick += (() =>
        {

        });

        transform.Find("StopVideo").GetComponent<UIIcon>()._OnClick += (() =>
        {

        });

        transform.Find("QRCode").GetComponent<UIIcon>()._OnClick += (() => 
        {
            QRCodeDetector.Instance.tryOpenQRCodeDetector();
        });
    }

    private void SwitchGameObjectActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        GestureManager.Instance.OnDoubleClick -= SwitchGameObjectActive;
    }
}
