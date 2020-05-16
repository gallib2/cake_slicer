using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const bool testingOnPersonalComputer = false;
    public static bool GetTouch()
    {
        return (testingOnPersonalComputer ? Input.GetMouseButton(0) : ((Input.touchCount > 0) && (Input.GetTouch(0).phase != TouchPhase.Ended)));
    }

    public static bool GetTouchDown()
    {
        return (testingOnPersonalComputer ? 
               Input.GetMouseButtonDown(0) : (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began));
    }

    public static bool GetTouchUp()
    {
        return (testingOnPersonalComputer ? 
               Input.GetMouseButtonUp(0) : (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended)));
    }

    public static Vector2 GetTouchPosition()
    {
        if (testingOnPersonalComputer)
        {
            return Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            return (Input.GetTouch(0).position);
        }
        return Vector2.zero;//TODO: make it nullable?
    }
}
