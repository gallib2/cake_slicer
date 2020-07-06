using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static Vector2 lastTouchPosition;
    private static bool isTouchDevice;
    private static bool currentTouchIsIlegitimate=false;

    private void Start()
    {
        /*if(SystemInfo.deviceType == DeviceType.Handheld)
        { isTouchDevice = true; }*/
        isTouchDevice = (SystemInfo.deviceType == DeviceType.Handheld);
    }

    public static bool GetTouch()
    {
        if (currentTouchIsIlegitimate)
        {
            return false;
        }
        else
        {
            return isTouchDevice ?
                ((Input.touchCount > 0) && (Input.GetTouch(0).phase != TouchPhase.Ended)) 
                : Input.GetMouseButton(0);
        }
    }

    public static bool GetTouchDown()
    {
        return isTouchDevice ? 
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) : Input.GetMouseButtonDown(0);
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
            return lastTouchPosition = Input.GetTouch(0).position;//TODO: check what the position is on touch end
        }
        return lastTouchPosition;//TODO: make it nullable?
    }

    public static void DelegitimiseCurrentTouch()//TODO: insert into an event?
    {
        currentTouchIsIlegitimate = true;
    }

    private void Update()
    {
        if (GetTouchDown())
        {
            //Debug.Log("currentTouchIsIlegitimate = false");
            currentTouchIsIlegitimate = false;
        }

       /* Debug.Log("--------");
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Mouse Pressed");

        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Up");
        }*/

    }
}
