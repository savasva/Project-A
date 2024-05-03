using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptSubmitter : MonoBehaviour
{
    [SerializeField]
    TMP_InputField promptField;

    public ColonistModel modelToPrompt;

    Button self;

    // Start is called before the first frame update
    void Start()
    {
        /*self = GetComponent<Button>();

        self.onClick.AddListener(() =>
        {
            LlamaContoller.inst.Prom(modelToPrompt, promptField.text);
        });*/
    }
}
