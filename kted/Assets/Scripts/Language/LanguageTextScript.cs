using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
