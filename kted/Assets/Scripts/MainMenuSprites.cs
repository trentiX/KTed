using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSprites : MonoBehaviour
{
    [SerializeField] public GameObject[] spritePrefabs;
    
    public static MainMenuSprites instance { get; private set; }
    public List<GameObject> spritePrefabsList = new List<GameObject>(); 
    
    private void Awake()
    {
        instance = this;
    }
    
    public void OnHovered(MenuSprite menuSprite)
    { 
        menuSprite.SpawnRandomSprites();
    }
    
    public void OnUnhovered(MenuSprite menuSprite)
    {
        menuSprite.DeleteSprites();
    }
}
