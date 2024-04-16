using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LocationCompleted : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject locComplete;

    private Player _player;

    private void Start()
    {
        locComplete.SetActive(false);

        // Find and assign the Player component in the scene
        _player = FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogWarning("Player component not found in the scene.");
        }
    }

    public void LocationCompletedAnim()
    {
        // Ensure _player is not null before using its location property
        if (_player != null)
        {
            locComplete.SetActive(true);
            StartCoroutine(LocationCompletedAnimation());
        }
        else
        {
            Debug.LogWarning("Player component is null. Cannot animate location completion.");
        }
    }

    public IEnumerator LocationCompletedAnimation()
    {
        text.DOFade(0, 0.01f).SetEase(Ease.OutCubic);
        text.text = "Локация " + _player.location + " пройдена!";
        text.DOFade(1, 3).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(2);
        
        text.DOFade(0, 3).SetEase(Ease.InCubic).OnComplete(() =>
        {
            locComplete.SetActive(false);
        }); 
    }
}