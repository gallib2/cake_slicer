using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelsUnlockToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;

    public void SetLevelsUnlocked()
    {
        LevelsManager.areAllLevelsUnlocked = toggle.isOn;
    }
    private void Start()
    {
        SetLevelsUnlocked();
    }
}
