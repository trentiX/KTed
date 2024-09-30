using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Timer : MonoBehaviour
{
    [SerializeField] public float songDuration;
    [SerializeField] private Image _timerFill;

    private float _timePlayed;

    private void Start()
    {
        songDuration = 10;
        _timePlayed = 0;
        _timerFill.fillAmount = 0;
    }

    private void Update()
    {
        if (_timePlayed < songDuration)
        {
            _timePlayed += Time.deltaTime;
            _timerFill.fillAmount = _timePlayed / songDuration;
        }
        else
        {
            Debug.Log("Song is played");
        }
    }
}
