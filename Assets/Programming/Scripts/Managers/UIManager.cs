using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;

    public GameObject worldObjContainer;
    public GameObject worldObjTemplate;

    [Header("Indicator")]
    public GameObject colonistIndicatorContainer;
    public GameObject colonistIndicator;

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

    public void CreateColonistIndicator(Colonist col)
    {
        ColonistIndicator indicator = Instantiate(colonistIndicator, colonistIndicatorContainer.transform, false).GetComponent<ColonistIndicator>();

        indicator.Init(col);
    }

    public void BuildRoomUI(Room room)
    {
        foreach (Transform child in worldObjContainer.transform)
        {
            Debug.Log(child.name);
            Destroy(child.gameObject);
        }

        foreach(WorldObject obj in room.contents)
        {
            GameObject curr = Instantiate(worldObjTemplate, worldObjContainer.transform);
            WorldObjectButton btn = curr.GetComponent<WorldObjectButton>();
            btn.Initialize(obj);
        }
    }
}
