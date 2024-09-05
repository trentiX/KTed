using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogueRus;
    [SerializeField] private Response[] _responses;
    
    [SerializeField][TextArea] private string[] dialogueKaz;
    
    [SerializeField] public Sprite sprite;
    public Sprite Sprite => sprite;
    public string[] DialogueRus => dialogueRus;
    public Response[] Responses => _responses;
    public string[] DialogueKaz => dialogueKaz;
}
