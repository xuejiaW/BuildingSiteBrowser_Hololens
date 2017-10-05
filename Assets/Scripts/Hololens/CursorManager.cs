using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manager the Cursor State
/// </summary>
public class CursorManager : Singleton<CursorManager>
{
    [Tooltip("The cursor when it hits some gameobjects")]
    public GameObject CursorOn;

    [Tooltip("The cursor when it doesn't hit any gameobjects ")]
    public GameObject CursorOff;

    [Tooltip("The cursor when detected hand ")]
    public GameObject CursorHand;

    private bool _hideCursor;
    public bool HideCursor
    {
        private get { return _hideCursor; }
        set
        {
            _hideCursor = value;
            if (_hideCursor)
            {
                CursorOn.SetActive(false);
                CursorOff.SetActive(false);
                CursorHand.SetActive(false);
            }
            else
                UpdateCursorState();
        }
    }

	void Start ()
    {
        if (CursorOn == null || CursorOff == null || CursorHand == null)
        {
            Debug.LogError("THE CURSOR IS NOT SET");
            return;
        }

        CursorOn.SetActive(false);
        CursorOff.SetActive(false);
        CursorHand.SetActive(false);
	}

    private void Update()
    {
        if (GazeManager.Instance == null)
            return;
        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        transform.position = GazeManager.Instance.HitPosition;
        transform.up = GazeManager.Instance.HitNormal;

        if (CursorOn == null || CursorOff == null || CursorHand==null)
            return;

        CursorHand.SetActive(HandsManager.Instance.HandDetected && !HideCursor);
        CursorOn.SetActive(GazeManager.Instance.Hit && !HandsManager.Instance.HandDetected &&!HideCursor);
        CursorOff.SetActive(!GazeManager.Instance.Hit && !HandsManager.Instance.HandDetected && !HideCursor);
    }
}
