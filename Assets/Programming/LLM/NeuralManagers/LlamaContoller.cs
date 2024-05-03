using UnityEngine;
using System.Collections.Generic;
using LLama;
using LLama.Common;
using Cysharp.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;

public class LlamaContoller : MonoBehaviour
{
    private string ModelPath = "llama3-8B-Q2K.gguf";
    //private string ModelPath = "phi-3.gguf";
    public static LlamaContoller inst;

    public ColonistModel jsonParser;

    public ColonistModel engineerModel;

    public ColonistModel bioengineerModel;

    public void Start()
    {
        inst = this;
        //jsonParser = new ColonistModel();
        CreateModel(engineerModel);

        CreateModel(bioengineerModel);
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

        Debug.Log("Model Created");
    }

    public void PromptTest (ColonistModel model, string prompt) 
    {    
        //Add user prompt to chat
        UIManager.inst.AddUserMessage(model, prompt);

        //float startTime = Time.realtimeSinceStartup;
        //string res = await ProcessPrompt(engineerModel, prompt);

        //process prompt
        ProcessPrompt(model, prompt).Forget();

        //Debug.LogFormat("Received response in {0} sec(s)\n\n{1}", Time.realtimeSinceStartup - startTime, res);

        //UIManager.inst.AddCrewMessage(res);
    }

    public UniTask<ChatHistory.Message> Prompt(ColonistModel model, string prompt, Action<ChatHistory.Message> onComplete = null)
    {
        return ProcessPrompt(model, prompt);
    }

    async UniTask<ChatHistory.Message> ProcessPrompt(ColonistModel model, string prompt)
    {
        string response = "";

        ChatHistory.Message userMsg = new ChatHistory.Message(AuthorRole.User, prompt);
        //model.session.AddMessage(userMsg);

        AsyncChatEntry msgUI = UIManager.inst.AddCrewMessage(model);

        await UniTask.SwitchToThreadPool();

        IAsyncEnumerable<string> chatStream = model.session.ChatAsync(userMsg, false,
            new InferenceParams() {
                Temperature = model.temperature,
                MaxTokens = model.maxTokens,
                AntiPrompts = new List<string> { "User:", }
            }
        );

        await foreach (var token in chatStream)
        {
            response += token;
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                msgUI.Append(token);
            });
            //msgUI.SendMessage("Append", token);
            //await UniTask.SwitchToMainThread();
            //msgUI.Append(token);
            //await UniTask.SwitchToThreadPool();
        }

        await UniTask.SwitchToMainThread();

        //model.session.AddMessage(new ChatHistory.Message(AuthorRole.Assistant, response));
        ChatHistory.Message responseMsg = new ChatHistory.Message(AuthorRole.Assistant, response);

        model.session.AddMessage(responseMsg);

        return responseMsg;
    }
}
