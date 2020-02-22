using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] elementsToAppearOnLose;
    [SerializeField]
    private GameObject[] elementsToDisppearOnLose;

    private void Start()
    {
        GameManager.OnLose += ShowLoseScreen;
        GameManager.OnLevelInitialised += HideLoseScreen;
    }

    private void OnDisable()
    {
        GameManager.OnLose -= ShowLoseScreen;
        GameManager.OnLevelInitialised -= HideLoseScreen;
    }

    private void ShowLoseScreen()
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectNames.LOSE);
        for (int i = 0; i < elementsToAppearOnLose.Length; i++)
        {
            elementsToAppearOnLose[i].SetActive(true);
        }
        for (int i = 0; i < elementsToDisppearOnLose.Length; i++)
        {
            elementsToDisppearOnLose[i].SetActive(false);
        }
    }

    private void HideLoseScreen()
    {
        for (int i = 0; i < elementsToAppearOnLose.Length; i++)
        {
            elementsToAppearOnLose[i].SetActive(false);
        }
    }
}
