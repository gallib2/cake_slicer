using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Swapper : MonoBehaviour
{

    public GameObject[] Swapped_Objects;

    public void swap()
    {
        for (int i = 0; i < Swapped_Objects.Length; i++)
        {
            Swapped_Objects[i].SetActive(!Swapped_Objects[i].active);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
