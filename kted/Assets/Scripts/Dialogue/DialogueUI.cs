using System.Collections;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject icon;
    [SerializeField] private TextMeshProUGUI text;

    
    public bool DialogueOpen { get; private set; }

    private TypewriterEffect typewriterEffect;

    private LocationsManager _locationsManager;

    private void Awake()
    {
        _locationsManager = FindObjectOfType<LocationsManager>();
    }
    
    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox(text.text);
    }

    public void showDialogue(DialogueObject dialogueObject, string nameOfPerson)
    {
        icon.GetComponent<Image>().sprite = dialogueObject.Sprite;
        text.text = nameOfPerson;
        DialogueOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        if (ChooseLanguageScript.Language == "russian")
        {
            foreach(string dialogue in dialogueObject.DialogueRus)
            {
                yield return RunTypingEffect(dialogue);

                textLabel.text = dialogue;

                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
        }
        
        else if (ChooseLanguageScript.Language == "kazakh")
        {
            foreach(string dialogue in dialogueObject.DialogueKaz)
            {
                yield return RunTypingEffect(dialogue);

                textLabel.text = dialogue;

                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
        }

        CloseDialogueBox(text.text);
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                typewriterEffect.Stop();
            }
        }
    }
         
    private void CloseDialogueBox(string nameOfPerson)
    {
        DialogueOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        if (_locationsManager != null)
        {
            _locationsManager.CheckIfCompleted(nameOfPerson);
        }
    }
}
