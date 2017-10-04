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
        UIManager.Instance.transform.parent = SelectedGameObject.transform;
        UIManager.Instance.transform.localPosition = new Vector3(-3.5f, 0,0);
        UIManager.Instance.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        Vector3 worldPosition = UIManager.Instance.transform.position;
        UIManager.Instance.transform.parent = null;
        UIManager.Instance.transform.position = worldPosition;

        SelectedGameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;

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
        toBeCanceled.GetComponent<MeshRenderer>().materials[0].color = Color.white;
    }
}
