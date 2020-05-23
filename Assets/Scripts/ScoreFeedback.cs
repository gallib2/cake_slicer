using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bonuslessScoreText;
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private GameObject comboObject;
    private float dateOfBirth=0;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float lifeSpan = 3f;


    public void ScoreFeedbackConstructor(int bonuslessScore, int bonus, Sprite sprite,Vector3 position)
    {
        dateOfBirth = Time.time;
        bonuslessScoreText.text = bonuslessScore.ToString();
        if (bonus > 0)
        {
            bonusText.text = "+"+bonus.ToString();
        }
        else
        {
            comboObject.SetActive(false);
        }
       
        spriteRenderer.sprite = sprite;
        transform.position = position;
    }
 
    void Update()
    {
        if(Time.time - dateOfBirth > lifeSpan)
        {
            Debug.Log(Time.time + " - " + dateOfBirth + " > " + lifeSpan);
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
