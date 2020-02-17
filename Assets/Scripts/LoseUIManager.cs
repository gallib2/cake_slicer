using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] elementsToAppearOnWin;
    [SerializeField]
    private GameObject[] elementsToDisppearOnWin;

    private void Start()
    {
        GameManager.OnLose += ShowLoseScreen;
        for (int i = 0; i < elementsToAppearOnWin.Length; i++)
        {
            elementsToAppearOnWin[i].SetActive(false);
        }
    }
    private void OnDisable()
    {
        GameManager.OnLose -= ShowLoseScreen;
    }
    private void ShowLoseScreen()
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectNames.LOSE);
        for (int i = 0; i < elementsToAppearOnWin.Length; i++)
        {
            elementsToAppearOnWin[i].SetActive(true);
        }
        for (int i = 0; i < elementsToDisppearOnWin.Length; i++)
        {
            elementsToDisppearOnWin[i].SetActive(false);
        }
    }
}
