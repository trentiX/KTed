using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ChooseLanguageScript : MonoBehaviour
{
    public static string Language = "russian";
    [SerializeField] private CanvasGroup prefab;

    
    public void RussianLanguage()
    {
        Language = "russian";
        prefab.DOFade(1, 2).OnComplete(() =>
        {
            SceneManager.LoadScene(0);
        }); 
    }

    public void KazakhLanguage()
    {
        Language = "kazakh";
        prefab.DOFade(1, 2).OnComplete(() =>
        {
            SceneManager.LoadScene(0);
        }); 
    }
}
