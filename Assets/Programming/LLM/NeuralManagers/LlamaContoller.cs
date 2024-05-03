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

    public void Awake()
    {
        inst = this;
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

    public void ChatPrompt(ChatPanel targetPanel, ColonistModel model, string prompt) 
    {
        ChatHistory.Message msg = new ChatHistory.Message(AuthorRole.User, prompt);

        //Add user prompt to chat
        targetPanel.AddMessage(msg);

        //process prompt, where it is added to chat
        ProcessPrompt(targetPanel, model, prompt).Forget();
    }

    async UniTask ProcessPrompt(ChatPanel targetPanel, ColonistModel model, string prompt)
    {
        string response = "";

        ChatHistory.Message userMsg = new ChatHistory.Message(AuthorRole.User, prompt);
        //model.session.AddMessage(userMsg);

        AsyncChatEntry msgUI = targetPanel.AddMessage(AuthorRole.Assistant, model);

        await UniTask.SwitchToThreadPool();

        IAsyncEnumerable<string> chatStream = model.session.ChatAsync(userMsg, false,
            new InferenceParams() {
                Temperature = model.temperature,
                MaxTokens = model.maxTokens,
                AntiPrompts = new List<string> { "User:", "System:" }
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

        msgUI.MarkComplete();
        Debug.Log("Complete!");

        Debug.Log(model.session.History.Messages.Count);
    }
}
