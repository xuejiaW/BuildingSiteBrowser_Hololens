using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    private enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Backward
    }

    private IEnumerator _forward;
    private IEnumerator _back;
    private IEnumerator _left;
    private IEnumerator _right;
    private IEnumerator _up;
    private IEnumerator _down;

    private IEnumerator currentCoroutine;

    private Vector3 rightDirection { get { return Camera.main.transform.right; } }
    private Vector3 upDirection { get { return Vector3.up; } }
    private Vector3 forwardDirection { get { return Vector3.Cross(rightDirection, upDirection); } }

    private void Start()
    {
        UIUtils.SetEachZTestMode(gameObject, GUIZTestMode.Always);
        UIIcon[] UIIcons = transform.GetComponentsInChildren<UIIcon>();
        for (MoveDirection direction = MoveDirection.Up; direction <= MoveDirection.Backward; ++direction)
        {
            int index = (int)direction;
            MoveDirection _moveDirection = direction;

            UIIcons[index]._OnClick += (() => {Interact.SelectedGameObject.transform.Translate(GetMoveDirection(_moveDirection) * 0.1f);});

        }    
    }

    private IEnumerator moveFunction(Vector3 direction)
    {
        Debug.Log("Start move Function");
        while (true)
        {
            Debug.Log("is moveing");
            if (Interact.SelectedGameObject != null)
                Interact.SelectedGameObject.transform.Translate(direction * 0.1f);
            yield return null;
        }
    }

    private IEnumerator GetMoveCoroutine(MoveDirection direction)
    {
        return moveFunction(GetMoveDirection(direction));
    }

    private Vector3 GetMoveDirection(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                return upDirection;
            case MoveDirection.Down:
                return -upDirection;
            case MoveDirection.Left:
                return -rightDirection;
            case MoveDirection.Right:
                return rightDirection;
            case MoveDirection.Forward:
                return forwardDirection;
            case MoveDirection.Backward:
                return -forwardDirection;
            default:
                return new Vector3();
        }
    }
}
