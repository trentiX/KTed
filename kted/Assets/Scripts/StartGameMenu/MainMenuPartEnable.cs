using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;


public class MainMenuPartEnable : MonoBehaviour
{
    public List<GameObject> allLabels; // Assign all labels from the Inspector or find them dynamically

    private void OnEnable()
    {
        foreach (var label in allLabels)
        {
            OnActiveAnimation(label);
        }
    }

    private void OnActiveAnimation(GameObject activatedLabel)
    {
        activatedLabel.SetActive(true);
        // Only animate the specific label that was activated
        Vector3 temp = activatedLabel.transform.position;
        activatedLabel.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        
        activatedLabel.transform.DOMove(temp, 3); // Move back to original position over 2 seconds
    }

    public void OnDisActiveAnimation()
    {
        foreach (var deactivatedLabel in allLabels)
        {
            Vector3 temp = deactivatedLabel.transform.position;
            deactivatedLabel.transform.DOMove(new Vector3
                (Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), 3).OnComplete((() =>
            {
                deactivatedLabel.SetActive(false);
                deactivatedLabel.transform.position = temp;
                gameObject.SetActive(false);
            }));
        }
    }
}