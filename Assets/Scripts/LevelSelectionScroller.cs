using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectionScroller : MonoBehaviour
{
    private static bool appJustOpened = true;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject[] objectsInvisibleWhileOnMainMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private float scrollSmoothDampTime = 0.4f;

    private void Start()
    {
        if (appJustOpened)
        {
            ShowMainMenu();
            appJustOpened = false;
        }
        else
        {
            GoToCurrentLevelButtonInstantly();
        }
    }

    private float GetCurrentLevelButtonY()
    {
        float y = 0;
        LevelSelectButton buttonToFocusOn = null;
        LevelSelectButton[] buttons = FindObjectsOfType<LevelSelectButton>();//TODO: make sure we (Gal especialy) are okay with this function
        foreach (LevelSelectButton button in buttons)
        {
            if (button.LevelIndex == LevelsManager.CurrentLevelNumber)
            {
                buttonToFocusOn = button;
            }
        }
        if (buttonToFocusOn == null)
        {
            Debug.LogWarning("Could not find button to focus on!");
        }
        else
        {
            y = buttonToFocusOn.transform.localPosition.y;
        }

        return y;
    }

    private void GoToCurrentLevelButtonInstantly()
    {
        float buttonY = GetCurrentLevelButtonY();
        this.transform.localPosition = new Vector3(
            this.transform.localPosition.x, -buttonY, this.transform.localPosition.z);
        //Debug.Log("buttonY = " + buttonY);
    }

    private void ShowMainMenu()
    {
        for (int i = 0; i < objectsInvisibleWhileOnMainMenu.Length; i++)
        {
            objectsInvisibleWhileOnMainMenu[i].SetActive(false);
        }
        scrollRect.enabled = false;

        float y = mainMenu.transform.localPosition.y;
        this.transform.localPosition = new Vector3(
            this.transform.localPosition.x, -y, this.transform.localPosition.z);
    }

    public void GoToLevelsSelectionScreen()
    {
        destination = new Vector2(
            this.transform.localPosition.x, -GetCurrentLevelButtonY());
        isScrollingToCurrentLevelButtonY = true;
    }

    //Bookeeping:
    private Vector2 destination;
    private bool isScrollingToCurrentLevelButtonY = false;
    Vector2 velocity;

    private void Update()
    {
        if (isScrollingToCurrentLevelButtonY)
        {

            Vector2 newPosition = Vector2.SmoothDamp(this.transform.localPosition, destination, ref velocity,scrollSmoothDampTime);
            this.transform.localPosition = newPosition;
            if( Vector2.Distance(newPosition, destination) < 0.8f)
            {
                for (int i = 0; i < objectsInvisibleWhileOnMainMenu.Length; i++)
                {
                    objectsInvisibleWhileOnMainMenu[i].SetActive(true);
                }
                isScrollingToCurrentLevelButtonY = false;
                scrollRect.enabled = true;
            }
        }
    }
}
