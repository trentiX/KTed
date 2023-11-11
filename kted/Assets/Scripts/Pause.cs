using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public static bool isOpen = false;
    
    public void GoToHome()
    {
        isOpen = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    public void Resume()
    {
        isOpen = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
