using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FractionUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    public void ChangeText(Fraction fraction)
    {
        text.text = fraction.numerator.ToString() + "\n" + fraction.denominator.ToString();
    }
}
