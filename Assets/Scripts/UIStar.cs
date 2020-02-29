using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStar : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite emptySprite;
    [SerializeField]
    private Sprite fullSprite;
    public RectTransform rectTransform;
    private void Start()
    {
        image.sprite = emptySprite;
    }
    public void FillStar()
    {
        image.sprite = fullSprite;
        // image.color = Color.yellow;
    }

}
