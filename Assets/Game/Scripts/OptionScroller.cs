using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionScroller : MonoBehaviour, IScrollHandler
{
    public bool initialized;
    public int optionId;
    public NarrativeManager narrativeManager;
    public ScrollRect scrollRect;

    public string option;

    public TextMeshProUGUI dialogueText;

    private void OnEnable()
    {
        dialogueText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Canvas.ForceUpdateCanvases();
    }

    private void Update()
    {
        if(initialized) return;
            
        if (gameObject.activeSelf)
        {
            initialized = true;
            scrollRect.verticalScrollbar.value = 1;
            
            dialogueText.text = option;
        }
    }

    public virtual void OnScroll(PointerEventData eventData)
    {
        if (scrollRect.verticalScrollbar.gameObject.activeSelf)
        {
            scrollRect.OnScroll(eventData);
        }
    }

    public void OnClickOption()
    {
        narrativeManager.OptionSelected(optionId);
    }

    private void OnDisable()
    {
        initialized = false;
    }
}