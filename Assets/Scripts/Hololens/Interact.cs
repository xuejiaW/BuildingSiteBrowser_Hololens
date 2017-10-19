using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the gaze message
/// </summary>
public class Interact : MonoBehaviour
{
    private static Interact[] InteractibleObject;
    private static GameObject _Parent;

    private static GameObject _selectedGameObject;
    public static GameObject SelectedGameObject
    {
        get{return _selectedGameObject;}
        private set{_selectedGameObject = value;}
    }


    private void InitInteractibleObject()
    {
        if (InteractibleObject != null)
            return;
        //Find all brother
        Transform _parent = transform.parent;
        InteractibleObject = _parent.GetComponentsInChildren<Interact>();
    }

    private void Start()
    {
        if (_Parent == null)
            _Parent = transform.parent.gameObject;

        InitInteractibleObject();
    }

    void OnSelect()
    {
        bool _cancelSelected = SelectedGameObject == gameObject;

        if (_cancelSelected)
            CancelProcess(_selectedGameObject);
        else
            SelectProcess();
    }

    private void SelectProcess()
    {

        SelectedGameObject = gameObject;
        AdjustUIPanel.Instance.gameObject.SetActive(true); ;

        for (int index = 0; index != InteractibleObject.Length; ++index)
        {
            if(InteractibleObject[index].gameObject!=gameObject)
                CancelProcess(InteractibleObject[index].gameObject);
        }
        //the GUI of Panel actually don't need switch recognizer,so this can use for rotation
    }

    private void CancelProcess(GameObject toBeCanceled)
    {
        if (toBeCanceled == null)
            return;
        Debug.Log("Enter Cancel Process  "+toBeCanceled.name);
        if (toBeCanceled == _selectedGameObject)
        {
            _selectedGameObject = null;
            AdjustUIPanel.Instance.gameObject.SetActive(false);
        }
    }
}
