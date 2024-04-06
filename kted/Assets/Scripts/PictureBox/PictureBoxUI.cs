using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PictureBoxUI : MonoBehaviour
{
    [SerializeField] private GameObject PictureBox;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    public bool PictureOpen { get; private set; }

    private void Awake()
    {
        image.GetComponent<Image>();
    }

    private void Start()
    {
        ClosePictureBox();
    }

    public void showPictureBox(Sprite photo, string _text)
    {
        image.sprite = photo;
        text.text = _text;
        PictureOpen = true;
        PictureBox.SetActive(true);
        StartCoroutine(waitForExit());
    }

    private IEnumerator waitForExit()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        ClosePictureBox();
    }
         
    private void ClosePictureBox()
    {
        PictureBox.SetActive(false);
        PictureOpen = false;
    }
}