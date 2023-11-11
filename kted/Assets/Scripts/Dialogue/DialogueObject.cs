using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogueRus;
    [SerializeField][TextArea] private string[] dialogueKaz;
    
    public string[] DialogueRus => dialogueRus;
    public string[] DialogueKaz => dialogueKaz;
}
