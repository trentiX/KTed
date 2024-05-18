using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlatformsChangesForText : MonoBehaviour
{
    [SerializeField] private string mobileText;
    [SerializeField] private string desktopText;
    [SerializeField] private TextMeshProUGUI text;
    private void Start()
    {
#if !UNITY_ANDROID
        text.text = desktopText;
#else
        text.text = mobileText;
#endif
    }
}
