using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;
using System;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject icon;
    [SerializeField] private TextMeshProUGUI text;

    
    public bool DialogueOpen { get; private set; }
    public static event Action OnDialogueClosed;

    private TypewriterEffect typewriterEffect;
    private ResponseHandler _responseHandler;
    private bool onButtonCliked = false;
    private LocationsManager _locationsManager;

    private void Awake()
    {
        _locationsManager = FindObjectOfType<LocationsManager>();
    }
    
    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        _responseHandler = GetComponent<ResponseHandler>(); 
        CloseDialogueBox(text.text);
    }

    public void showDialogue(DialogueObject dialogueObject, string nameOfPerson)
    {
        icon.GetComponent<Image>().sprite = dialogueObject.Sprite;
        text.text = nameOfPerson;
        DialogueOpen = true;

        dialogueBox.SetActive(true);
        dialogueBox.GetComponent<CanvasGroup>().alpha = 0;
        dialogueBox.GetComponent<CanvasGroup>().DOFade(1, 0.6f);
        
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {   
        if (ChooseLanguageScript.Language == "russian")
        {
            for (int i = 0; i < dialogueObject.DialogueRus.Length; i++)
            {
                onButtonCliked = false;
                string dialogue = dialogueObject.DialogueRus[i];
                yield return RunTypingEffect(dialogue);

                textLabel.text = dialogue;

                if (i == dialogueObject.DialogueRus.Length - 1 && dialogueObject.HasResponses) break;
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || onButtonCliked);
            }
        }
        
        else if (ChooseLanguageScript.Language == "kazakh")
        {
            foreach(string dialogue in dialogueObject.DialogueKaz)
            {
                onButtonCliked = false;
                yield return RunTypingEffect(dialogue);

                textLabel.text = dialogue;

                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || onButtonCliked);
            }
        }

        if (dialogueObject.HasResponses)
        {
            _responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox(text.text);
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning)
        {
            onButtonCliked = false;
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space) || onButtonCliked)
            {
                typewriterEffect.Stop();
            }
        }
    }
         
    private void CloseDialogueBox(string nameOfPerson)
    {
        DialogueOpen = false;
        
        dialogueBox.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete((() =>
        {
            dialogueBox.SetActive(false);
        }));
        textLabel.text = string.Empty;
        if (_locationsManager != null)
        {
            _locationsManager.CheckIfCompleted(nameOfPerson);
        }
        OnDialogueClosed?.Invoke();
    }
    
    public void OnPointerDown()
    {
         onButtonCliked = true;
    }
}
