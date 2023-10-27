using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LLama;
using LLama.Native;
using UnityEngine.UI;


public class LLMManager : MonoBehaviour
{

    public static LLMManager singleton;

    public InputField GivenLine;
    public Text textToOutput;

    public string DialogueSesh = "User: Please tell me a joke?" + "\r\nCAIN:";

    ChatSession<LLamaModel> curSession;

    private void Start()
    {
        singleton = this;
    }

    public void CreateModel()
    {
        //StartCoroutine(modelCreation());
    }

    public void GenOutput()
    {
        Debug.Log(GivenLine.text);
        StartCoroutine(AskQuestion(GivenLine.text));
    }

    public IEnumerator modelCreation(string prompt)
    {
        yield return new WaitForSeconds(.1f);

        string modelPath = Application.dataPath + "/Programming/LLM/ImportedLLMBases/ggml-model-f32-q4_0.bin"; // change it to your own model path

        // Load a model

        LLamaModel model = new(new LLamaParams
        (
                model: modelPath,
                n_ctx: 1024,
                interactive: true,
                temp: 0.4f,
                top_k: 4,
                top_p: 0.25f,
                repeat_penalty: 1.12f,
                n_gpu_layers: 20,
                verbose_prompt: false
        ));

        // Initialize a chat session
        curSession = new ChatSession<LLamaModel>(model).WithPrompt(prompt).WithAntiprompt(new string[] { "CAIN:" });

        // show the prompt
        Debug.Log(prompt);
        UpdateText("");
    }

    public IEnumerator AskQuestion(string textFromInput)
    {
        string question = textFromInput + " User:";
        
        UpdateText(textFromInput + "\nUser:");

        var outputs = curSession.Chat(question, encoding: "UTF-8");
        string buff = string.Empty;
        foreach (var output in outputs)
        {
            buff += output;
            Debug.Log(buff);
            yield return new WaitForSeconds(.01f);
        }

        UpdateText(buff);
    }

    private void UpdateText(string newText)
    {
        DialogueSesh = DialogueSesh + newText;
        textToOutput.text = DialogueSesh;
    }

    /*
    public void OldCreateModel()
    {
        string modelPath = Application.dataPath + "/ImportLLM/llama-2-7b-guanaco-qlora.Q4_K_M.gguf"; // change it to your own model path
        prompt = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Bob.\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe.\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.\r\nUser:"; // use the "chat-with-bob" prompt here.

        // Load a model
        var parameters = new ModelParams(modelPath)
        {
            ContextSize = 1024,
            Seed = 1337,
        };
        using var model = LLamaWeights.LoadFromFile(parameters);

        // Initialize a chat session
        using var context = model.CreateContext(parameters);
        var ex = new InteractiveExecutor(context);
        curSession = new ChatSession(ex);

        // show the prompt
        textToOutput.text = "\n";
        textToOutput.text = textToOutput + prompt;
        //prompt = prompt + " test";

        // run the inference in a loop to chat with LLM
        foreach (var text in curSession.Chat(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
        {
            Debug.Log(text);
            //textToOutput.text = textToOutput + text;
        }
        prompt = textFromInput;

        foreach (var text in curSession.Chat(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
        {
            Debug.Log(text);
            //textToOutput.text = textToOutput + text;
        }

    }

    public void GenOutput()
    {
        // run the inference in a loop to chat with LLM
        foreach (var text in curSession.Chat(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
        {
            textToOutput.text = textToOutput + text;
        }
        prompt = textFromInput;
    }

    */
}
