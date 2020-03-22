using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopUpStar : MonoBehaviour
{
    [SerializeField]
    private GameObject star;
    public void FillStar()
    {
        star.SetActive(true);
        star.GetComponent<Animator>().SetTrigger("FillStar");
    }
    public void EmptyStar()
    {
        star.SetActive(false);
    }
}
