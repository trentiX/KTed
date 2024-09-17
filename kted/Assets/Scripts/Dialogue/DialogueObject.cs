using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogueRus;
    [SerializeField][TextArea] private string[] dialogueKaz;
    [SerializeField] public Sprite sprite;
    [SerializeField] public string name;
    [SerializeField] private Response[] _responses;

    public Sprite Sprite => sprite;
    public string Name => name;
    
    public bool HasResponses => Responses != null && Responses.Length > 0;
    public Response[] Responses => _responses;
    
    public string[] DialogueKaz => dialogueKaz;
    public string[] DialogueRus => dialogueRus;
}
