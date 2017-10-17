using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GestureManager : Singleton<GestureManager>
{
    public UnityEngine.XR.WSA.Input.GestureRecognizer ActiveRecognizer{get;private set;}
    public UnityEngine.XR.WSA.Input.GestureRecognizer ManipulationRecognizer{get;private set;}

    public delegate void DoubleClickHandler();
    public event DoubleClickHandler OnDoubleClick;
    public delegate void SingleClickHandler();
    public event SingleClickHandler OnSingleClick;

    private void OnEnable()
    {
        ManipulationRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        ManipulationRecognizer.SetRecognizableGestures(UnityEngine.XR.WSA.Input.GestureSettings.Tap | UnityEngine.XR.WSA.Input.GestureSettings.DoubleTap);
        ManipulationRecognizer.TappedEvent += Recognizer_TappedEvent;

        SwitchRecognizer(ManipulationRecognizer);
    }

    public void SwitchRecognizer(UnityEngine.XR.WSA.Input.GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
            return;
        if (ActiveRecognizer != null)
        {
            if (ActiveRecognizer == newRecognizer)
                return;
            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();
        }
        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
    }

    private void Recognizer_TappedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay)
    {;
        if (tapCount == 1 && OnSingleClick != null)
            OnSingleClick();
        if (tapCount == 2 && OnDoubleClick != null)
            OnDoubleClick();
    }
    
    private void OnDisable()
    {
        ManipulationRecognizer.TappedEvent -= Recognizer_TappedEvent;
    }

}
