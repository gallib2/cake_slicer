using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBar : MonoBehaviour
{
    public Animator anim;


    public void StopCriticalAnimation()
    {
        anim.SetBool("isCritical", false);
    }
}
