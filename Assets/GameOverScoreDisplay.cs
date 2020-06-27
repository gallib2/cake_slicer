using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    private void OnEnable()
    {
        SetText();
    }

    public void SetText()
    {
        this.text.text = Score.score.ToString();
    }
}
