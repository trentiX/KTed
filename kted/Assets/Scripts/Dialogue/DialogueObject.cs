using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogueRus;
    [SerializeField][TextArea] private string[] dialogueKaz;
    [SerializeField] public Sprite sprite;

    public Sprite Sprite => sprite;
    public string[] DialogueRus => dialogueRus;
    public string[] DialogueKaz => dialogueKaz;
}
