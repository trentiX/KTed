using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSprite : MonoBehaviour
{
    [SerializeField] public Transform spawnPoint; 
    
    private GameObject[] sprites;
    private List<GameObject> spritesList = new List<GameObject>();

    void Start()
    {
        sprites = MainMenuSprites.instance.spritePrefabs;
        spritesList = MainMenuSprites.instance.spritePrefabsList;
    }
    
    public void SpawnRandomSprites()
    {
        int i = Random.Range(0, sprites.Length);
        GameObject sprite1 = Instantiate(sprites[i], new Vector3
            (-2.5f,spawnPoint.transform.position.y,0), Quaternion.identity);
        spritesList.Add(sprite1);
        sprite1.transform.position = new Vector3(3f, gameObject.transform.position.y, 0);
        
        int j = Random.Range(0, sprites.Length);
        GameObject sprite2 = Instantiate(sprites[j], new Vector3
            (2.5f,spawnPoint.transform.position.y,0), Quaternion.identity);
        spritesList.Add(sprite2);  
        sprite2.transform.position = new Vector3(-3f, gameObject.transform.position.y, 0);
        sprite2.GetComponent<SpriteRenderer>().flipX = true;
    }
    
    public void DeleteSprites()
    {
        foreach (var sprite in spritesList)
        {
            Destroy(sprite);
        }
        spritesList.Clear();
    }
}
