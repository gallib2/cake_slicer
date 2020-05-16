using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionScroller : MonoBehaviour
{
    private void Start()
    {
        LevelSelectButton buttonToFocusOn = null;
        LevelSelectButton[] buttons = FindObjectsOfType<LevelSelectButton>();//TODO: make sure we (Gal especialy) are okay with this function
        foreach (LevelSelectButton button in buttons)
        {
            if(button.LevelIndex == LevelsManager.CurrentLevelNumber)
            {
                buttonToFocusOn = button;
            }
        }
        if(buttonToFocusOn == null)
        {
            Debug.LogWarning("Could not find button to focus on!");
            return;
        }

        float buttonY = buttonToFocusOn.transform.localPosition.y;
        this.transform.localPosition = new Vector3(
            this.transform.localPosition.x, -buttonY, this.transform.localPosition.z);
        //Debug.Log("buttonY = " + buttonY);
    }
}
