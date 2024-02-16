using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public List<Transform> Graphics;

    public GameObject DialoguePanel;

    public void Start()
    {
        TurnOffAllGraphics(Graphics[0]);
    }

    public void TurnDialoguePanelOnOFF(int on)
    {
        if(on > 0)
        {
            DialoguePanel.SetActive(true);
        }
        else
        {
            DialoguePanel.SetActive(false);
        }
    }

    public void TurnOffAllGraphics(Transform obj)
    {
        foreach (Transform t in Graphics)
        {
            foreach (Transform child in t)
            {
                if (child.name == "mainText")
                {
                    continue;
                }
                child.gameObject.SetActive(false);
            }
        }
    }

    public void TurnGraphicOn(Transform buttonPressed)
    {
        if (buttonPressed.name != "RightDirectoryPanel")
        {
            TurnOffAllGraphics(buttonPressed.parent);
            TurnGraphicOn(buttonPressed.parent);
        }
        foreach (Transform child in buttonPressed)
        {
            child.gameObject.SetActive(true);
        }

    }
}
