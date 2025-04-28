using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicUI : MonoBehaviour
{
    [SerializeField] private GameObject MusicBox;
    [SerializeField] public UnityEngine.UI.Button[] buttons;

    public bool MusicOpen { get; private set; }

    public void showMusicBox()
    {
        Player.interactButton.SetActive(false);
        MusicOpen= true;
        MusicBox.SetActive(true);
        StartCoroutine(waitForExit());
    }

    private IEnumerator waitForExit()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        CloseMusicBox();
    }
         
    public void CloseMusicBox()
    {
        if (DialogueUI.instance.DialogueOpen) return;

        Player.interactButton.SetActive(true);
        MusicBox.SetActive(false);
        MusicOpen = false;
    }
}
