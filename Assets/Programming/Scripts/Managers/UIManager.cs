using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;

    public GameObject worldObjContainer;
    public GameObject worldObjTemplate;

    [Header("LLM Chat")]
    public GameObject cainMessageTemplate;
    public GameObject crewMessageTemplate;

    void Awake()
    {
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    public void BuildRoomUI(Room room)
    {
        foreach (Transform child in worldObjContainer.transform)
        {
            Debug.Log(child.name);
            Destroy(child.gameObject);
        }

        foreach(WorldObject obj in room.contents.objects)
        {
            GameObject curr = Instantiate(worldObjTemplate, worldObjContainer.transform);
            WorldObjectButton btn = curr.GetComponent<WorldObjectButton>();
            btn.Initialize(obj);
        }
    }

    public void AddUserMessage(ColonistModel model, string msg)
    {
        Transform message = Instantiate(cainMessageTemplate, model.chatMessageContainer.transform).transform;
        message.Find("Content").GetComponent<TMP_Text>().text = msg;
    }

    public AsyncChatEntry AddCrewMessage(ColonistModel model)
    {
         Transform message = Instantiate(crewMessageTemplate, model.chatMessageContainer.transform).transform;

        return message.GetComponent<AsyncChatEntry>();
    }
}
