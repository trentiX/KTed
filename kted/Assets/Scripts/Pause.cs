using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private CanvasGroup prefab;

    public static bool isOpen = false;
    
    public void GoToHome()
    {
        isOpen = false;
        Time.timeScale = 1f;
        
        prefab.DOFade(1, 2).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            SceneManager.LoadScene(0);
        }); 
    }
    
    public void Resume()
    {
        isOpen = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
