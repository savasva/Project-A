using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using LLama;
using LLama.Native;
using LLama.Common;

public class LlamaContoller : MonoBehaviour
{
    private string ModelPath = "Funny/zephyr-7b-beta.Q4_K_M.gguf";

    public ColonistModel engineerModel;

    public void Start()
    {
        engineerModel = new ColonistModel();

        engineerModel.prompt = "Transcript of a dialogue where the User interacts with an Assistant named CAIN." +
            "CAIN is helpful and asnwers User request's." +
            "User is a ship colonist travelling in the space." +
            "\nUser: Hello Cain. What just happened?" +
            "\nCAIN: Our ship stopped" +
            "\nUser: What? How did it happen?" +
            "\nCAIN: I have no idea" +
            "\nUser: ";
    }

    public async void createModel(ColonistModel givenModel)
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

        string buf = "";

        await foreach (var token in ChatConcurrent(givenModel.Session.ChatAsync(givenModel.ChatHistory,
            new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "CAIN:" } })))
        {
            buf += token;
            Debug.Log(buf);
        }

        givenModel.lastAnswer = buf.Replace("\nCAIN:", "");

        givenModel.ChatHistory += buf;

        Debug.Log("Model Created");

        
    }

    public async void askQuestion(ColonistModel givenModel, string givenText)
    {

        string currQuestion = givenText + "\nUser: ";
        givenModel.ChatHistory += currQuestion;


        string buf = "";

        await foreach (var token in ChatConcurrent(givenModel.Session.ChatAsync(currQuestion, 
            new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "CAIN:" }})))
        {
            buf += token;
            Debug.Log(buf);
        }

        givenModel.ChatHistory += buf;

        givenModel.lastAnswer = buf.Replace("\nCAIN:", "");
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
        public string lastAnswer;
        [TextArea(3, 10)]
        public string prompt;
        [TextArea(3, 10)]
        public string ChatHistory;
        public ChatSession Session;
    }
}
