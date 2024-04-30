using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using LLama;
using LLama.Native;
using LLama.Common;
using Cysharp.Threading.Tasks;

public class LlamaContoller : MonoBehaviour
{
    private string ModelPath = "llama3-8B.gguf"; // = "Funny/zephyr-7b-beta.Q4_K_M.gguf";
    public static LlamaContoller inst;

    public ColonistModel jsonParser;

    public ColonistModel engineerModel;

    public void Start()
    {
        inst = this;
        //jsonParser = new ColonistModel();
        CreateModel(engineerModel);
    }

    public void CreateModel(ColonistModel givenModel)
    {
        Debug.Log(Application.dataPath + "/StreamingAssets/" + ModelPath);

        // Load a model
        var parameters = new ModelParams(Application.dataPath + "/StreamingAssets/" + ModelPath)
        {
            ContextSize = 4096,
            Seed = 1337,
            GpuLayerCount = 35
        };
        var model = LLamaWeights.LoadFromFile(parameters);
        // Initialize a chat session
        var context = model.CreateContext(parameters);
        var ex = new InteractiveExecutor(context);
        givenModel.session = new ChatSession(ex);

        //Add System Prompt to history
        //givenModel.chatHistory.AddMessage(AuthorRole.System, givenModel.prompt);

        //string buf = "";

        //await foreach (var token in givenModel.session.ChatAsync(new ChatHistory.Message(AuthorRole.User, givenModel.prompt),
        //    inferenceParams: new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "CAIN:" } }))
        //{
        //    buf += token;
        //    Debug.Log(buf);
        //}

        //givenModel.lastAnswer = buf.Replace("\nCAIN:", "");

        //givenModel.chatHistory += buf;

        Debug.Log("Model Created");
    }

    //public async void askQuestion(ColonistModel givenModel, string givenText)
    //{

    //    string currQuestion = givenText;
    //    givenModel.chatHistory.AddMessage(AuthorRole.User, currQuestion);


    //    string buf = "";

    //    await foreach (var token in ChatConcurrent(givenModel.session.ChatAsync(new ChatHistory.Message(AuthorRole.Assistant, currQuestion),
    //        new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "CAIN:" } })))
    //    {
    //        buf += token;
    //        Debug.Log(buf);
    //    }

    //    //givenModel.chatHistory += buf;

    //    givenModel.lastAnswer = buf.Replace("\nCAIN:", "");
    //}

    public void PromptTest (string prompt) {
        Debug.Log(ProcessPrompt(engineerModel, prompt));
    }

    async UniTask<string> ProcessPrompt(ColonistModel model, string prompt)
    {
        string response = "";
        await foreach (var token in model.session.ChatAsync(new ChatHistory.Message(AuthorRole.User, prompt),
            new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "CAIN:" } }))
        {
            response += token;
        }

        model.session.AddMessage(new ChatHistory.Message(AuthorRole.Assistant, response));

        return response;
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


    [CreateAssetMenu(fileName = "New LLM Model", menuName = "Project A/LLM Model")]
    [System.Serializable]
    public class ColonistModel : ScriptableObject
    {
        public new string name;
        public string lastAnswer;
        [TextArea(3, 10)]
        public string prompt;

        public ChatHistory chatHistory;

        public ChatSession session;
    }
}
