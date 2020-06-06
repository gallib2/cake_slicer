using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOfLivesDisplay: MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    private void OnEnable()
    {
        PlayerStats.OnLivesChanged += UpdateText;
    }

    private void OnDisable()
    {
        PlayerStats.OnLivesChanged -= UpdateText;
    }

    void Start()
    {
        uint numberOfLives = (uint)PlayerStats.Lives;
        UpdateText(numberOfLives);
    }

    private void UpdateText(uint numberOfLives)
    {
        text.text = numberOfLives.ToString();
    }
}
