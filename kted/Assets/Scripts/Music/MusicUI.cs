using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicUI : MonoBehaviour
{
    [SerializeField] private GameObject MusicBox;

    public bool MusicOpen { get; private set; }
    
    private void Start()
    {
        CloseMusicBox();
    }

    public void showMusicBox()
    {
        MusicOpen= true;
        MusicBox.SetActive(true);
        StartCoroutine(waitForExit());
    }

    private IEnumerator waitForExit()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        CloseMusicBox();
    }
         
    private void CloseMusicBox()
    {
        MusicBox.SetActive(false);
        MusicOpen = false;
    }
}
