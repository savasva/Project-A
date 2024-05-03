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
        model = col.model;
        LlamaContoller.inst.CreateModel(model);
        sendButton.onClick.AddListener(() =>
        {
            AddMessage(new ChatHistory.Message(AuthorRole.User, input.text));
            AddMessage(await LlamaContoller.inst.Prompt(model, input.text));
        });
    }

    public void AddMessage(ChatHistory.Message msg)
    {
        GameObject msgObj;
        switch (msg.AuthorRole)
        {
            case AuthorRole.Assistant:
                msgObj = Instantiate(crewMessageTemplate, msgContainer.transform, false);
                break;

            case AuthorRole.User:
            default:
                msgObj = Instantiate(gaiaMessageTemplate, msgContainer.transform, false);
                break;
        }
    }
}
