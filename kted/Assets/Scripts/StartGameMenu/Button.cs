using System.Collections;
using System.Collections.Generic;
using DataSave;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    private Example _example;
    public void ExitGame()
    {
        _example.Save();
        Application.Quit();
    }

    public void Telegram()
    {
        Application.OpenURL("https://t.me/KTEDchat");
    }
    
    public void Instagram()
    {
        Application.OpenURL("https://www.instagram.com/kted_game/");
    }
}
