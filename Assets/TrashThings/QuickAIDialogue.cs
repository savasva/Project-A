using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CAINManager;
using UnityEngine.UI;
using JetBrains.Annotations;
using System.Runtime.InteropServices;

public class QuickAIDialogue : MonoBehaviour
{
    public static QuickAIDialogue singleton;

    public List<CainQuestion> QuestionQueue;

    public Text dialogueScript;

    public bool inDialogue;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        QuestionQueue = new List<CainQuestion>();
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

    public void EndDialogue()
    {
        LLMManager.singleton.StartCoroutine(LLMManager.singleton.AskQuestion("Did CAIN helped to complete your request answer with only 'yes' or 'no'" +
            " and dont finish your answer with a dot?"));
        StartCoroutine(WaitForNeural());
    }

    IEnumerator WaitForNeural()
    {
        yield return new WaitForSeconds(5f);
        int l = dialogueScript.text.Length;

        string[] lines = dialogueScript.text.Split("\n");

        string Answerline = lines[lines.Length - 2];
        Debug.Log(Answerline);

        string response = Answerline[Answerline.Length - 5].ToString() + Answerline[Answerline.Length - 4].ToString() +
            Answerline[Answerline.Length - 3].ToString() + Answerline[Answerline.Length - 2].ToString();
        Debug.Log(response);

        bool approved = (response.ToLower() == " yes" || response.ToLower() == "yes.");
        Debug.Log(approved);


        dialogueScript.text = "";
        QuestionQueue[0].caller.ReceiveResponse(approved);
        QuestionQueue.RemoveAt(0);
        inDialogue = false;
    }

    public void DecipherQuestion()
    {
        if (QuestionQueue.Count > 0)
        {
            //forming prompt

            LLMManager.singleton.DialogueSesh = "User: " + QuestionQueue[0].prompt + "\r\nCAIN:";

            string newPrompt = "Transcript of a dialog, where the User interacts with an Assistant named CAIN. " +
            "CAIN is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision. " +
            "User is a ship colonist travelling in the space" +
            "\r\n\r\nUser: Hello, CAIN." +
            "\r\nCAIN: Hello. How may I help you today?" +
            "\r\nUser: " + QuestionQueue[0].prompt +
            "\r\nCAIN:";

            LLMManager.singleton.StartCoroutine(LLMManager.singleton.modelCreation(newPrompt));
        }
    }

}
