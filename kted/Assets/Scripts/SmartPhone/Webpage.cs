using DG.Tweening;
using UnityEngine;

public class Webpage : MonoBehaviour
{
    // Serialized
    [SerializeField] public string url;
    
    // Variables
    private Browser _browser;
    private CanvasGroup _canvasGroup;
    private Tweener _ClosePageAnimation;


    private void Start()
    {
        _browser = FindObjectOfType<Browser>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open()
    {
        Debug.Log("Открытие страницы: " + url);
        _canvasGroup.DOFade(1, 0.1f).OnComplete((() =>
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }));
    }

    public void Close(Webpage webpage) // webpage is page that need to be opened after closing other
    {
        if (_ClosePageAnimation.IsActive()) return;

        Debug.Log("Закрытие страницы: " + url);
        _ClosePageAnimation = _canvasGroup.DOFade(0, 0.1f).OnComplete((() =>
        {
            webpage.Open();
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }));
    }
}
