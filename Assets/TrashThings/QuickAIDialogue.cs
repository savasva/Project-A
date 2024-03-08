using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CAINManager;
using UnityEngine.UI;
using JetBrains.Annotations;
using System.Runtime.InteropServices;
using TMPro;
using System.Linq;
using System;

public class QuickAIDialogue : MonoBehaviour
{
    public static QuickAIDialogue singleton;

    public List<CainQuestion> QuestionQueue;

    public TMP_Text dialogueScript;
    public TMP_InputField basicInput;

    public bool inDialogue;

    public LlamaContoller.ColonistModel basicModel;

    

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        QuestionQueue = new List<CainQuestion>();
        Debug.Log(JsonUtility.ToJson(new tempJson()));
    }

    private void LateUpdate()
    {
        if (!inDialogue)
        {
            if(QuestionQueue.Count > 0)
            {
                inDialogue = true;
                DecipherQuestion();               
            }
        }
    }


    public void AddQuestion(CainQuestion question)
    {
        QuestionQueue.Add(question);
    }

    public void SendResponce()
    {
        LlamaContoller.singleton.askQuestion(basicModel, basicInput.text + "\r\nBOB:");
        basicInput.text = "";
    }

    public void EndDialogue()
    {
        LlamaContoller.singleton.jsonParser.prompt = basicModel.ChatHistory 
            + "\n\nBased on this transcript we can form JSON file  with only one variable WasBobRequstFufilled, as follows: {\"WasBobRequstFufilled\":";
        LlamaContoller.singleton.createModel(LlamaContoller.singleton.jsonParser);
        StartCoroutine(WaitForNeural());
    }

    IEnumerator WaitForNeural()
    {
        yield return new WaitForSeconds(5f);
        string response = LlamaContoller.singleton.jsonParser.ChatHistory;
        Debug.Log(response);

        char[] stringArray = response.ToCharArray();
        Array.Reverse(stringArray);
        string reversedStr = new string(stringArray);
        Debug.Log(reversedStr);

        response = "";
        for (int i = 0; i < reversedStr.Length; i++)
        {
            response += reversedStr[i];
            if (reversedStr[i] == '{')
            {
                break;
            }
        }

        Debug.Log(response);

        stringArray = response.ToCharArray();
        Array.Reverse(stringArray);
        reversedStr = new string(stringArray);
        Debug.Log(reversedStr);

        tempJson finalResponce = JsonUtility.FromJson<tempJson>(reversedStr);

        bool approved = finalResponce.WasBobRequstFufilled;
        Debug.Log(approved);


        dialogueScript.text = "";
        //QuestionQueue[0].caller.ReceiveResponse(approved);
        QuestionQueue.RemoveAt(0);
        inDialogue = false;
    }

    class tempJson
    {
        public bool WasBobRequstFufilled;
    }


    public void DecipherQuestion()
    {
        if (QuestionQueue.Count > 0)
        {

            string newPrompt = "Transcript of a dialog, where BOB interacts with CAIN. " +           
            "BOB is a ship colonist travelling in the space, that has a problem and want CAIN to solve it. " +
            "CAIN asnwers swiftly with precision and never lies." +
            "\r\nBOB: Hello, CAIN." +
            "\r\nCAIN: Hello. How may I help you today?" +
            "\r\nBOB: I have a problem I need your help with" +
            "\r\nCAIN: Sure, I will help you." +
            "\r\nBOB: " + QuestionQueue[0].prompt +
            "\r\nCAIN";

            basicModel.prompt = newPrompt;

            LlamaContoller.singleton.createModel(basicModel);


        }
    }

}
