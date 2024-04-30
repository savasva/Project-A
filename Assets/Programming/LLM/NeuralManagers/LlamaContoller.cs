using UnityEngine;
using System.Collections.Generic;
using LLama;
using LLama.Common;
using Cysharp.Threading.Tasks;
using System.Security.Cryptography;

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

        Debug.Log("Model Created");
    }

    public void PromptTest (string prompt) {
        UIManager.inst.AddUserMessage(prompt);

        //float startTime = Time.realtimeSinceStartup;
        //string res = await ProcessPrompt(engineerModel, prompt);

        ProcessPrompt(engineerModel, prompt).Forget();

        //Debug.LogFormat("Received response in {0} sec(s)\n\n{1}", Time.realtimeSinceStartup - startTime, res);

        //UIManager.inst.AddCrewMessage(res);
    }

    async UniTask<string> ProcessPrompt(ColonistModel model, string prompt)
    {
        string response = "";

        ChatHistory.Message userMsg = new ChatHistory.Message(AuthorRole.User, prompt);
        //model.session.AddMessage(userMsg);

        AsyncChatEntry msgUI = UIManager.inst.AddCrewMessage();

        await UniTask.SwitchToThreadPool();

        IAsyncEnumerable<string> chatStream = model.session.ChatAsync(userMsg, false,
            new InferenceParams() {
                Temperature = 0.6f,
                MaxTokens = 55,
                AntiPrompts = new List<string> { "User:", }
            }
        );

        await foreach (var token in chatStream)
        {
            response += token;
            await UniTask.SwitchToMainThread();
            msgUI.Append(token);
            await UniTask.SwitchToThreadPool();
        }

        await UniTask.SwitchToMainThread();

        //model.session.AddMessage(new ChatHistory.Message(AuthorRole.Assistant, response));

        return response;
    }
}
