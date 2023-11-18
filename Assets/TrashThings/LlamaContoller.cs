using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using LLama;
using LLama.Native;
using LLama.Common;
using TMPro;

public class LlamaContoller : MonoBehaviour
{
    public bool use;
    public string InputText;

    private string ModelPath = "Funny/zephyr-7b-beta.Q4_K_M.gguf";
    
    public TMP_Text displayOutput;
    

    public ColonistModel engineerModel;

    public void Update()
    {
        if(use)
        {
            use = false;
            askQuestion(engineerModel);
        }
    }

    public void Start()
    {
        engineerModel = new ColonistModel();

        engineerModel.prompt = "Transcript of a dialogue where the User interacts with an Assistant named CAIN." +
            "CAIN is helpful and asnwers User request's." +
            "User is a ship colonist travelling in the space." +
            "\nUser: Hello Cain. What just happened?" +
            "\nCAIN: Our ship stopped" +
            "\nUser: Can you please open door to Room 1 for me please?" +
            "\nCAIN: ";

        createModel(engineerModel);
    }

    async void createModel(ColonistModel givenModel)
    {      
        // Load a model
        var parameters = new ModelParams(Application.dataPath + "/Programming/LLM/ImportedLLMBases/" + ModelPath)
        {
            ContextSize = 4096,
            Seed = 1337,
            GpuLayerCount = 35
        };
        var model = LLamaWeights.LoadFromFile(parameters);
        // Initialize a chat session
        var context = model.CreateContext(parameters);
        var ex = new InteractiveExecutor(context);
        givenModel.Session = new ChatSession(ex);

        givenModel.ChatHistory = givenModel.prompt;

        givenModel.ChatHistory = givenModel.ChatHistory + " " + InputText + "\nUser: ";

        displayOutput.text = "User: ";

        string buf = "";

        await foreach (var token in ChatConcurrent(givenModel.Session.ChatAsync(givenModel.ChatHistory,
            new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "CAIN:" } })))
        {
            buf += token;
            Debug.Log(buf);
        }

        displayOutput.text += buf;

        Debug.Log("Model Created");

        
    }

    async void askQuestion(ColonistModel givenModel)
    {

        givenModel.ChatHistory = InputText + "\nUser: ";

        displayOutput.text += givenModel.ChatHistory;

        string buf = "";

        await foreach (var token in ChatConcurrent(givenModel.Session.ChatAsync(givenModel.ChatHistory, 
            new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "CAIN:" }})))
        {
            buf += token;
            Debug.Log(buf);
        }

        displayOutput.text += buf;

        
    }

    /// <summary>
    /// Wraps AsyncEnumerable with transition to the thread pool. 
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns>IAsyncEnumerable computed on a thread pool</returns>
    private async IAsyncEnumerable<string> ChatConcurrent(IAsyncEnumerable<string> tokens)
    {
        await foreach (var token in tokens)
        {           
            yield return token;
        }
    }


    [System.Serializable]
    public class ColonistModel
    {
        public string name;
        public string prompt;
        public string ChatHistory;
        public ChatSession Session;
    }
}
