using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}
