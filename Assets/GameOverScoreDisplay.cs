using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    private void OnEnable()
    {
        GameManager.OnGameOver += SetText;
    }

    private void OnDisable()
    {
        //TODO: solve synchronisation issues
        //GameManager.OnGameOver -= SetText;
    }

    private void SetText(int score)
    {
        this.text.text = score.ToString();
    }
}
