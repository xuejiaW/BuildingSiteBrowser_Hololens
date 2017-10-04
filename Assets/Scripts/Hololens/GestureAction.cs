using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureAction : MonoBehaviour
{
    [Tooltip("How fast the gameobject will rotate by the value of navigation")]
    public float RotationSensitivity = 3f;

    [Range(0,1)]
    [Tooltip("How fast the gameobject will move by the value of manipulation")]
    public float ManipulationSensitivity = 0.05f;

    //when handling the gui or we detected that we are navigating in gestureManager,this flag should be true
    public static bool IsNavigating = false;

    public static bool IsManipulation = false;

    private void Update()
    {
        PerformRotation();
        PerformMove();
    }

    //TODO:Optimize this function,think wheter can remove it from the update function
    private void PerformRotation()
   { 
        if (GestureManager.Instance == null)
        {
            return;
        }

        if (GestureManager.Instance.IsNavigation)
        {
            float rotationFactor = GestureManager.Instance.NavigationRelativePosition.x * RotationSensitivity;
            transform.Rotate(0, -1 * rotationFactor, 0);
            IsNavigating = true;
        }
        else
        {
            IsNavigating = false;
        }
    }

    private void PerformMove()
    {
        GameObject _selected = Interact.SelectedGameObject;

        if (GestureManager.Instance == null)
            return;

        if (_selected == null || _selected != gameObject)
            return;

        if (GestureManager.Instance.IsManipulation)
        {
            Debug.Log("is moving");
            Vector3 deltaVector = GestureManager.Instance.ManipulationRelativePosition - GestureManager.Instance.ManipulationStartPosition;
            Debug.Log("delta position is " + deltaVector * ManipulationSensitivity);
            transform.position += deltaVector * ManipulationSensitivity;
            IsManipulation = true;
        }
        else
        {
            IsManipulation = false;
        }

    }

}
