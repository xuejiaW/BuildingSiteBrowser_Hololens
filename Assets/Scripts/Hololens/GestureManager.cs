using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GestureManager : Singleton<GestureManager>
{
    public GestureRecognizer ActiveRecognizer{get;private set;}
    public GestureRecognizer ManipulationRecognizer{get;private set;}

    public delegate void DoubleClickHandler();
    public event DoubleClickHandler OnDoubleClick;
    public delegate void SingleClickHandler();
    public event SingleClickHandler OnSingleClick;

    private void OnEnable()
    {
        ManipulationRecognizer = new GestureRecognizer();
        ManipulationRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap);
        ManipulationRecognizer.TappedEvent += Recognizer_TappedEvent;

        SwitchRecognizer(ManipulationRecognizer);
    }

    public void SwitchRecognizer(GestureRecognizer newRecognizer)
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

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
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
