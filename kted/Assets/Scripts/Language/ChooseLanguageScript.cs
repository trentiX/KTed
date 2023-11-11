using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLanguageScript : MonoBehaviour
{
    public static string Language = "russian";
    
    public void RussianLanguage()
    {
        Language = "russian";
        SceneManager.LoadScene("StartMenu");
    }

    public void KazakhLanguage()
    {
        Language = "kazakh";
        SceneManager.LoadScene("StartMenu");
    }
}
