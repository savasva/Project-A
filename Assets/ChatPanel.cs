using LLama.Common;
using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    [Header("Instantiatables")]
    [SerializeField] GameObject crewMessageTemplate;
    [SerializeField] GameObject gaiaMessageTemplate;

    [Header("Editables")]
    [SerializeField] GameObject msgContainer;

    [SerializeField] Button sendButton;

    [SerializeField] TMP_InputField input;

    ColonistModel model;

    public void Init(Colonist col)
    {
        if (col.model == null) return;

        model = col.model;
        //LlamaContoller.inst.CreateModel(model);
        sendButton.onClick.AddListener(() =>
        {
            AddUserMessage(input.text);
            AddCrewMessage(input.text);
            //LlamaContoller.inst.ChatPrompt(this, model, input.text);
            input.text = "";
        });
    }

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void AddUserMessage(string message)
    {
        GameObject msgObj = Instantiate(gaiaMessageTemplate, msgContainer.transform, false);
        msgObj.transform.Find("Content").GetComponent<TMP_Text>().text = message;
    }

    public AsyncChatEntry AddCrewMessage(string prompt)
    {
        GameObject msgObj = Instantiate(crewMessageTemplate, msgContainer.transform, false);
        msgObj.transform.Find("Image/Author").GetComponent<TMP_Text>().text = model.name.ToUpper();
        msgObj.transform.Find("Content").GetComponent<TMP_Text>().text = "...";

        AsyncChatEntry text = msgObj.GetComponent<AsyncChatEntry>();

        //TODO: wtf is a Discard
        _ = model.llm.Chat(prompt, text.Set);

        return text;
    }
}
