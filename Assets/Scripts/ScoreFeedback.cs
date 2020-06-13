using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bonuslessScoreText;
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private GameObject comboObject;
    private float dateOfBirth = 0;
    //[SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    private const float  LIFE_SPAN = 2.5f;
    private const float MAX_RANDOMISED_ANGLE = 18f;

    public void ScoreFeedbackConstructor(int bonuslessScore, int bonus, Color upperFontColour, Color lowerFontColour, Vector3 position)
    {
        dateOfBirth = Time.time;
        bonuslessScoreText.text = bonuslessScore.ToString();
        VertexGradient colourGradient = new VertexGradient(upperFontColour, upperFontColour, lowerFontColour, lowerFontColour);
        bonuslessScoreText.colorGradient = colourGradient;
        if (bonus > 0)
        {
            bonusText.text = "+"+bonus.ToString();
            comboObject.SetActive(true);

        }
        else
        {
            comboObject.SetActive(false);
        }
       
        //spriteRenderer.sprite = sprite;
        transform.position = position;
        transform.rotation = Quaternion.identity;
        float randomAngle = Random.Range(-MAX_RANDOMISED_ANGLE, MAX_RANDOMISED_ANGLE);
        transform.Rotate(Vector3.forward, randomAngle);
        animator.SetTrigger("Play");
    }
 
    void Update()
    {
        if(Time.time - dateOfBirth > LIFE_SPAN)
        {
            Debug.Log(Time.time + " - " + dateOfBirth + " > " + LIFE_SPAN);
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
