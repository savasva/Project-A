using LLama.Common;
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
        LlamaContoller.inst.CreateModel(model);
        sendButton.onClick.AddListener(() =>
        {
            Debug.Log(input.text);
            LlamaContoller.inst.ChatPrompt(this, model, input.text);
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

    public AsyncChatEntry AddMessage(AuthorRole role, ColonistModel model = null)
    {
        return AddMessage(new ChatHistory.Message(role, ""), model);
    }

    public AsyncChatEntry AddMessage(ChatHistory.Message msg, ColonistModel model = null)
    {
        if (msg.AuthorRole == AuthorRole.Assistant && model == null)
        {
            throw new MissingReferenceException("A chat message from the LLM must use the 'model' parameter.");
        }

        GameObject msgObj;
        switch (msg.AuthorRole)
        {
            case AuthorRole.Assistant:
                msgObj = Instantiate(crewMessageTemplate, msgContainer.transform, false);
                msgObj.transform.Find("Image/Author").GetComponent<TMP_Text>().text = model.name.ToUpper();
                break;

            case AuthorRole.User:
            default:
                msgObj = Instantiate(gaiaMessageTemplate, msgContainer.transform, false);
                msgObj.transform.Find("Content").GetComponent<TMP_Text>().text = msg.Content;
                break;
        }

        return msgObj.GetComponent<AsyncChatEntry>();
    }
}
