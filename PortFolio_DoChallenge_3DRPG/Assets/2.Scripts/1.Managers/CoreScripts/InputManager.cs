using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Defines;

public class InputManager
{
    public Action KeyAction = null;
    public Action<MouseEvent> LeftAction = null;
    public Action<MouseEvent> RightAction = null;
    bool LPress = false;
    bool RPress = false;
    float LPressTime = 0;
    float RPressTime = 0;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if(Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if(LeftAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!LPress)
                {
                    LeftAction.Invoke(MouseEvent.PointerDown);
                    LPressTime = Time.time;
                }
                LeftAction.Invoke(MouseEvent.Press);
                LPress = true;
            }
            else
            {
                if (LPress)
                {
                    if (Time.time > LPressTime + 0.25f)
                        LeftAction.Invoke(MouseEvent.Click);
                    LeftAction.Invoke(MouseEvent.PointerUp);
                }
                LPress = false;
                LPressTime = 0;
            }
        }
        if (RightAction != null)
        {
            if (Input.GetMouseButton(1))
            {
                if (!RPress)
                {
                    RightAction.Invoke(MouseEvent.PointerDown);
                    RPressTime = Time.time;
                }
                RightAction.Invoke(MouseEvent.Press);
                RPress = true;
            }
            else
            {
                if (RPress)
                {
                    if (Time.time > RPressTime + 0.25f)
                        RightAction.Invoke(MouseEvent.Click);
                    RightAction.Invoke(MouseEvent.PointerUp);
                }
                RPress = false;
                RPressTime = 0;
            }
        }
    }

    public void Clear()
    {
        LeftAction = null;
        RightAction = null;
        KeyAction = null;
    }
}
