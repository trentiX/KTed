using System;
using TMPro;
using UnityEngine;

public class LanguageTextScript : MonoBehaviour
{
    [SerializeField] private String rusText;
    [SerializeField] private String kazText;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        if (ChooseLanguageScript.Language == "kazakh")
        {
            _text.text = kazText;
        }
        
        else if (ChooseLanguageScript.Language == "russian")
        {
            _text.text = rusText;
        }
    }
}
