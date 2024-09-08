using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEditor.Events;
using Random = UnityEngine.Random;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUI _dialogueUI;

    private List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start()
    {
        _dialogueUI = GetComponent<DialogueUI>();
    }

    public void ShowResponses(Response[] responses)
    {
        float responseBoxWidth = 0;

        foreach (var response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);

            /* Button animation with DOTween
            responseButton.transform.Rotate(0, 0, Random.Range(-0.5f, -0.9f));
            responseButton.transform.DORotate(new Vector3(0, 0, Random.Range(0.5f, 0.9f)), Random.Range(3,4))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine); // Optional ease for smoother animation
                */
            
            responseButton.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnPickedResponse(response));
            
            // Add PointerEnter event to change text
            EventTrigger eventTrigger = responseButton.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = responseButton.AddComponent<EventTrigger>();
            }
            
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entry.callback.AddListener((eventData) => OnPointerEnter(responseButton, response));
            eventTrigger.triggers.Add(entry);
            EventTrigger.Entry exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            exitEntry.callback.AddListener((eventData) => OnPointerExit(responseButton, response));
            eventTrigger.triggers.Add(exitEntry);
            

            tempResponseButtons.Add(responseButton);

            responseBoxWidth = responseButtonTemplate.sizeDelta.x;
        }

        responseBox.sizeDelta = new Vector2(responseBoxWidth, responseBox.sizeDelta.y);
        responseBox.gameObject.SetActive(true);
    }

    private void OnPointerEnter(GameObject responseButton, Response response)
    {
        responseButton.GetComponent<TMP_Text>().text = "-> " + response.ResponseText;
    }
    private void OnPointerExit(GameObject responseButton, Response response)
    {
        responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
    }


    public void OnPickedResponse(Response response)
    {
        responseBox.gameObject.SetActive(false);

        foreach (var button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();

        _dialogueUI.showDialogue(response.DialogueObject, response.DialogueObject.name);
    }
}
