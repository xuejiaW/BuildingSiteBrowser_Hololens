using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;


public class PictureVideoCapture : Singleton<PictureVideoCapture>
{
    private UnityEngine.XR.WSA.WebCam.PhotoCapture capture;

    private bool isReady = false;

    private string currentImagePath;
    private string pictureFolderPath;

    private void Start()
    {
        UnityEngine.XR.WSA.WebCam.PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated);

#if NETFX_CORE
        getPicturesFolderAsync();
#endif
    }

#if NETFX_CORE
    private async void getPicturesFolderAsync() {
        Windows.Storage.StorageLibrary picturesStorage = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
        pictureFolderPath = picturesStorage.SaveFolder.Path;
    }
#endif

    private void OnPhotoCaptureCreated(UnityEngine.XR.WSA.WebCam.PhotoCapture captureObject)
    {
        capture = captureObject;

        Resolution resolution = UnityEngine.XR.WSA.WebCam.PhotoCapture.SupportedResolutions.OrderByDescending(res => res.width * res.height).First();

        UnityEngine.XR.WSA.WebCam.CameraParameters c = new UnityEngine.XR.WSA.WebCam.CameraParameters(UnityEngine.XR.WSA.WebCam.WebCamMode.PhotoMode);
        c.hologramOpacity = 1.0f;
        c.cameraResolutionWidth = resolution.width;
        c.cameraResolutionHeight = resolution.height;
        c.pixelFormat = UnityEngine.XR.WSA.WebCam.CapturePixelFormat.BGRA32;

        capture.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(UnityEngine.XR.WSA.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        isReady = result.success;
    }


    /// <summary>
    /// Take a photo and save it to a temporary application folder.
    /// </summary>
    public void TakePhoto()
    {
        if (isReady)
        {
            string file = string.Format(@"Image_{0:yyyy-MM-dd_hh-mm-ss-tt}.jpg", DateTime.Now);
            currentImagePath = System.IO.Path.Combine(Application.persistentDataPath, file);

            capture.TakePhotoAsync(currentImagePath, UnityEngine.XR.WSA.WebCam.PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
        }
        else
        {
            Debug.LogWarning("The camera is not yet ready.");
        }
    }

    public void StopCamera()
    {
        if (isReady)
            capture.StopPhotoModeAsync(OnPhotoModeStopped);
    }

    private void OnCapturedPhotoToDisk(UnityEngine.XR.WSA.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
#if NETFX_CORE
            if(pictureFolderPath != null)
                System.IO.File.Move(currentImagePath, System.IO.Path.Combine(pictureFolderPath, "Camera Roll", System.IO.Path.GetFileName(currentImagePath)));

#endif

        }
        else
        {
            Debug.LogError(string.Format("Failed to save photo to disk ({0})", result.hResult));
        }
    }

    private void OnPhotoModeStopped(UnityEngine.XR.WSA.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        capture.Dispose();
        capture = null;
        isReady = false;
    }

}

