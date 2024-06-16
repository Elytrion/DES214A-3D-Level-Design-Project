using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayTextOnTrigger : MonoBehaviour
{
    public TMP_Text UITextElement;
    [TextArea(3, 10)]
    public string TextToDisplay;
    public bool ShouldDisplayOnce = true;

    public bool isWorldSpace = false;

    public GameObject worldSpaceText = null;

    private bool _hasDisplayed = false;

    public void ForceExit()
    {
        if (UITextElement != null)
            UITextElement.text = "";
        if (worldSpaceText != null)
            worldSpaceText.SetActive(false);
        
        if (!_hasDisplayed && ShouldDisplayOnce)
        {
            _hasDisplayed = true;
        }
    }

    public void ForceEnter()
    {
        if (ShouldDisplayOnce && _hasDisplayed)
        {
            return;
        }
        if (UITextElement != null)
            UITextElement.text = TextToDisplay;
        if (worldSpaceText != null)
            worldSpaceText.SetActive(true);
        _hasDisplayed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ShouldDisplayOnce && _hasDisplayed)
            {
                return;
            }
            if (!isWorldSpace)
                UITextElement.text = TextToDisplay;
            else if (worldSpaceText != null)
                worldSpaceText.SetActive(true);

            _hasDisplayed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isWorldSpace)
                UITextElement.text = "";
            else if (worldSpaceText != null)
                worldSpaceText.SetActive(false);
            
            if (!_hasDisplayed && ShouldDisplayOnce)
            {
                _hasDisplayed = true;
            }
        }
    }
}
