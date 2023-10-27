using LLama;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using LLama.Native;
using System.IO;
using System.Data;
using System.Runtime.CompilerServices;

public class test : MonoBehaviour
{
    public InputField Role;
    public InputField ChatDisplayOutput;

    public string model_path = "wizardLM-7B.ggmlv3.q5_1.bin";
    private static string buff;

    private string question = string.Empty;

    public ChatSession<LLamaModel> _session;
    public LLamaModel model = null;
    private bool IsLoad = false;


    IEnumerator DoModel()
    {
        var outputs = _session.Chat(question, encoding: "UTF-8");
        string tmp = ChatDisplayOutput.text;
        buff = string.Empty;
        foreach (var output in outputs)
        {
            buff += output;
            ChatDisplayOutput.text = tmp + buff;
            // Debug.Log(buff);
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator DoLoad()
    {
        yield return new WaitForSeconds(.1f);
        string full_path = Application.dataPath + "/StreamingAssets/";
        Debug.Log("Start load.");
        model_path = full_path + model_path;
        Debug.Log(model_path + "\n\n");
        Role.text = File.ReadAllText(full_path + "chat-with-bob.txt");
        model = new(new LLamaParams(model: model_path, n_ctx: 1024, interactive: true, temp: 0.94f, top_k: 4, top_p: 0.25f, repeat_penalty: 1.12f, n_gpu_layers: 20, verbose_prompt: false));
        //model = new(new LLamaParams(model: model_path, n_ctx: 1024, interactive: true, repeat_penalty: 1.1f, n_gpu_layers: 24, verbose_prompt: false));
        _session = new ChatSession<LLamaModel>(model)
            .WithPromptFile(full_path + "chat-with-bob.txt")
            .WithAntiprompt(new string[] { "User:" });
        Debug.Log("Model loaded.");
        // I don't want to come up with checks to see if the chat is initiated. Therefore, we initiate it forcibly. Otherwise, there will be problems with the completion and unloading of the DLL.
        ChatDisplayOutput.text = "User: Hi Bob.\n";
        question = ChatDisplayOutput.text;
        StartCoroutine(DoModel());
        Debug.Log("Start Chat");
        ChatDisplayOutput.text = "";
        //yield return new WaitForSeconds(.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        ChatDisplayOutput.text = "Model loading...";
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLoad)
        {
            IsLoad = true;
            StartCoroutine(DoLoad());
        }
    }



    public void LLaMa_Eval()
    {
        question = ChatDisplayOutput.text + "\n";
        Debug.Log("Eval Chat");
        StartCoroutine(DoModel());
    }

    // generate a message when the game shuts down
    void OnDestroy()
    {
        /*
        * For performance, all this does not matter and is not necessary at all. 
        * But when debugging, it creates an eternal problem. Until the DLL is loaded the Unity project cannot be loaded again. And he himself at the end does not unload the loaded DLL. 
        * Therefore, you have to restart the entire environment in order to start the project. This part is needed to simplify debugging. 
        * Allows you to unload the DLL forcibly when calling the end of the application.
        */
        NativeApi.UnloadImportedDll("libllama");
        Debug.Log("Unload all DLL.");
    }

}
