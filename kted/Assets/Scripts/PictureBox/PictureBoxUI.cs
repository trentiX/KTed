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
    public void showPictureBox(Sprite photo, string _text)
    {
        Player.interactButton.SetActive(false);
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
         
    public void ClosePictureBox()
    {
        if (DialogueUI.instance.DialogueOpen) return;
        
        Player.interactButton.SetActive(true);
        PictureBox.SetActive(false);
        PictureOpen = false;
    }
}