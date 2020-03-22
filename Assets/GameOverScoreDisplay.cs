using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
