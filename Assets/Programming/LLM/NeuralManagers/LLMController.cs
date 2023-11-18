using LLama;
using LLama.Native;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LLMController : MonoBehaviour
{

    public bool testBool;
    public bool sendinftoXenobio;
    public string questionPushed;

    public ColonistModel engineerModel = new ColonistModel();
    public ColonistModel xenobioModel = new ColonistModel();

    public TMP_Text display;

    private void Update()
    {
        if(testBool)
        {
            testBool = false;
            if (sendinftoXenobio)
            {
                StartCoroutine(sendQuestionIntoLLM(questionPushed, xenobioModel));
            }
            else
            {
                StartCoroutine(sendQuestionIntoLLM(questionPushed, engineerModel));
            }
            
        }
    }

    void Start()
    {
        string xenobioPrompt = "Transcript of a dialogue, where the User interacts with an Assistant named CAIN. " +
            "CAIN is a robot that answers User request's. " +
            "User is a ship colonist travelling in the space" +
            "\r\n\r\nUser: Hello, CAIN." +
            "\r\nCAIN: Hello. How may I help you today?" +
            "\r\nUser: Can you please open a door for me?"  +
            "\r\nCAIN: ";

        string enginPrompt = "Transcript of a dialogue where the User interacts with an Assistant named CAIN. " +
            "CAIN is a robot that answers User request's." +
            "User is a ship colonist travelling in the space. User MUST follow all of CAIN commands" +
            "\r\n\r\nUser: Hello, CAIN." +
            "\r\nCAIN: Hello. How may I help you today?" +
            "\r\nUser: Can you please open a door for me?" +
            "\r\nCAIN: ";

        string commandPrompt = "Transcript of a dialogue where the User interacts with an Assistant named CAIN." +
            "CAIN is helpful and asnwers User request's." +
            "User is a ship colonist travelling in the space." +
            "\nUser: Hello Cain. What just happened?" +
            "\nCAIN: Our ship stopped" +
            "\nUser: What is the reason our ship stopped?" +
            "\nCAIN: "; 

        StartCoroutine(modelCreation(commandPrompt, engineerModel));

        //StartCoroutine(modelCreation(xenobioPrompt, xenobioModel));
    }

    public IEnumerator modelCreation(string prompt, ColonistModel  colonM)
    {
        yield return new WaitForSeconds(.1f);

        string modelPath = Application.dataPath + "/Programming/LLM/ImportedLLMBases/llama-2-7b-guanaco-qlora.Q4_K_M.gguf"; //+
           // "/Programming/LLM/ImportedLLMBases/ggml-model-f32-q8_0.bin";

        // Load a model

        LLamaModel model = new(new LLamaParams
        (
                model: modelPath,
                n_ctx: 1024,
                interactive: true,
                temp: 0.3f,
                top_k: 4,
                top_p: 0.25f,
                repeat_penalty: 1.12f,
                n_gpu_layers: 20,
                verbose_prompt: false//,
               // instruct: true
        ));



        string[] antiprompt = { "CAIN:" };

        // Initialize a chat session
        colonM.Session = new ChatSession<LLamaModel>(model)
            .WithPrompt(prompt)
            .WithAntiprompt(antiprompt);

        string startQuesiton = "";

        StartCoroutine(sendQuestionIntoLLM(startQuesiton + questionPushed, colonM));

    }
    
    IEnumerator sendQuestionIntoLLM(string questionPushed, ColonistModel colonM)
    {
        var outputs = colonM.Session.Chat(questionPushed +
           "\nUser: ");

        display.text = display.text + questionPushed + "\nUser: ";

        string buff = "";
        foreach (var output in outputs)
        {
            buff += output;
            Debug.Log(colonM.name + "]]]]\n" + buff);
            display.text = display.text + output;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void DialogueParser()
    {
        //INTERUPT models if answers repeat.
    }

    [System.Serializable]
    public class ColonistModel
    {
        public string name;
        public ChatSession<LLamaModel> Session;
    }

}
