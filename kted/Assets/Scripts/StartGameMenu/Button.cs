using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    //Components
    public GameObject blackImage;
    public GameObject settingsMenu;
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowSettings()
    {
        blackImage.SetActive(true);
        settingsMenu.SetActive(true);
    }

    public void HideSettigns()
    {
        blackImage.SetActive(false);
        settingsMenu.SetActive(false);
    }
}
