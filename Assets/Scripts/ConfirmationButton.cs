using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConfirmationButton : MonoBehaviour
{
    public Action ClickAction;
    public void Click()
    {
        ClickAction();
    }
}
