using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Code : MonoBehaviour
{
    [SerializeField] private GameObject _CodeHackMenu;
    [SerializeField] private CodeSelector[] _selectors;
    [SerializeField] private Image[] _statusMarks;

    [SerializeField] private Sprite _unknown;
    [SerializeField] private Sprite _miss;
    [SerializeField] private Sprite _exist;
    [SerializeField] private Sprite _open;

    [SerializeField] private TMP_Text _textTries;
    private int _tries = 0;
    private int[] _digits = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private int[] _code;
    public Action OnHacked, OnHackFailed;

    private void OnEnable()
    {
        foreach (Image image in _statusMarks)
        {
            image.sprite = _unknown;
        }

        _tries = UnityEngine.Random.Range(4, 7);
        _textTries.text = _tries.ToString();

        System.Random rnd = new();

        _digits = _digits.OrderBy(x => rnd.Next()).ToArray();

        _code = new int[4];
        for (int i = 0; i < 4; i++)
        {
            _code[i] = _digits[i];
        }
    }

    public void TryHack()
    {
        bool isOpen = true;
        for (int i = 0; i < 4; i++)
        {
            if (_selectors[i].Digit == _code[i])
            {
                _statusMarks[i].sprite = _open;
            }
            else if (_code.Contains(_selectors[i].Digit))
            {
                isOpen = false;
                _statusMarks[i].sprite = _exist;
            }
            else
            {
                isOpen = false;
                _statusMarks[i].sprite = _miss;
            }
        }
        
        if(isOpen)
        {
            OnHacked?.Invoke();
            _CodeHackMenu.SetActive(false);
        }

        _tries--;
        _textTries.text = _tries.ToString();
        if(_tries == 0)
        {
            OnHackFailed?.Invoke();
            _CodeHackMenu.SetActive(false);
        }
    }
}
