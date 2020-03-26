using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FunSlicingToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;

    public void SetFunSlicing()
    {
        GameManager.FunSlicing = toggle.isOn;
    }
    private void Start()
    {
        SetFunSlicing();
    }
}
