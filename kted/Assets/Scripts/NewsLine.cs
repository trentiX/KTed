using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsLine : MonoBehaviour
{
    [SerializeField] private GameObject newsTextPrefab;
    
    private void Start()
    {
        StartCoroutine(NewsLoop());    
    }
    
    private IEnumerator NewsLoop()
    {
        while (true)
        {
            if (newsTextPrefab.transform.position.x > 100)
            {
                
            }
        }
    }
}
