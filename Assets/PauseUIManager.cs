﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] elementsToAppearOnPause;
    [SerializeField] private GameObject[] elementsToDisppearOnPause;

    private void Awake()
    {
        // HideWinScreen();//Should be called by OnLevelInitialised though
    }

    public void ChangeState(bool to)
    {
        if (to)
        {
            ShowPauseScreen();
        }
        else
        {
            HidePauseScreen();
        }
    }

    private void ShowPauseScreen()
    {
        for (int i = 0; i < elementsToAppearOnPause.Length; i++)
        {
            elementsToAppearOnPause[i].SetActive(true);
        }
        for (int i = 0; i < elementsToDisppearOnPause.Length; i++)
        {
            elementsToDisppearOnPause[i].SetActive(false);
        }
    }

    public void HidePauseScreen()
    {
        for (int i = 0; i < elementsToAppearOnPause.Length; i++)
        {
            elementsToAppearOnPause[i].SetActive(false);
        }
        for (int i = 0; i < elementsToDisppearOnPause.Length; i++)
        {
            elementsToDisppearOnPause[i].SetActive(true);
        }
    }

}
