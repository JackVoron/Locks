using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeSelector : MonoBehaviour
{
    public int Digit { get; private set; }
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        Digit = 1;
        _text.text = Digit.ToString();
    }

    public void ChangeDigit(int num)
    {
        Digit += num;
        if (Digit == 10)
            Digit = 0;
        else if (Digit == -1)
            Digit = 9;

        _text.text = Digit.ToString();
    }
}
