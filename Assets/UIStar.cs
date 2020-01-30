using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStar : MonoBehaviour
{
    [SerializeField]
    private Image image;
    public RectTransform rectTransform;
    public void FillStar()
    {
        image.color = Color.yellow;
    }

}
