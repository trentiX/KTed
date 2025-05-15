using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void ExitGame()
    {
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
    
    public void TikTok()
    {
        Application.OpenURL("https://www.tiktok.com/@kted_yap?_t=ZM-8wNTVEYCbp9&_r=1");
    }

    public void Github()
    {
        Application.OpenURL("https://github.com/trentiX/KTed");
    }
}
