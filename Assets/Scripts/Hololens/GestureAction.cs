using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureAction : MonoBehaviour
{
    [Tooltip("How fast the gameobject will rotate by the value of navigation")]
    public float RotationSensitivity = 3f;

    [Range(0, 1)]
    [Tooltip("How fast the gameobject will move by the value of manipulation")]
    public float ManipulationSensitivity = 0.05f;

    [HideInInspector]
    public bool IsNavigating = false;

    [HideInInspector]
    public bool IsManipulating = false;

    private void Update()
    {
        if (GestureManager.Instance == null || Interact.SelectedGameObject==null)
            return;

        PerformRotation();
        PerformMove();
    }

    private void PerformRotation()
    {
        GameObject _selected = Interact.SelectedGameObject;
        IsNavigating = GestureManager.Instance.IsNavigation;
        if (GestureManager.Instance.IsNavigation)
        {
            float rotationFactor = GestureManager.Instance.NavigationRelativePosition.x * RotationSensitivity;
            Interact.SelectedGameObject.transform.Rotate(0, -1 * rotationFactor, 0);
        }
    }

    private void PerformMove()
    {
        GameObject _selected = Interact.SelectedGameObject;
        IsManipulating = GestureManager.Instance.IsManipulation;
        if (GestureManager.Instance.IsManipulation)
        {
            Vector3 deltaVector = GestureManager.Instance.ManipulationRelativePosition - GestureManager.Instance.ManipulationStartPosition;
            Interact.SelectedGameObject.transform.position += deltaVector * ManipulationSensitivity;
        }
    }

}

