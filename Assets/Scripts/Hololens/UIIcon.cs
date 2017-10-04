using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIIcon : Graphic,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler,IPointerDownHandler
{
    public delegate void OnClick();
    public event OnClick _OnClick;
    public delegate void OnEnter();
    public event OnEnter _OnEnter;
    public delegate void OnExit();
    public event OnEnter _OnExit;
    public delegate void OnDown();
    public event OnDown _OnDown;
    public delegate void OnUp();
    public event OnUp _OnUp;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_OnClick != null)
            _OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_OnEnter != null)
            _OnEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_OnExit != null)
            _OnExit();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_OnUp != null)
            _OnUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_OnDown != null)
            _OnDown();
    }
}
