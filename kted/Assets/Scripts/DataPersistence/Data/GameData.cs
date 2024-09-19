using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool phoneIsPicked;
    public Vector3 playerPosition;
    public int chatTurn;
    public SerializableDictionary<string, SerializableDictionary<bool,int>> dialogueObjInteracted;

    public GameData()
    {
        this.phoneIsPicked = false;
        playerPosition = Vector3.zero;
        dialogueObjInteracted = new SerializableDictionary<string, SerializableDictionary<bool, int>>();
        chatTurn = 0;
    }

}