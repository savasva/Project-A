using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using TMPro;
using UnityEngine.UI;

public class LLMUnityTest : MonoBehaviour
{
    public LLM llm;
    public TMP_Text Output;
    public TMP_InputField Input;
    public Button Submit;

    private string _submittedText = "";

    void Start()
    {
        Submit.onClick.AddListener(() =>
        {
            _submittedText = Input.text;
            Input.text = "";
            onInputFieldSubmit(_submittedText);
        });
    }

    void onInputFieldSubmit(string message)
    {
        SetInteractable(false);
        Output.text = "...";
        _ = llm.Chat(message, SetAIText, AIReplyComplete);
    }

    public void SetAIText(string text)
    {
        Output.text = text;
    }

    public void AIReplyComplete()
    {
        SetInteractable(true);
        Input.text = "";
    }

    public void CancelRequests()
    {
        llm.CancelRequests();
        AIReplyComplete();
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    private void SetInteractable(bool interactable)
    {
        Submit.interactable = interactable;
        Input.interactable = interactable;
    }

}