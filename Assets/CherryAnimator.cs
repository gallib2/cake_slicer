using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    //private int previousTime;
    // Update is called once per frame
    private void Start()
    {
        Invoke("PlayAnimation", Random.Range(0, 0.1f));
    }

    private void PlayAnimation()
    {
        animator.SetTrigger("Beat");
    }
    void Update()
    {
       /* int time = (int)Time.time;
        if (previousTime != time)
        {
            if (Random.Range(0, 6) == 1)
            {
                animator.SetTrigger("Beat");
            }

            previousTime = time;
        }*/

    }
}
