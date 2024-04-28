using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptSubmitter : MonoBehaviour
{
    [SerializeField]
    TMP_InputField promptField;

    Button self;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Button>();
        Button.ButtonClickedEvent OnClick = new Button.ButtonClickedEvent();
        OnClick.AddListener(() =>
        {
            LlamaContoller.inst.PromptTest(promptField.text);
        });
    }
}
