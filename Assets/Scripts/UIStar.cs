using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite emptySprite;//TODO: Move to a sprite holder if you wanna save some memry
    [SerializeField] private Sprite fullSprite;
    public RectTransform rectTransform;
    [SerializeField] private Animator animator;

    private void Start()
    {
        image.sprite = emptySprite;
    }
    public void FillStar()
    {
        image.sprite = fullSprite;
        animator.SetTrigger("Pop");
        // image.color = Color.yellow;
    }

}
