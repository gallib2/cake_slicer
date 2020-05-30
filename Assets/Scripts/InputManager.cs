using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static Vector2 lastTouchPosition;
    private static bool isTouchDevice;

    private void Start()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            isTouchDevice = true;
        }
    }

    public static bool GetTouch()
    {
        return isTouchDevice ? ((Input.touchCount > 0) && (Input.GetTouch(0).phase != TouchPhase.Ended)) : Input.GetMouseButton(0);
    }

    public static bool GetTouchDown()
    {
        return isTouchDevice ? (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                : Input.GetMouseButtonDown(0);
    }

    public static bool GetTouchUp()
    {
        return isTouchDevice ? 
                (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended)) : Input.GetMouseButtonUp(0);
    }

    public static Vector2 GetTouchPosition()
    {
        if (!isTouchDevice)
        {
            return lastTouchPosition = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            return lastTouchPosition = Input.GetTouch(0).position;
        }
        return lastTouchPosition;//TODO: make it nullable?
    }
}
