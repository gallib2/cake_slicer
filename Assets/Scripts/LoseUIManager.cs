using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] elementsToAppearOnLose;
    [SerializeField]
    private GameObject[] elementsToDisppearOnLose;
    [SerializeField]
    private SoundManager soundManager;

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
        Debug.Log("ShowLoseScreen");
        soundManager.PlaySoundEffect(SoundEffectNames.LOSE);
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
