using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool phoneIsPicked;
    public Vector3 playerPosition;
    public List<DialogueObject> chatsInStorage;

    public GameData()
    {
        this.phoneIsPicked = false;
        playerPosition = Vector3.zero;
        chatsInStorage = new List<DialogueObject>();
    }
}
