using UnityEngine;
using System.Collections.Generic;
using LLama;
using LLama.Common;
using Cysharp.Threading.Tasks;

public class LlamaContoller : MonoBehaviour
{
    private string ModelPath = "llama3-8B-Q2K.gguf";
    //private string ModelPath = "phi-3.gguf";
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
        givenModel.session.AddSystemMessage(givenModel.prompt);

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

    public async void PromptTest (string prompt) {
        UIManager.inst.AddUserMessage(prompt);

        float startTime = Time.realtimeSinceStartup;
        string res = await ProcessPrompt(engineerModel, prompt);
        Debug.LogFormat("Received response in {0} sec(s)\n\n{1}", Time.realtimeSinceStartup - startTime, res);

        UIManager.inst.AddCrewMessage(res);
    }

    async UniTask<string> ProcessPrompt(ColonistModel model, string prompt)
    {
        string response = "";

        ChatHistory.Message userMsg = new ChatHistory.Message(AuthorRole.User, prompt);
        //model.session.AddMessage(userMsg);

        await foreach (var token in model.session.ChatAsync(userMsg, false,
            new InferenceParams() { Temperature = 0.6f, MaxTokens = 55, AntiPrompts = new List<string> { "User:", } }))
        {
            response += token;
        }

        //model.session.AddMessage(new ChatHistory.Message(AuthorRole.Assistant, response));

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
}
